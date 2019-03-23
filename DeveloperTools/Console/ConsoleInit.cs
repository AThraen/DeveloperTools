using System;
using System.Linq;
using CodeArtCommandsExperiment.CodeArtCommand;
using DeveloperTools.Console.Interfaces;
using DeveloperTools.Console.Models;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace DeveloperTools.Console
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class ConsoleInit : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            //Add initialization logic, this method is called once after CMS has been initialized
            context.InitComplete += Context_InitComplete;
        }

        private void Context_InitComplete(object sender, EventArgs e)
        {
            var cmdMgr = ServiceLocator.Current.GetInstance<CommandManager>();
            var type = typeof(IConsoleCommand);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));

            foreach(var t in types)
            {
                var ccd = new ConsoleCommandDescriptor() { CommandType = t };
                ccd.LoadParameters();
                cmdMgr.Commands.Add(ccd.Keyword.ToLower(), ccd);
            }
        }

        public void Uninitialize(InitializationEngine context)
        {
            //Add uninitialization logic
        }
    }
}