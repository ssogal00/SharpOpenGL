using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class ConsoleCommandAttribute : System.Attribute
    {
        public ConsoleCommandAttribute(string name)
        {
            Name = name;
        }

        public string Name = "";
    }
    
}
