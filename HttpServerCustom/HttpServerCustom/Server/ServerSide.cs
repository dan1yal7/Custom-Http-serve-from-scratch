using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HttpServerCustom.Server
{
    public class ServerSide
    {
        private Socket ?_socket;
        private static X509Certificate2? _serverCertificate; 
        public ServerSide()
        {
            var certPath = Path.Combine(AppContext.BaseDirectory, "server.pfx");
            if (!File.Exists(certPath))
            {
                Console.WriteLine($"Certificate not found: {certPath}");
                throw new FileNotFoundException("Certificate not found", certPath);
            }
            _serverCertificate = new X509Certificate2(certPath, "password");
        }
        public async void TcpServerFunction()
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 8888);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(ipEndPoint);
            _socket.Listen(1000);

            Console.WriteLine("Waiting for a client to connect...");

            while (true)
            {
                using Socket client = await _socket.AcceptAsync();
                Console.WriteLine($"Client connected: {client.RemoteEndPoint}");
                await ProcessClientAsync(client);
            }
        } 
        private async Task ProcessClientAsync(Socket client)
        {
          using var netStream = new NetworkStream(client, ownsSocket: true);
          using var sslStream = new SslStream(netStream, leaveInnerStreamOpen: false);

            try
            {
                sslStream.AuthenticateAsServer(_serverCertificate!, clientCertificateRequired: false, 
                enabledSslProtocols: System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13,
                checkCertificateRevocation: true);

                // 1. Getting request 
                byte[] buffer = new byte[4096];
                int received = await sslStream.ReadAsync(buffer, 0, buffer.Length);
                string request = Encoding.UTF8.GetString(buffer, 0, received);

                Console.WriteLine("===HTTP Request over SSL");
                Console.WriteLine(request);

                //Forming Response
                string body = "<h1>Hello from custom HTTP server!</h1>";
                string response = "Http/1.1 200 OK\r\n" + "Content-Type: text/html; charset=UTF-8\r\n" + $"Content-Length: {Encoding.UTF8.GetByteCount(body)}\r\n" + "\r\n" + body;

                //Send Response
                byte[] sendResponse = Encoding.UTF8.GetBytes(response);
                await sslStream.WriteAsync(sendResponse, 0, sendResponse.Length);
            }
            catch (AuthenticationException e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
                if (e.InnerException != null)
                {
                    Console.WriteLine("Inner exception: {0}", e.InnerException);
                }
                Console.WriteLine("Authentication failed - closing the connection");
                sslStream.Close();
                client.Close();
                return;
            }

        }
    }
}
    