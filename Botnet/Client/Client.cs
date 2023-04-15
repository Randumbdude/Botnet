using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Client
    {
        private static Socket s;

        static void Main(string[] args)
        {
            s = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
            s.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
            s.Bind(new IPEndPoint(IPAddress.IPv6Any, 32767));

            Task t = Task.Factory.StartNew(listen);

            Console.WriteLine("...");
            Console.ReadKey();
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

            if (receivedByte == "getIP")
            {
                //Console.WriteLine("User IP: " + GetUserIP());
                sendIP($"Persons IP: {GetIPAddress()}");
            }
            listen();
        }

        static string GetIPAddress() //public ip
        {
            try
            {
                String address = "";
                WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
                using (WebResponse response = request.GetResponse())
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    address = stream.ReadToEnd();
                }

                int first = address.IndexOf("Address: ") + 9;
                int last = address.LastIndexOf("</body>");
                address = address.Substring(first, last - first);

                return address;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public static void sendIP(string IP)
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.79.45.6"), 32767);
            Byte[] data = Encoding.ASCII.GetBytes(IP);
            s.SendTo(data, ipep);
        }
    }
}
