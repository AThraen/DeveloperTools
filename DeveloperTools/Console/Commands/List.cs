using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeArtCommandsExperiment.CodeArtCommand.Commands
{
    [ServiceConfiguration(ServiceType = typeof(ICommand), IncludeServiceAccessor = false)]
    public class ListCommands : CommandBase
    {
        public override string Keyword
        {
            get { return "list-commands"; }
        }

        public override CommandJob CreateJob()
        {
            CommandJob cj = new CommandJob();
            cj.ToExecute = (c) => {
               foreach(var d in c.Manager.Service.AllCommands)
                {
                    c.LogQueue.Enqueue(new Models.CommandLog(c.Name, d.Keyword));
                }
            };
            return cj;
        }
    }
}