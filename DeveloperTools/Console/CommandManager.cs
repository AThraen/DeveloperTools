using EPiServer.Framework;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Framework.Initialization;
using CodeArtCommandsExperiment.CodeArtCommand.Models;
using EPiServer.Personalization;
using System.Threading;

namespace CodeArtCommandsExperiment.CodeArtCommand
{
    [ServiceConfiguration(Lifecycle = ServiceInstanceScope.Singleton, ServiceType = typeof(CommandManager))]
    public class CommandManager 
    {
        public List<CommandLog> Log { get; set; }
        public List<ICommand> AllCommands { get; set; }
        public List<CommandJob> ActiveJobs { get; set; }
        private Random _rand = new Random();

        private  string[] _threadNames = new string[] {
            "Alice","Alex","Max","Oliver","Olga","George","John","Paul","Ringo","Ada","Charlie","Mikael","Jonas","Magnus","Anna"
        };

        public void UpdateJobs()
        {
            //Logging
            foreach(var j in ActiveJobs)
            {
                CommandLog cl = null;
                while(j.LogQueue.TryDequeue(out cl))
                {
                    Log.Add(cl);
                }
            }
            //TODO: Incoming and Outgoing and piping
            //Clean up
            ActiveJobs.RemoveAll(cj => cj.Thread.ThreadState == ThreadState.Stopped);
        }

        public CommandManager()
        {
            ActiveJobs = new List<CommandJob>();
            Log = new List<CommandLog>();
            Log.Add(new CommandLog("Episerver", $"Episerver {typeof(EPiServer.Core.ContentReference).Assembly.GetName().Version.ToString()} loaded and ready."));
        }





        public void ExecuteCommand(string command)
        {
            if(AllCommands==null) AllCommands = ServiceLocator.Current.GetAllInstances<ICommand>().ToList();
            //TODO handle pipes
            //TODO handle input to specific threads
            string kw = command.Split(' ').First();
            var cmd=AllCommands.Where(cb => cb.Keyword.ToLower() == kw.ToLower()).FirstOrDefault();
            if (cmd == null) return;
            var job=cmd.CreateJob();
            //Assign parameters
            job.OriginalCommand = command.Substring(kw.Length).Trim();
            job.Owner = EPiServerProfile.Current.DisplayName;
            var possnames=_threadNames.Except(ActiveJobs.Select(cj => cj.Name)).ToArray();
            if(possnames.Length==0)
            {
                Log.Add(new CommandLog("System", $"Unable to start job, no workers available"));
            }
            job.Name = possnames[_rand.Next(possnames.Length)];
            job.Thread = new Thread(new ThreadStart(job.Execute));
            job.Thread.Start();
            ActiveJobs.Add(job);
            Log.Add(new CommandLog("System", $"{kw} job assigned to {job.Name}"));

        }

    }
}