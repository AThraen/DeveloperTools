using DeveloperTools.Console.Attributes;
using DeveloperTools.Console.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperTools.Console.NewCommands
{
    [Command(Keyword = "memory")]
    public class Memory : IConsoleCommand
    {
        public string Help => "";

        public string Execute(params string[] parameters)
        {
            Process p = Process.GetCurrentProcess();

            return "Private Memory Used: " + p.PrivateMemorySize64;
        }
    }
}
