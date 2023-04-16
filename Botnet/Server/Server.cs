﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        const int STD_INPUT_HANDLE = -10;

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CancelIoEx(IntPtr handle, IntPtr lpOverlapped);

        //setting server ip address and port
        private static TcpListener server = new TcpListener(IPAddress.Parse("127.79.45.6"), 32767);

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"   ._________________.
   |.---------------.|
   ||               ||
   ||   -._ .-.     ||
   ||   -._| | |    ||
   ||   -._|`|`|    ||
   ||   -._|.-.|    ||
   || ______________||
   /.-.-.-.-.-.-.-.-.\
  /.-.-.-.-.-.-.-.-.-.\
 /.-.-.-.-.-.-.-.-.-.-.\
/ ______ / _______\___o_\
\_______________________/");
            Console.WriteLine("\nWelcome To The Control Center\n");
            Console.ForegroundColor = ConsoleColor.White;

            server.Start();  // this will start the server

            while (true)
            {
                Task t = Task.Factory.StartNew(listen);

                Console.Write("> ");
                Console.ReadLine();
            }
        }

        private static void listen()
        {
            TcpClient client = server.AcceptTcpClient();  //if a connection exists, the server will accept it

            NetworkStream ns = client.GetStream(); //networkstream is used to send/receive messages

            byte[] data = new byte[100];   //any message must be serialized (converted to byte array)

            data = Encoding.Default.GetBytes("Server Received Transmission");  //conversion string => byte array

            ns.Write(data, 0, data.Length);     //sending the message


            while (client.Connected)  //while the client is connected, we look for incoming messages
            {
                try
                {
                    byte[] msg = new byte[1024];     //the messages arrive as byte array
                    ns.Read(msg, 0, msg.Length);   //the same networkstream reads the message sent by the client
                    Console.WriteLine("\r" + Encoding.ASCII.GetString(msg).Trim('\0')); //now , we write the message as string
                    Console.Write("> ");
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\r" + e.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("> ");
                }
            }
        }
    }
}
/*
            new Thread(() =>
            {
            }).Start();
*/