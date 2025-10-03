using HttpServerCustom.Server;
using System.Net.Sockets;
using System.Text;


ServerSide serverSide = new ServerSide();
serverSide.TcpServerFunction();

var port = 8888;
var url = "127.0.0.1";

using Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
try
{
    await tcpSocket.ConnectAsync(url, port);
    var message = $"GET / HTTP/1.1\r\nHost: {url}\r\nConnection: close\r\n\r\n";
    var messageBytes = Encoding.UTF8.GetBytes(message);
    await tcpSocket.SendAsync(messageBytes);

    tcpSocket.Shutdown(SocketShutdown.Send);

    var responseBytes = new byte[512];

    var builder = new StringBuilder();
    int bytes;
    do
    {
        bytes = await tcpSocket.ReceiveAsync(responseBytes);
        string response = Encoding.UTF8.GetString(responseBytes, 0, bytes);
        builder.Append(response);
    }
    while (bytes > 0);
    Console.WriteLine(builder);

}
catch(SocketException ex)
{
    Console.WriteLine($"{ex.Message}");
}
