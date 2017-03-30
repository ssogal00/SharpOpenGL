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
        public VertexShaderTemplate(ShaderProgram Program, string ShaderName)
        {
            VSProgram = Program;
            VSShaderName = ShaderName;            
        }

        protected ShaderProgram VSProgram;
        protected string VSShaderName;
    }
}
