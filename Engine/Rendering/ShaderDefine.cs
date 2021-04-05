using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.OpenGLShader
{
    public class ShaderDefine
    {
        public ShaderDefine(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        public override string ToString()
        {
            return string.Format("#define {0} {1}", Name, Value);
        }

        protected string Name;
        protected string Value;
    }
}