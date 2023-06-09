using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Commands
{
    class getCount : Command
    {
        public getCount(string name) : base(name) { }

        public override string execute(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Beep();
                return "count return";
            }
            else
            {
                return "Invalid arguments for getcount";
            }
        }
    }
}
