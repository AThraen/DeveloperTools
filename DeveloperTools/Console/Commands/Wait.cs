using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace CodeArtCommandsExperiment.CodeArtCommand.Commands
{
    [ServiceConfiguration(ServiceType = typeof(ICommand), IncludeServiceAccessor = false)]
    public class Wait : CommandBase
    {
        public override string Keyword
        {
            get { return "wait"; }
        }

        public override CommandJob CreateJob()
        {
            CommandJob cj = new CommandJob();
            cj.ToExecute = (c) => {
                Thread.Sleep(int.Parse(c.OriginalCommand));
                c.LogQueue.Enqueue(new Models.CommandLog(c.Name, c.OriginalCommand));
            };
            return cj;
        }
    }
}