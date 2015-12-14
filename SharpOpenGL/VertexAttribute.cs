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
        public VertexAttribute(int nLocation, int nComponentCount, ActiveAttribType _AttributeType, string _Name)
        {
            AttributeLocation   = nLocation;
            ComponentCount      = nComponentCount;
            AttributeType       = _AttributeType;
            Name                = _Name;
            AttributeTypeString = OpenGLTypeConverter.FromVertexAttributeType(AttributeType);
            Size = OpenGLTypeConverter.GetAttributeTypeSize(AttributeType);            
        }

        public int AttributeLocation { get; set; }
        public int Size { get; set; }
        public int ComponentCount { get; set; }
        public string Name {get;set;}
        public string AttributeTypeString { get; set; }
        public ActiveAttribType AttributeType { get; set; }
    }
}
