namespace Client.Commands
{
    class GetUserName : Command
    {
        public GetUserName(string name) : base(name) { }

        public override string execute(string[] args)
        {
            if (args.Length == 0)
            {
                string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                return $"Username: {userName}";
            }
            else
            {
                return "Invalid arguments for getname.";
            }
        }
    }
}
