using DeveloperTools.Console.Attributes;
using DeveloperTools.Console.Interfaces;
using EPiServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperTools.Console.NewCommands
{
    [Command(Keyword = "select", Description ="Selects a specific property from a piece of content or an object or a dictionary.")]
    public class SelectCommand : IOutputCommand, IInputCommand
    {
        public IOutputCommand Source { get; set; }

        public event CommandOutput OnCommandOutput;

        [CommandParameter]
        public string Property { get; set; } //TODO: Support multiple properties


        public string Execute(params string[] parameters)
        {
            if(parameters.Length>0 && string.IsNullOrEmpty(Property))
            {
                Property = parameters.First();
            }
            if (Source != null)
            {
                Source.OnCommandOutput += Source_OnCommandOutput;
            }

            return string.Empty;
        }

        private void Source_OnCommandOutput(IOutputCommand sender, object output)
        {
                if (output is IContent)
                {
                    OnCommandOutput?.Invoke(this, ((IContent)output).Property[Property].Value);
                } 
                //TODO: Support dictionary, object, ?
        }
    }
}
