using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SharpOpenGL
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false)]
    public class ComponentType : System.Attribute
    {
        public ComponentType(VertexAttribPointerType _Type)
        {
            Type = _Type;
        }

        public VertexAttribPointerType Type = VertexAttribPointerType.Float;
    }    
}
