using CodeArtCommandsExperiment.CodeArtCommand.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeArtCommandsExperiment.CodeArtCommand.NewCommands
{
    [Command(Keyword ="hello")]
    public class HelloWorldCommand
    {
        //help text?

        //Parameter - maybe attribute or validation? Way to fetch array of all parameters?
        public string Name { get; set; }

        public void Execute()
        {
            //object output?
            //Console output?
            //Accept streaming (piped) input?

        }
    }
}