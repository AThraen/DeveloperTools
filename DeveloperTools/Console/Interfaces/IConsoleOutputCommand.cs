using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperTools.Console.Interfaces
{
    /// <summary>
    /// Use this for commands that outputs to console
    /// </summary>
    interface IConsoleOutputCommand
    {
        event OutputToConsoleHandler OutputToConsole;
    }

    public delegate void OutputToConsoleHandler(IConsoleCommand sender, string message);
}
