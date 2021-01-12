using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.OpenGLShader;

namespace ShaderCompilerCore
{
    public partial class MaterialTemplate
    {
        public MaterialTemplate(ShaderProgram _VSProgram, ShaderProgram _FSProgram, string _VSSourceCode, string _FSSourceCode, string _ShaderName)
        {
            VSProgram = _VSProgram;
            FSProgram = _FSProgram;
            ShaderName = _ShaderName;
            VSSourceCode = _VSSourceCode;
            FSSourceCode = _FSSourceCode;
        }

        protected ShaderProgram VSProgram;
        protected ShaderProgram FSProgram;
        protected string ShaderName;
        protected string VSSourceCode;
        protected string FSSourceCode;
    }
}
