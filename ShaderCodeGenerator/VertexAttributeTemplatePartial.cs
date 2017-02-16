using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using SharpOpenGL;

namespace ShaderCompiler
{
    public partial class VertexAttributeTemplate
    {
        public VertexAttributeTemplate(ShaderProgram Program, string Name)
        {
            VertexAttributeList = Program.GetActiveVertexAttributeList();
            StructName = Name;
        }

        protected List<VertexAttribute> VertexAttributeList = null;

        protected string StructName;
    }
}
