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
        string ReceiveData(); 
        void SendData(string data);
        void Close();
    }

    public class Wrapper : ISocketWrapper
    { 
        public void CreateAndListen(int port)
        {
            try
            {
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 8888);
                using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(iPEndPoint);
                socket.Listen(1000);
                using Socket client = socket.Accept();
                Console.WriteLine($"Adress of connected client:{client.RemoteEndPoint}");
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private StreamReader? input;
        public string ReceiveData()
        {   
            try
            {
                return input!.ReadLine() ?? ("");
            }
            catch (IOException)
            {
                throw new SocketException();
            }
        }

        private StreamWriter? output;
        public void SendData(string data)
        {
            output!.WriteLine(data);
        }

        public void Close()
        {
            using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            { 
                output?.Close(); 
                input?.Close();
                socket?.Close();
                
            }
            catch (IOException)
            {
                throw new SocketException();
            }
        }
    }
}
