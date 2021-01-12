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
        public MaterialCodeGenerator(Core.OpenGLShader.ShaderProgram mVsProgramObject, Core.OpenGLShader.ShaderProgram FSProgramObject,
            string _VSSourceCode, string _FSSourceCode, string _MaterialName
            )
        {            
            NameSpace = string.Format("CompiledMaterial.{0}", _MaterialName);
            FSProgram = FSProgramObject;
            mVSProgram = mVsProgramObject;
            VSSourceCode = _VSSourceCode;
            FSSourceCode = _FSSourceCode;
            MaterialName = _MaterialName;
        }

        public MaterialCodeGenerator(ShaderProgram mVsProgram, ShaderProgram fsProgram,
            string vsSourceCode, List<Tuple<string, string>> vsDefines,
            string fsSourceCode, List<Tuple<string, string>> fsDefines,
            string mtlName)
        {
            NameSpace = string.Format("CompiledMaterial.{0}", mtlName);
            FSProgram = fsProgram;
            mVSProgram = mVsProgram;

            VSSourceCode = vsSourceCode;
            FSSourceCode = fsSourceCode;
            MaterialName = mtlName;
        }

        public override string GetCodeContents()
        {
            var template = new MaterialTemplate(mVSProgram, FSProgram, VSSourceCode, FSSourceCode, MaterialName);

            var sb = new StringBuilder();
            sb.AppendLine("namespace " + MaterialName);
            sb.AppendLine("{");
            sb.AppendLine(template.TransformText());
            sb.AppendLine("}");

            return sb.ToString();
        }

        ShaderProgram mVSProgram;
        ShaderProgram FSProgram;
        string VSSourceCode;
        string FSSourceCode;
        string MaterialName;
    }
}
