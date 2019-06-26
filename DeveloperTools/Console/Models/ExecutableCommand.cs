using DeveloperTools.Console.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperTools.Console.Models
{
    public class ExecutableCommand
    {
        public IConsoleCommand  Command{ get; set; }
        public string[] Parameters { get; set; }
    }
}
