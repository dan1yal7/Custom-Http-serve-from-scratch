using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Tests.SocketWrapper
{
    public interface SocketWrapper
    {
        void CreateAndListen(int port);
        string ReceiveData(); 
        void SendData(string data);
    }

    public class Wrapper : SocketWrapper
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
            }
            catch(SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public string ReceiveData()
        {
            throw new NotImplementedException();
        }

        public void SendData(string data)
        {
            throw new NotImplementedException();
        }
    }
}
