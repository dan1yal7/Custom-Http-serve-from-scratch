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
            var wrapperServ = new WrapperService(mock.Object);
            mock.Setup(cl => cl.CreateAndListen());

            //Act 
             wrapperServ.EstablishListenerAndAcceptence();

            //Assert 
            mock.Verify(cl => cl.CreateAndListen(), Times.Once);
            Assert.NotNull(mock.Object);

        }
    }
}
