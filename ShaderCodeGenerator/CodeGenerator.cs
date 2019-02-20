using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShaderCompiler
{
    public class CodeGenerator
    {
        static protected List<string> DependencyList = new List<string>
        {
            "System",
            "System.Runtime.InteropServices",
            "OpenTK",
            "OpenTK.Graphics.OpenGL",
            "Core",
            "Core.Buffer",
            "Core.OpenGLShader",
            "Core.Texture",
            "Core.VertexCustomAttribute",
            "Core.MaterialBase",
            "ZeroFormatter",
            "ZeroFormatter.Formatters",
            "Core.CustomAttribute",
        };

        public string NameSpace = "";

        public virtual string GetCode()
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

        public static string GetCodeWithNamesapceAndDependency(string CodeToWrap)
        {
            StringBuilder Builder = new StringBuilder("");

            foreach (var Dependency in DependencyList)
            {
                Builder.AppendLine(string.Format("using {0};", Dependency));
            }

            Builder.AppendLine("namespace SharpOpenGL");
            Builder.AppendLine("{");
            Builder.AppendLine(CodeToWrap);
            Builder.AppendLine("}");

            return Builder.ToString();
        }

        public virtual string GetCodeContents()
        {
            return "";
        }
    }
}
