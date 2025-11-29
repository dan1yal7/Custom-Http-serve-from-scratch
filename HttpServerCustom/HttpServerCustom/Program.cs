using HttpServerCustom.Client;
using HttpServerCustom.Server;
using System.Net.Sockets;

var server = new ServerSide();
_ = Task.Run(server.TcpServerFunction);

await Task.Delay(1000);

var client = new ClientSide();
var port = 8888;
var url = "127.0.0.1";
string response = await client.SocketSendRecieve(url, port);
Console.WriteLine(response);
Console.ReadLine();