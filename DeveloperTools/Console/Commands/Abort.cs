using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeArtCommandsExperiment.CodeArtCommand.Commands
{
    [ServiceConfiguration(ServiceType = typeof(ICommand), IncludeServiceAccessor = false)]
    public class Abort : CommandBase
    {
        public override string Keyword
        {
            get { return "abort"; }
        }

        public override CommandJob CreateJob()
        {
            CommandJob cj = new CommandJob();
            cj.ToExecute = (c) => {
                var jb = c.Manager.Service.ActiveJobs.FirstOrDefault(d => d.Name.ToLower() == c.OriginalCommand.ToLower());
                if (jb == null)
                {
                    c.LogQueue.Enqueue(new Models.CommandLog(c.Name, $"Worker {c.OriginalCommand} not active."));
                } else
                {
                    jb.Thread.Abort();
                    c.Manager.Service.ActiveJobs.Remove(jb);
                    c.LogQueue.Enqueue(new Models.CommandLog(c.Name, $"Aborted {jb.Name}"));
                }
            };
            return cj;
        }
    }
}