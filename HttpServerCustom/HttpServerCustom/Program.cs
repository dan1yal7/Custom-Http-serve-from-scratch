using HttpServerCustom.Client;
using HttpServerCustom.Server;
using System.Net.Sockets;

var server = new ServerSide();
_ = Task.Run(server.TcpServerFunction);

await Task.Delay(1000);

var client = new ClientSide();
string response = await client.SocketSendRecieve("127.0.0.1", 8888);
Console.WriteLine(response);
Console.ReadLine();