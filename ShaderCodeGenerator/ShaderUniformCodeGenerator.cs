using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.OpenGLShader;

namespace ShaderCompiler
{
    public class ShaderUniformCodeGenerator : CodeGenerator
    {
        public ShaderUniformCodeGenerator(ShaderProgram ProgramObject , string Name)
        {
            Program = ProgramObject;
            NameSpace = string.Format("SharpOpenGL.{0}", Name);
        }

        protected override string GetCodeContents()
        {
            StringBuilder sb = new StringBuilder();

            for(int i = 0; i < Program.GetActiveUniformBlockCount(); ++i)
            {
                var template = new ShaderUniformTemplate(Program, i);
                sb.Append(template.TransformText());
            }

            return sb.ToString();
        }

        protected ShaderProgram Program;
    }
}
