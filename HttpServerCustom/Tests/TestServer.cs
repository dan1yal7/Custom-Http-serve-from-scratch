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
            //Arrange
            var mock = new Mock<ISocketWrapper>();
            int port = 1234;
            var func = new Wrapper();
            mock.Setup(f => f.CreateAndListen(port));

            //Act 
            var result = func.CreateAndListen;

            //Assert 
            Assert.NotNull(result);
        }
    }
}
