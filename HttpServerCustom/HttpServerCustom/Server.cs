using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;


var port = 80;
var url = "www.google.com";
var response = await SocketSendRecieveAsync(url, port);
Console.WriteLine(response);

async Task<Socket?> ConnectSocketAsync(string url,  int port)
{
    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    try
    {
        await socket.ConnectAsync(url, port);
        return socket;
    }
    catch (SocketException ex)
    {
        Console.WriteLine(ex.Message);
        socket.Close();
    }
    return socket;
}

async Task<string> SocketSendRecieveAsync(string url, int port)
{
    using Socket? socket = await ConnectSocketAsync(url, port);
    if (socket == null)
    {
        return $"Can not connect to the this url: {url}";
    }

    // Отправляем данные 
    var requestMessage = $"GET / HTTP/1.1\r\nHost: {url}\r\nConnezction: Close\r\n\r\n";
    var requestMessageBytes = Encoding.UTF8.GetBytes(requestMessage);
    await socket.SendAsync(requestMessageBytes);

    //Получаем данные 

    int bytes;

    var responseBytes = new byte[512];
    var builder = new StringBuilder();
    do
    {
        bytes = await socket.ReceiveAsync(responseBytes);
        string responsePart = Encoding.UTF8.GetString(responseBytes, 0, bytes);
        builder.Append(responsePart);
    }
    while (bytes > 0);
    return builder.ToString();
}


