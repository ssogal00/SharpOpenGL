using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.OpenGLShader;

namespace ShaderCompiler
{
    public partial class VertexShaderTemplate
    {
        public VertexShaderTemplate(ShaderProgram Program, string ShaderName, string ShaderSourceCode)
        {
            VSProgram = Program;
            VSShaderName = ShaderName;
            SourceCode = ShaderSourceCode;
        }

        protected ShaderProgram VSProgram;
        protected string VSShaderName;
        protected string SourceCode;
    }
}
