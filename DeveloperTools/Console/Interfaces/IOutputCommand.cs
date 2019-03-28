using DeveloperTools.Console.NewCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperTools.Console.Interfaces
{
    public delegate void CommandOutput(IOutputCommand sender, object output);

    public interface IOutputCommand : IConsoleCommand
    {
        event CommandOutput OnCommandOutput;
    }
}
