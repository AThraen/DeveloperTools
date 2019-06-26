using CodeArtCommandsExperiment.CodeArtCommand;
using DeveloperTools.Console.Attributes;
using DeveloperTools.Console.Interfaces;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperTools.Console.NewCommands
{
    [Command(Keyword = "help", Description ="Provides help using the console")]
    public class HelpCommand : IConsoleOutputCommand, IConsoleCommand
    {
        public event OutputToConsoleHandler OutputToConsole;

        public string Execute(params string[] parameters)
        {
            //Retrieve a list of commands
            var cmdMgr = ServiceLocator.Current.GetInstance<CommandManager>();
            foreach(var cmd in cmdMgr.Commands.Values)
            {
                OutputToConsole?.Invoke(this, cmd.Keyword);
                if(cmd.Info.Description!=null)  OutputToConsole?.Invoke(this, "&nbsp;&nbsp;"+cmd.Info.Description);
            }
            return null;
        }
    }
}
