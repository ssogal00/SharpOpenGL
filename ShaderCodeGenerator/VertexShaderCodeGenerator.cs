using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.OpenGLShader;

namespace ShaderCompiler
{
    public class VertexShaderCodeGenerator : CodeGenerator
    {
        public VertexShaderCodeGenerator(Core.OpenGLShader.ShaderProgram ProgramObject, string Name, string ShaderSourceCode)
        {
            Program = ProgramObject;
            NameSpace = string.Format("SharpOpenGL.{0}", Name);
            SourceCode = ShaderSourceCode;
        }

        protected override string GetCodeContents()
        {
            var template = new VertexShaderTemplate(Program, "", SourceCode);
            return template.TransformText();
        }

        ShaderProgram Program;
        string SourceCode;
    }
}
