using HttpServerCustom.Server;
using Moq;
using System.Net.Sockets;
using Xunit;

namespace Tests
{
    public class TestServer
    {
        [Fact]
        public async Task TcpServerFunc()
        {
            //Arrange 
            var mock = new Mock<Socket>();
            var clientSide = new ServerSide();

        }
    }
}
