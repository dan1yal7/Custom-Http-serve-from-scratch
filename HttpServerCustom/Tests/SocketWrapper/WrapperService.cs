using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.SocketWrapper
{
    public class WrapperService
    {
        private readonly ISocketWrapper _socketWrapper;
        public WrapperService(ISocketWrapper socketWrapper)
        {
            _socketWrapper = socketWrapper;
        }

        public void EstablishListenerAndAcceptence()
        {
            _socketWrapper.CreateAndListen();
        }

        public void SendDta(byte[] data)
        {
            _socketWrapper.SendData(data);
        }
        
        public string GetData()
        {
            byte[] data = _socketWrapper.ReceiveData(1024);
            return Encoding.UTF8.GetString(data);
        }
    }

}
