using HttpServerCustom.Server;
using Moq;
using System.Net.Sockets;
using System.Text;
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

        [Fact]
        public void TcpServerSendData()
        { 
            //Arrange
            var mock = new Mock<ISocketWrapper>();
            var wrapperServ = new WrapperService(mock.Object);
            mock.Setup(s => s.SendData(It.IsAny<byte[]>()));

            byte[] dataSend = Encoding.UTF8.GetBytes("HTTP/1.1 200 OK\r\n");
            
            //Act
            wrapperServ.SendDta(dataSend);

            //Assert
            mock.Verify(s => s.SendData(dataSend), Times.Once);

        }
    }
}
