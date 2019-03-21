using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeArtCommandsExperiment.CodeArtCommand.Commands
{
    [ServiceConfiguration(ServiceType = typeof(ICommand), IncludeServiceAccessor = false)]
    public class ActiveJobs : CommandBase
    {
        public override string Keyword
        {
            get { return "active-workers"; }
        }

        public override CommandJob CreateJob()
        {
            CommandJob cj = new CommandJob();
            cj.ToExecute = (c) => {
                c.LogQueue.Enqueue(new Models.CommandLog(c.Name, $"Active Jobs ({c.Manager.Service.ActiveJobs.Count}):"));
                foreach (var j in c.Manager.Service.ActiveJobs)
                {
                    c.LogQueue.Enqueue(new Models.CommandLog(c.Name, $"{j.Name} started by {j.Owner} is {j.Thread.ThreadState}"));
                }
            };
            return cj;
        }
    }
}