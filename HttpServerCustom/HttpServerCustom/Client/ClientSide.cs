using HttpServerCustom.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HttpServerCustom.Client
{ 
    public class ClientSide
    {
        public async Task<Socket?> ConnectSocketAsync(string url, int port)
        { 
            Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                await tcpSocket.ConnectAsync(url, port);
                return tcpSocket;
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
                tcpSocket.Close();
            }
            return null;
        }
       public async Task<string> SocketSendRecieve(string url, int port)
       {
            using Socket? socket = await ConnectSocketAsync(url, port);
            if (socket is null)
            {
                return $"Connection could not be established with {url}";
            }
            using var netStream = new NetworkStream(socket, ownsSocket: true);
            using var sslStream = new SslStream(netStream, false, (sender, cert, cahin, errors) => true);
            await sslStream.AuthenticateAsClientAsync(url);

            var message = $"GET / HTTP/1.1\r\nHost: {url}\r\nConnection: close\r\n\r\n";
            var messageBytes = Encoding.UTF8.GetBytes(message);
            await sslStream.WriteAsync(messageBytes, 0, message.Length);
            await sslStream.FlushAsync();

            //buffer for recieving data 
            byte[] responseBytes = new byte[512];
            var builder = new StringBuilder();
            int bytes;
            while ((bytes = await sslStream.ReadAsync(responseBytes, 0, responseBytes.Length)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(responseBytes, 0, bytes));
            }
            
            return builder.ToString();
        }
    }
}
