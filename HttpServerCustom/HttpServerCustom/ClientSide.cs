using HttpServerCustom.Server;
using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;



ServerSide serverSide = new ServerSide();
serverSide.TcpServerFunction();

var port = 8888;
var url = "127.0.0.1";
var response = await SocketSendRecieve(url, port);
Console.WriteLine(response);


async Task<Socket?> ConnectSocketAsync(string url,  int port)
{
  Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    try
    {
        await tcpSocket.ConnectAsync(url, port);
        return tcpSocket;
    }
    catch(SocketException ex)
    {
        Console.WriteLine(ex.Message);
        tcpSocket.Close();
    }
    return null;
}

async Task<string> SocketSendRecieve(string url, int port)
{
    using Socket? socket = await ConnectSocketAsync(url, port);
    if (socket is null)
    {
        return $"Connection could not be established with {url}";
    }
    var message = $"GET / HTTP/1.1\r\nHost: {url}\r\nConnection: close\r\n\r\n";
    var messageBytes = Encoding.UTF8.GetBytes(message);
    await socket.SendAsync(messageBytes);
    socket.Shutdown(SocketShutdown.Send);
    
    //buffer for recieving data 
    var responseBytes = new byte[512];
    var builder = new StringBuilder();
    int bytes;
    do
    {
        bytes = await socket.ReceiveAsync(responseBytes);
        string response = Encoding.UTF8.GetString(responseBytes, 0, bytes);
        builder.Append(response);
    }
    while (bytes > 0);
    return builder.ToString();
}