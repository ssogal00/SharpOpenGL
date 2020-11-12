using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.OpenGLShader;

namespace ShaderCompiler
{
    public class VertexAttributeCodeGenerator : CodeGenerator
    {
        VertexAttributeTemplate m_Template;

        public VertexAttributeCodeGenerator(ShaderProgram ProgramObject, string Name)
        {
            m_Template = new VertexAttributeTemplate(ProgramObject, Name);
            NameSpace = Name;
        }

        public override string GetCodeContents()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("namespace {0}", NameSpace));
            sb.AppendLine("{");
            sb.Append(m_Template.TransformText());
            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}
