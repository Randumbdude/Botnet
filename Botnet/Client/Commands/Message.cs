using System;
using System.Threading;
using System.Windows.Forms;

namespace Client.Commands
{
    class Message : Command
    {
        public Message(string name) : base(name) { }

        public override string execute(string[] args)
        {
            if (args.Length == 0) return "Expected argument for msg";
            try
            {
                new Thread(() =>
                {
                    MessageBox.Show(args[0]);
                }).Start();
                return $"showed messagebox: {args[0]}";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
