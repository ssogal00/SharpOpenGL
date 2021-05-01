using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.OpenGLShader;

namespace ShaderCompilerCore
{
    public class MaterialCodeGenerator : CodeGenerator
    {
        public MaterialCodeGenerator(Core.OpenGLShader.ShaderProgram vsProgramObject,
            string _VSSourceCode, string _FSSourceCode, string _MaterialName
            )
        {            
            NameSpace = string.Format("CompiledMaterial.{0}", _MaterialName);
           
            mShaderProgram = vsProgramObject;
            VSSourceCode = _VSSourceCode;
            FSSourceCode = _FSSourceCode;
            MaterialName = _MaterialName;
        }

        public MaterialCodeGenerator(ShaderProgram vsProgramObject, string vsCode, string fsCode, string gsCode, string materialName)
        {
            NameSpace = string.Format("CompiledMaterial.{0}", materialName);
            
            mShaderProgram = vsProgramObject;
            VSSourceCode = vsCode;
            FSSourceCode = fsCode;
            GSSourceCode = gsCode;
            MaterialName = materialName;
        }

        public override string GetCodeContents()
        {
            var template = new MaterialTemplate(mShaderProgram, VSSourceCode, FSSourceCode, GSSourceCode, MaterialName);

            var sb = new StringBuilder();
            sb.AppendLine("namespace " + MaterialName);
            sb.AppendLine("{");
            sb.AppendLine(template.TransformText());
            sb.AppendLine("}");

            return sb.ToString();
        }

        private ShaderProgram mShaderProgram;
        string VSSourceCode = "";
        string FSSourceCode = "";
        private string GSSourceCode = "";
        string MaterialName;
    }
}
