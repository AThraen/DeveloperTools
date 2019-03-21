using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeArtCommandsExperiment.CodeArtCommand
{
    public abstract class TypedCommand
    {
        public abstract string Keyword { get; }
    }
}