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
        public MaterialTemplate(ShaderProgram vsProgram, string vsSourceCode, string fsSourceCode, string shaderName)
        {
            mShaderProgram = vsProgram;
            ShaderName = shaderName;
            VSSourceCode = vsSourceCode;
            FSSourceCode = fsSourceCode;
            mVertexAttributeList = vsProgram.GetActiveVertexAttributeList();
        }

        public MaterialTemplate(ShaderProgram vsProgram,
            string vsSourceCode, string fsSourceCode, string gsSourceCode, string shaderName)
        {
            mShaderProgram = vsProgram;
            ShaderName = shaderName;
            VSSourceCode = vsSourceCode;
            FSSourceCode = fsSourceCode;
            GSSourceCode = gsSourceCode;
            mVertexAttributeList = vsProgram.GetActiveVertexAttributeList();
        }

        protected ShaderProgram mShaderProgram;
        protected string ShaderName;
        protected string VSSourceCode = "";
        protected string FSSourceCode = "";
        protected string GSSourceCode = "";
        protected List<VertexAttribute> mVertexAttributeList = new List<VertexAttribute>();
    }
}
