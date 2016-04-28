using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using System.Reflection;

using System.Runtime.InteropServices;

namespace SharpOpenGL
{
    public class VertexAttribute
    {
        public VertexAttribute(int nLocation, ActiveAttribType _AttributeType, string _Name)
        {
            AttributeLocation   = nLocation;
            ComponentCount      = OpenGLTypeHelper.GetAttributeComponentCount(_AttributeType);
            AttributeType       = _AttributeType;
            Name                = _Name;
            AttributeTypeString = OpenGLTypeHelper.FromVertexAttributeType(AttributeType);
            Size                = OpenGLTypeHelper.GetAttributeTypeSizeInBytes(AttributeType);
            ComponentType       = OpenGLTypeHelper.GetComponentTypeFromAttribType(AttributeType);
        }        

        public int AttributeLocation { get; set; }
        public int Size { get; set; }
        public int ComponentCount { get; set; }
        public string Name {get;set;}
        public string AttributeTypeString { get; set; }
        public ActiveAttribType AttributeType { get; set; }
        public VertexAttribPointerType ComponentType { get; set; }
    }
}
