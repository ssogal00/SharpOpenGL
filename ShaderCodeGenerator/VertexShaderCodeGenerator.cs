using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpOpenGL;

namespace ShaderCompiler
{
    public class VertexShaderCodeGenerator : CodeGenerator
    {
        public VertexShaderCodeGenerator(SharpOpenGL.ShaderProgram ProgramObject, string Name)
        {
            Program = ProgramObject;
        }

        protected override string GetCodeContents()
        {
            var template = new VertexShaderTemplate(Program, "");
            return template.TransformText();
        }

        ShaderProgram Program;
    }
}
