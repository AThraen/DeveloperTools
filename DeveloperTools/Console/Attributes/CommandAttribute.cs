using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeArtCommandsExperiment.CodeArtCommand.Attributes
{
    public class CommandAttribute : Attribute
    {
        public string Keyword { get; set; }

    }
}