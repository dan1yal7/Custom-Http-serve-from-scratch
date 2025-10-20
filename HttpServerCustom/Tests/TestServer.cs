using HttpServerCustom.Server;
using Moq;
using System.Net.Sockets;
using Tests.SocketWrapper;
using Xunit;

namespace Tests
{
    public class TestServer
    {
        [Fact]
        public void TcpServerCreateAndListen()
        {
            var mock = new Mock<ISocketWrapper>();
            int port = 1234;
            mock.Setup(s => s.CreateAndListen(port));

        }
    }
}
