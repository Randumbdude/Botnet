﻿using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Client
{
    class Client
    {
        //IP address to connect to
        private static string address = "127.79.45.6";

        private static TcpClient tcpclnt = new TcpClient();

        static void Main(string[] args)
        {
            bool writeDots = true;
            while (true)
            {
                try
                {

                    Console.Write("Connecting...");

                    new Thread(() =>
                    {
                        while (writeDots)
                        {
                            Console.Write(".");
                            Thread.Sleep(100);
                        }
                    }).Start();

                    tcpclnt.Connect(address, 32767);
                    writeDots = false;
                    // use the ipaddress as in the server program

                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("COMPLETE");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("]");

                    String str = $"Incoming Connection From: {GetIPAddress().Trim()}";
                    Stream stm = tcpclnt.GetStream();

                    ASCIIEncoding asen = new ASCIIEncoding();
                    byte[] ba = asen.GetBytes(str);
                    Console.Write("\nTransmitting...");

                    new Thread(() =>
                    {
                        while (writeDots)
                        {
                            Console.Write(".");
                            Thread.Sleep(100);
                        }
                    }).Start();

                    stm.Write(ba, 0, ba.Length);
                    writeDots = false;

                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("COMPLETE");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("]");

                    byte[] bb = new byte[100];
                    int k = stm.Read(bb, 0, 100);

                    for (int i = 0; i < k; i++)
                        Console.Write(Convert.ToChar(bb[i]));
                }
                catch (Exception e)
                {
                    writeDots = false;
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("ERROR");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("]\n");

                    Console.WriteLine(e.Message);
                }
                Console.ReadKey();
                Console.Clear();
            }
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
                return e.Message;
            }
        }
    }
}