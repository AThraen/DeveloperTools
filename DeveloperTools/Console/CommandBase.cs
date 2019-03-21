using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeArtCommandsExperiment.CodeArtCommand
{
    public abstract class CommandBase : ICommand
    {
        /// <summary>
        /// The commands keyword
        /// </summary>
        public abstract string Keyword { get; }

        /// <summary>
        /// Which roles are allowed to call this command. Should perhaps be a default, but also configurable?
        /// </summary>
        public virtual string[] AllowedRoles { get { return new[] { "WebAdmins","Administrators" }; } }


        /// <summary>
        /// Descriptive help text for this command
        /// </summary>
        public virtual string HelpText { get { return String.Empty; } }


        public abstract CommandJob CreateJob();

    }
}