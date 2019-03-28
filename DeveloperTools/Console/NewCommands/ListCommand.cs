using DeveloperTools.Console.Interfaces;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperTools.Console.NewCommands
{
    public class ListCommand : IOutputCommand
    {
        public event CommandOutput OnCommandOutput;

        public string Execute(params string[] parameters)
        {
            var repo = ServiceLocator.Current.GetInstance<IContentRepository>();

            foreach(var r in repo.GetDescendents(ContentReference.StartPage))
            {
                OnCommandOutput?.Invoke(this, repo.Get<IContent>(r));
            }

            return "Done";
        }
    }
}
