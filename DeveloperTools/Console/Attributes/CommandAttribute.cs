using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeveloperTools.Console.Attributes
{
    public class CommandAttribute : Attribute
    {
        public string Keyword { get; set; }

        public string Syntax { get; set; }

    }
}