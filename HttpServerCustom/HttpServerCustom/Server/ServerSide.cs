using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace HttpServerCustom.Server
{
    public class ServerSide
    {
        private Socket ?_socket;
        public async void TcpServerFunction()
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 8888);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(ipEndPoint);
            _socket.Listen(1000);   
            using Socket client = await _socket.AcceptAsync();
            Console.WriteLine($"Adress of connected client:{client.RemoteEndPoint}");
        }

        public async void TcpServerRequestResponse()
        {
            // 1. Getting request 
            var getRequestBytes = new byte[512];
            int received = await _socket.ReceiveAsync(getRequestBytes, SocketFlags.None);
            string request = Encoding.UTF8.GetString(getRequestBytes, 0, received);
            Console.WriteLine("=== Http Request ===");
            Console.WriteLine(request);

            //2. Forming response
            string body = "<h1>Hello from custom HTTP server!</h1>";
            string response = "HTTP/1.1 200 OK\r\n" + "Content-Type: text/html; charset=UTF-8\r\n" + $"Content-Length: {Encoding.UTF8.GetByteCount(body)}\r\n" + "\r\n" + body;

            // Send response
            byte[] sendResponse = Encoding.UTF8.GetBytes(response);
            await _socket.SendAsync(sendResponse, SocketFlags.None);
            _socket.Shutdown(SocketShutdown.Both);
        }
    }
}
    