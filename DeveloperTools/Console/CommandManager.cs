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
using DeveloperTools.Console.Models;
using DeveloperTools.Console.Interfaces;
using CommandLine;

namespace CodeArtCommandsExperiment.CodeArtCommand
{
    [ServiceConfiguration(Lifecycle = ServiceInstanceScope.Singleton, ServiceType = typeof(CommandManager))]
    public class CommandManager 
    {

        //New stuff
        public Dictionary<string,ConsoleCommandDescriptor> Commands { get; set; }


        public List<string> Log { get; set; }
        public List<CommandJob> ActiveJobs { get; set; }
        private Random _rand = new Random();
        

        public void UpdateJobs()
        {
            //Logging
            foreach(var j in ActiveJobs)
            {
                CommandLog cl = null;
                while(j.LogQueue.TryDequeue(out cl))
                {
                    //Log.Add(cl);
                }
            }
            //Clean up
            ActiveJobs.RemoveAll(cj => cj.Thread.ThreadState == ThreadState.Stopped);
        }

        public CommandManager()
        {
            Commands = new Dictionary<string, ConsoleCommandDescriptor>();
            ActiveJobs = new List<CommandJob>();
            Log = new List<string>();
            Log.Add($"Episerver {typeof(EPiServer.Core.ContentReference).Assembly.GetName().Version.ToString()} loaded and ready.");
        }

        public static string[] SplitArguments(string commandLine)
        {
            var parmChars = commandLine.ToCharArray();
            var inSingleQuote = false;
            var inDoubleQuote = false;
            for (var index = 0; index < parmChars.Length; index++)
            {
                if (parmChars[index] == '"' && !inSingleQuote)
                {
                    inDoubleQuote = !inDoubleQuote;
                    parmChars[index] = '\n';
                }
                if (parmChars[index] == '\'' && !inDoubleQuote)
                {
                    inSingleQuote = !inSingleQuote;
                    parmChars[index] = '\n';
                }
                if (!inSingleQuote && !inDoubleQuote && parmChars[index] == ' ')
                    parmChars[index] = '\n';
            }
            return (new string(parmChars)).Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }


        public void ExecuteCommand(string command)
        {
            var parts = SplitArguments(command).Where(p => p!="").ToArray(); //TODO: Better arguments handling (support quotes)

            //Support piping

            if (Commands.ContainsKey(parts.First().ToLower()))
            {
                var cmdd = Commands[parts.First().ToLower()];

                //Create command object
                var cmd = cmdd.CreateNew<IConsoleCommand>();

                //Map parameters
                //var map = Parser.Default.ParseArguments( parts.Skip(1).ToArray(), cmd.GetType());

                for (int i = 1; i < parts.Length; i+=2)
                {
                    if (parts[i].StartsWith("-"))
                    {
                        //Parameter
                        if (cmdd.Parameters.ContainsKey(parts[i].ToLower().TrimStart('-')))
                        {
                            var pi=cmdd.Parameters[parts[i].ToLower().TrimStart('-')];
                            pi.SetValue(cmd, parts[i + 1]); //TODO: Support other types than string
                        }
                        {
                           // Log.Add("Unrecognized parameter: " + parts[i]);
                        }
                    } else
                    {
                        Log.Add("Unknown parameter: " + parts[i]);
                    }
                }

                //Execute command
                Log.Add(cmd.Execute(parts.Skip(1).ToArray()));
            }
            else Log.Add("Unknown Command");

        }

    }
}