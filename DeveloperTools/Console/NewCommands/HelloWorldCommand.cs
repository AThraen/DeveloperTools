using DeveloperTools.Console.Attributes;
using DeveloperTools.Console.Interfaces;
using DeveloperTools.Console.NewCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeArtCommandsExperiment.CodeArtCommand.NewCommands
{
    [Command(Keyword ="hello")]
    public class HelloWorldCommand : IConsoleCommand
    {
        //help text?
        //Parameter - maybe attribute or validation? Way to fetch array of all parameters?
        [CommandParameter]
        public string Name { get; set; }

        /// <summary>
        /// Returns a string that will be returned as output.
        /// </summary>
        /// <returns></returns>
        public string Execute(params string[] parameters)
        {
            return "Hello " + Name;
        }
    }
}