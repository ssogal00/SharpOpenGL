using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.OpenGLShader;

namespace ShaderCompiler
{
    public class MaterialCodeGenerator : CodeGenerator
    {
        public MaterialCodeGenerator(Core.OpenGLShader.ShaderProgram VSProgramObject, Core.OpenGLShader.ShaderProgram FSProgramObject,
            string _VSSourceCode, string _FSSourceCode, string _MaterialName
            )
        {            
            NameSpace = string.Format("CompiledMaterial.{0}", _MaterialName);
            FSProgram = FSProgramObject;
            VSProgram = VSProgramObject;
            VSSourceCode = _VSSourceCode;
            FSSourceCode = _FSSourceCode;
            MaterialName = _MaterialName;
        }

        public override string GetCodeContents()
        {
            var template = new MaterialTemplate(VSProgram, FSProgram, VSSourceCode, FSSourceCode, MaterialName);

            var sb = new StringBuilder();
            sb.AppendLine("namespace " + MaterialName);
            sb.AppendLine("{");
            sb.AppendLine(template.TransformText());
            sb.AppendLine("}");

            return sb.ToString();
        }

        ShaderProgram VSProgram;
        ShaderProgram FSProgram;
        string VSSourceCode;
        string FSSourceCode;
        string MaterialName;
    }
}
