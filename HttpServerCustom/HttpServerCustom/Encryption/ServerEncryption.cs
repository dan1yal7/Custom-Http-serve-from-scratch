using HttpServerCustom.Client;
using HttpServerCustom.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HttpServerCustom.Encryption
{
    public class ServerEncryption
    {
        static X509Certificate servercertificate = null;

        public void RunServer(string certificate)
        {
            servercertificate = X509Certificate.CreateFromCertFile(certificate);

            TcpListener listener = new TcpListener(System.Net.IPAddress.Any, 8888);
            listener.Start();
            while (true)
            {
                Console.WriteLine("Connection...");
                TcpClient client = listener.AcceptTcpClient();
                ProcessClient(client);
            }
        }   

        static void ProcessClient(TcpClient client)
        { 
            SslStream stream = new SslStream(client.GetStream(), false);

            try
            {
                stream.AuthenticateAsServer(servercertificate, clientCertificateRequired: false, checkCertificateRevocation: true);
                stream.ReadTimeout = 5000;
                stream.WriteTimeout = 5000;

                Console.WriteLine("Waiting for client message...");
                string messageData = ReadMessage(stream);
                Console.WriteLine("Received: {0}", messageData);

                byte[] message = Encoding.UTF8.GetBytes("Hi from the server. <EOF>");
                Console.WriteLine("Sending hi");
                stream.Write(message);
            }
            catch (AuthenticationException e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
                if (e.InnerException != null)
                {
                    Console.WriteLine("Inner exception: {0}", e.InnerException.Message);
                }
                Console.WriteLine("Authentication failed - closing the connection.");
                stream.Close();
                client.Close();
                return;
            }
            finally
            {
                // The client stream will be closed with the sslStream
                // because we specified this behavior when creating
                // the sslStream.
                stream.Close();
                client.Close();
            }
            static string ReadMessage(SslStream sslStream)
            {
                // Read the  message sent by the client.
                // The client signals the end of the message using the
                // "<EOF>" marker.
                byte[] buffer = new byte[2048];
                StringBuilder messageData = new StringBuilder();
                int bytes = -1;
                do
                {
                    // Read the client's test message.
                    bytes = sslStream.Read(buffer, 0, buffer.Length);

                    // Use Decoder class to convert from bytes to UTF8
                    // in case a character spans two buffers.
                    Decoder decoder = Encoding.UTF8.GetDecoder();
                    char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
                    decoder.GetChars(buffer, 0, bytes, chars, 0);
                    messageData.Append(chars);
                    // Check for EOF or an empty message.
                    if (messageData.ToString().IndexOf("<EOF>") != -1)
                    {
                        break;
                    }
                } while (bytes != 0);

                return messageData.ToString();
            }
        }
    }
}
