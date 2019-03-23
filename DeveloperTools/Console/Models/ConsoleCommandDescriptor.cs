using DeveloperTools.Console.Attributes;
using DeveloperTools.Console.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperTools.Console.Models
{
    public class ConsoleCommandDescriptor
    {
        public Type CommandType { get; set; }

        public Dictionary<string,PropertyInfo> Parameters { get; set; }

        

        public void LoadParameters()
        {
            Parameters = new Dictionary<string, PropertyInfo>();

            foreach(var p in CommandType.GetProperties())
            {
                var pa = p.GetCustomAttribute<CommandParameterAttribute>();
                if (pa != null && !string.IsNullOrEmpty(pa.Name)) Parameters.Add(pa.Name.ToLower(), p);
                else Parameters.Add(p.Name.ToLower(), p);
            }
        }

        public string Keyword {
            get
            {
                var ca=CommandType.GetCustomAttributes(typeof(CommandAttribute), true).FirstOrDefault() as CommandAttribute;
                if (ca == null) return CommandType.Name;
                return ca.Keyword;
            }
        }

        public T CreateNew<T>() where T:IConsoleCommand
        {
            return (T) Activator.CreateInstance(CommandType);
        }
    }
}
