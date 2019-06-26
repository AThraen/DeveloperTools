﻿using EPiServer.Framework;
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
            var commands = command.Split('|').Where(p => p != "").ToArray();

            List<ExecutableCommand> ecommands = new List<ExecutableCommand>();
            foreach (var cmds in commands)
            {
                ExecutableCommand ecmd = new ExecutableCommand();
                
                var parts = SplitArguments(cmds).Where(p => p != "").ToArray(); //TODO: Better arguments handling (support quotes)
                ecmd.Parameters = parts.Skip(1).ToArray();
                //Support piping

                if (Commands.ContainsKey(parts.First().ToLower()))
                {
                    var cmdd = Commands[parts.First().ToLower()];

                    //Create command object
                    var cmd = cmdd.CreateNew<IConsoleCommand>();
                    ecmd.Command = cmd;
                    //Map parameters

                    for (int i = 1; i < parts.Length; i += 2)
                    {
                        if (parts[i].StartsWith("-"))
                        {
                            //Parameter
                            if (cmdd.Parameters.ContainsKey(parts[i].ToLower().TrimStart('-')))
                            {
                                var pi = cmdd.Parameters[parts[i].ToLower().TrimStart('-')];
                                if (pi.PropertyType == typeof(string))
                                {
                                    pi.SetValue(cmd, parts[i + 1]); //TODO: Support other types than string
                                } else if(pi.PropertyType == typeof(int))
                                {
                                    pi.SetValue(cmd, int.Parse(parts[i + 1]));
                                } else if (pi.PropertyType.IsEnum)
                                {
                                    pi.SetValue(cmd, Enum.Parse(pi.PropertyType, parts[i + 1]));
                                }
                            } else 
                            {
                                Log.Add("Unrecognized parameter: " + parts[i]);
                            }
                        }
                        else
                        {
                            //Log.Add("Unknown parameter: " + parts[i]);
                        }
                    }

                    //Execute command
                    //Log.Add(cmd.Execute(parts.Skip(1).ToArray()));
                }
                else Log.Add("Unknown Command");

                if(ecmd.Command is IConsoleOutputCommand)
                {
                    ((IConsoleOutputCommand)ecmd.Command).OutputToConsole += new OutputToConsoleHandler((c, s) => { if (s != null) Log.Add(s); });
                }

                if (ecmd.Command is IInputCommand) (ecmd.Command as IInputCommand).Source = ecommands.Last().Command as IOutputCommand;

                if(ecommands.Count>0 && !(ecmd.Command is IInputCommand))
                {
                    Log.Add("You cannot pipe content to that command");
                    return;
                } else if(commands.Length>1 && ecommands.Count==0 && !(ecmd.Command is IOutputCommand))
                {
                    Log.Add("You cannot pipe content from that command");
                    return;
                }
                ecommands.Add(ecmd);
            }
            //Chain them togetehr
            
            //Execute
            ecommands.Reverse();
            foreach(var ec in ecommands)
            {
                var exec = ec.Command.Execute(ec.Parameters);
                if(!string.IsNullOrEmpty(exec))    Log.Add(exec);
            }

        }

    }
}