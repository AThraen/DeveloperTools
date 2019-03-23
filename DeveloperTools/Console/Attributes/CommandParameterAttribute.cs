using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperTools.Console.Attributes
{
    public class CommandParameterAttribute :  Attribute
    {
        public string DefaultValue { get; set; }

        public int Order { get; set; }

        public string Name { get; set; }

    }
}
