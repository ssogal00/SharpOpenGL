using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpOpenGL;

namespace ShaderCompiler
{
    public class VertexAttributeCodeGenerator : CodeGenerator
    {
        VertexAttributeTemplate m_Template;

        public VertexAttributeCodeGenerator(ShaderProgram ProgramObject, string Name)
        {
            m_Template = new VertexAttributeTemplate(ProgramObject, Name + "VertexAttributes");
            NameSpace = string.Format("SharpOpenGL.{0}", Name);
        }

        protected override string GetCodeContents()
        {
            return m_Template.TransformText();
        }
    }
}
