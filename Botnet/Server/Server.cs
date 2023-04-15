using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        private static Socket s;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome To Botnet Control Panel");

            //s = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
            //s.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
            //s.Bind(new IPEndPoint(IPAddress.IPv6Any, 32767));

            //Task t = Task.Factory.StartNew(listen);

            while (true)
            {
                Console.Write("> ");
                //Console.Beep();
                sendCmd(Console.ReadLine());
            }
        }

        public static void sendCmd(string cmd)
        {
            Socket s = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
            s.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.79.45.6"), 32767);
            Byte[] data = Encoding.ASCII.GetBytes(cmd);
            s.SendTo(data, ipep);
        }

        public static void listen()
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.IPv6Any, 32767);
            EndPoint ep = ipep as EndPoint;
            Byte[] data = new Byte[65535];
            s.ReceiveFrom(data, ref ep);
            List<Byte> bytes = new List<Byte>(data);
            bytes.RemoveAll(b => b == 0);

            string receivedByte = Encoding.ASCII.GetString(bytes.ToArray());

            Console.WriteLine(receivedByte);

            listen();
        }
    }
}
