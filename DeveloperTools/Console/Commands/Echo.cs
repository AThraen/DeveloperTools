using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeArtCommandsExperiment.CodeArtCommand.Commands
{

    [ServiceConfiguration(ServiceType = typeof(ICommand),IncludeServiceAccessor =false)]
    public class Echo : CommandBase
    {
        public override string Keyword
        {
            get{ return "echo";    }
        }

        public override CommandJob CreateJob()
        {
            CommandJob cj = new CommandJob();
            cj.ToExecute = (c) => {
                c.LogQueue.Enqueue(new Models.CommandLog(c.Name,c.OriginalCommand));
            };
            return cj;
        }
    }
}