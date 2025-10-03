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
        public async void TcpServerFunction()
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 8888);
            using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipEndPoint);
            socket.Listen(1000);
            using Socket client = await socket.AcceptAsync();
            Console.WriteLine($"Adress of connected client:{client.RemoteEndPoint}");
        }
    }
}
    