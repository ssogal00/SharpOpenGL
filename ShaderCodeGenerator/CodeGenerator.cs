using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShaderCompiler
{
    public class CodeGenerator
    {
        protected List<string> DependencyList = new List<string>
        {
            "System.Runtime.InteropServices",
            "OpenTK",
            "OpenTK.Graphics.OpenGL",
        };

        public string NameSpace = "";

        public string GetCode()
        {
            StringBuilder Builder = new StringBuilder("");

            foreach (var Dependency in DependencyList)
            {
                Builder.AppendLine(string.Format("using {0};", Dependency));
            }
            
            Builder.AppendLine("namespace " + NameSpace);
            Builder.AppendLine("{");
            Builder.AppendLine(GetCodeContents());
            Builder.AppendLine("}");

            return Builder.ToString();
        }

        protected virtual string GetCodeContents()
        {
            return "";
        }
    }
}
