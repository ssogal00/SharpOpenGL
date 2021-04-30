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
        public MaterialCodeGenerator(Core.OpenGLShader.ShaderProgram vsProgramObject, Core.OpenGLShader.ShaderProgram fsProgramObject,
            string _VSSourceCode, string _FSSourceCode, string _MaterialName
            )
        {            
            NameSpace = string.Format("CompiledMaterial.{0}", _MaterialName);
            mFSProgram = fsProgramObject;
            mVSProgram = vsProgramObject;
            VSSourceCode = _VSSourceCode;
            FSSourceCode = _FSSourceCode;
            MaterialName = _MaterialName;
        }

        public MaterialCodeGenerator(ShaderProgram vsProgramObject, ShaderProgram fsProgramObject,
            ShaderProgram gsProgramObject, string vsCode, string fsCode, string gsCode, string materialName)
        {
            NameSpace = string.Format("CompiledMaterial.{0}", materialName);
            mFSProgram = fsProgramObject;
            mVSProgram = vsProgramObject;
            mGSProgram = gsProgramObject;
            VSSourceCode = vsCode;
            FSSourceCode = fsCode;
            GSSourceCode = gsCode;
            MaterialName = materialName;
        }

        public override string GetCodeContents()
        {
            var template = new MaterialTemplate(mVSProgram, mFSProgram, mGSProgram,  VSSourceCode, FSSourceCode, GSSourceCode, MaterialName);

            var sb = new StringBuilder();
            sb.AppendLine("namespace " + MaterialName);
            sb.AppendLine("{");
            sb.AppendLine(template.TransformText());
            sb.AppendLine("}");

            return sb.ToString();
        }

        private ShaderProgram mVSProgram;
        private ShaderProgram mFSProgram;
        private ShaderProgram mGSProgram;
        string VSSourceCode = "";
        string FSSourceCode = "";
        private string GSSourceCode = "#version 460 core";
        string MaterialName;
    }
}
