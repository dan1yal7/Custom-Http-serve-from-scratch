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
        public async Task TcpServerFunc()
        {
            var mock = new Mock<ISocketWrapper>();

        }
    }
}
