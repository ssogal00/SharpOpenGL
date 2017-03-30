using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.OpenGLShader;

namespace ShaderCompiler
{
    public partial class MaterialTemplate
    {
        public MaterialTemplate(ShaderProgram _VSProgram, ShaderProgram _FSProgram)
        {
            VSProgram = _VSProgram;
            FSProgram = _FSProgram;
        }

        protected ShaderProgram VSProgram;
        protected ShaderProgram FSProgram;
    }
}
