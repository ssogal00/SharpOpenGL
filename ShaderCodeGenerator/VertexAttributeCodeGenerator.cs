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

        VertexAttributeCodeGenerator(ShaderProgram ProgramObject, string Name)
        {
            m_Template = new VertexAttributeTemplate(ProgramObject, Name + "VertexAttributes");
        }

        protected virtual string GetCodeContents()
        {
            return m_Template.TransformText();
        }
    }
}
