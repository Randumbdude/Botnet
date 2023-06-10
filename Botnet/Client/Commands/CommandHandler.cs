using System.Collections.Generic;
using System.Linq;

namespace Client.Commands
{
    class CommandHandler
    {
        List<Command> commands;

        public CommandHandler()
        {
            this.commands = new List<Command>();

            this.commands.Add(new Beep("beep"));
            this.commands.Add(new GetUserName("getname"));
            this.commands.Add(new File("file"));
            this.commands.Add(new getToken("gettoken"));
            this.commands.Add(new Message("msg"));
        }

        public string runCommand(string cmd)
        {
            string[] sp = cmd.Split(' ');
            string name = sp.First();
            string[] args = sp.Skip(1).ToArray();

            foreach (Command c in this.commands)
            {
                if (c.name.ToLower() == name)
                {
                    return c.execute(args);
                }
            }

            return $"Command \"{cmd}\" does not exist";
        }
    }
}
