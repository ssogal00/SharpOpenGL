using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpOpenGL
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false)]
    public class ComponentCount : System.Attribute
    {
        public int Size = 0;

        public ComponentCount(int nSize)
        {
            Size = nSize;
        }
    }
}
