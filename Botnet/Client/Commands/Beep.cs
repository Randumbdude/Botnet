using System;
using System.Linq;

namespace Client.Commands
{
    class Beep : Command
    {
        public Beep(string name) : base(name) { }

        public override string execute(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Beep();
                return "beep";
            }
            else if (args.Length == 1)
            {
                int i = 0;
                while (i != Int32.Parse(args.First()))
                {
                    Console.Beep();
                    i++;
                }
                return $"beep {i}";
            }
            else
            {
                return "Invalid arguments for beep. Requires either 0 or 1 arguments";
            }
        }
    }
}
