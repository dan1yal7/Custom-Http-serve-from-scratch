using System.Net;
using System.Net.Sockets;
using System.Text;

TcpListener server = new TcpListener(IPAddress.Any, 8888);

try
{
   server.Start();
   Console.WriteLine("Server started. Waiting for the connection");

    while(true)
    {
        var tcpClient = await server.AcceptTcpClientAsync();  //wait for the client
    }
}
catch(SocketException ex)
{
    Console.WriteLine($"Som went wrong... {ex.Message}");
}
finally
{
    server?.Stop();
}

//static void ProcessClientAsync(TcpClient tcpClient)
//{
//    string Html = "<html><body><h1>It works!</h1></body></html>";

//    var response = "HTTP/1.1 200 OK\nContent-type: text/html\nContent-Length:" + Html.Length.ToString() + "\n\n" + Html;

//    var responseBytes = Encoding.UTF8.GetBytes(response);

//    tcpClient.GetStream().Write(responseBytes, 0, responseBytes.Length);

//    tcpClient.Close();

//}



