using HttpServerCustom.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HttpServerCustom.Encryption
{
    public class ServerEncryption
    {
        private readonly ServerSide _serverside;
        public ServerEncryption(ServerSide serverside)
        {
            _serverside = serverside;
        }
        static X509Certificate servercertificate = null;

        public void RunServer(string certificate)
        {
            servercertificate = X509Certificate.CreateFromCertFile(certificate);
            _serverside.TcpServerFunction();
        }


    }
}
