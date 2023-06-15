using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    public class ConnectedClient
    {
        public TcpClient Client { get; set; }
        public Thread ClientThread { get; set; }
        public NetworkStream Stream { get; set; }
        public bool IsRunning { get; set; }

        public ConnectedClient(TcpClient client)
        {
            Client = client;
            Stream = client.GetStream();
            IsRunning = true;

            ClientThread = new Thread(HandleClient);
            ClientThread.Start();
        }

        public void HandleClient()
        {
            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = Stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    IPEndPoint remoteIpEndPoint = Client.Client.RemoteEndPoint as IPEndPoint;

                    string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\r" + data);
                    Console.ForegroundColor = ConsoleColor.White;

                    Console.Write("\r> ");
                    //receive
                    //byte[] responseBuffer = Encoding.ASCII.GetBytes(data);
                    //Stream.Write(responseBuffer, 0, responseBuffer.Length);
                }
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\rClient disconnected.");
                Console.ForegroundColor = ConsoleColor.White;

                Server.server.connectedClients.Remove(this);
                Console.Title = $"Server (Bot Count: {Server.server.connectedClients.Count})";
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                Server.server.connectedClients.Remove(this);
                Console.Title = $"Server (Bot Count: {Server.server.connectedClients.Count})";

                Console.Write("\r> ");
            }
            finally
            {
                Cleanup();
            }
        }

        public void SendData(string data)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            Stream.Write(buffer, 0, buffer.Length);
        }

        public void Cleanup()
        {
            IsRunning = false;
            Stream.Close();
            Client.Close();
        }
    }
}
