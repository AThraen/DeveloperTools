using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace CodeArtCommandsExperiment.CodeArtCommand.Commands
{
    [ServiceConfiguration(ServiceType = typeof(ICommand), IncludeServiceAccessor = false)]
    public class MemoryStats : CommandBase
    {
        public override string Keyword
        {
            get { return "memory"; }
        }

        public override CommandJob CreateJob()
        {
            CommandJob cj = new CommandJob();
            cj.ToExecute = (c) => {
                Process p = Process.GetCurrentProcess();
                
                c.LogQueue.Enqueue(new Models.CommandLog(c.Name, "Private Memory Used: "+p.PrivateMemorySize64));
            };
            return cj;
        }
    }
}