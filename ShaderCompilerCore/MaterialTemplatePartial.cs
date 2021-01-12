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
        public MaterialTemplate(ShaderProgram vsProgram, ShaderProgram fsProgram, string vsSourceCode, string fsSourceCode, string shaderName)
        {
            VSProgram = vsProgram;
            FSProgram = fsProgram;
            ShaderName = shaderName;
            VSSourceCode = vsSourceCode;
            FSSourceCode = fsSourceCode;
            mVertexAttributeList = vsProgram.GetActiveVertexAttributeList();
        }

        protected ShaderProgram VSProgram;
        protected ShaderProgram FSProgram;
        protected string ShaderName;
        protected string VSSourceCode;
        protected string FSSourceCode;
        protected List<VertexAttribute> mVertexAttributeList = new List<VertexAttribute>();
    }
}
