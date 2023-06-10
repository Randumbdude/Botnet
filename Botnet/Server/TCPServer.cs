using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class TCPServer
    {
        private TcpListener listener;
        private bool isRunning;
        public List<ConnectedClient> connectedClients;

        public bool serverStarted = false;

        public void StartServer()
        {
            listener = new TcpListener(IPAddress.Parse("127.79.45.6"), 32767);
            listener.Start();
            isRunning = true;
            connectedClients = new List<ConnectedClient>();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Server started");
            Console.ForegroundColor = ConsoleColor.White;
            serverStarted = true;

            while (isRunning)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\rClient connected");
                Console.ForegroundColor = ConsoleColor.White;

                Console.Write("\r> ");

                ConnectedClient connectedClient = new ConnectedClient(client);
                connectedClients.Add(connectedClient);
                Console.Title = $"Server (Bot Count: {connectedClients.Count})";
            }
        }

        public void StopServer()
        {
            isRunning = false;
            listener.Stop();
            Console.WriteLine("Server stopped.");

            foreach (ConnectedClient client in connectedClients)
            {
                client.Cleanup();
            }

            connectedClients.Clear();
        }

        public void SendToAllClients(string data)
        {
            foreach (ConnectedClient client in connectedClients)
            {
                if (client.IsRunning)
                {
                    client.SendData(data);
                }
            }
        }

        public int GetConnectedClientsCount()
        {
            return connectedClients.Count;
        }
    }
}
