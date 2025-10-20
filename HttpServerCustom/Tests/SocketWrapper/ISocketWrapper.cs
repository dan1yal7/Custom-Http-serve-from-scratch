using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Tests.SocketWrapper
{
    public interface ISocketWrapper
    {
        void CreateAndListen(int port);
        byte[] ReceiveData(int bufferSize); 
        void SendData(byte[] data);
        void Close();
    }

    public class Wrapper : ISocketWrapper
    {  
        private Socket _socket;
        
        public Wrapper()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void CreateAndListen(int port)
        {
            try
            {
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 8888);                
                _socket.Bind(iPEndPoint);
                _socket.Listen(1000);
                using Socket client = _socket.Accept();
                Console.WriteLine($"Adress of connected client:{client.RemoteEndPoint}");
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public byte[] ReceiveData(int bufferSize)
        {
            var getRequestBytes = new byte[bufferSize];
            int received = _socket.Receive(getRequestBytes);
            string request = Encoding.UTF8.GetString(getRequestBytes, 0, received);
            return getRequestBytes;
        }
 
        public void SendData(byte[] data)
        {
           _socket.Send(data);
        }

        public void Close()
        {
            _socket.Close();
        }
    }
}
