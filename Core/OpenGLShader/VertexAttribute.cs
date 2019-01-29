using System;
using OpenTK.Graphics.OpenGL;

using Core.OpenGLType;

namespace Core.OpenGLShader
{
    public class VertexAttribute : IComparable
    {
        public VertexAttribute(int nLocation, ActiveAttribType _AttributeType, string _Name)
        {
            AttributeLocation   = nLocation;
            ComponentCount      = GLToSharpTranslator.GetAttributeComponentCount(_AttributeType);
            AttributeType       = _AttributeType;
            Name                = _Name;
            AttributeTypeString = GLToSharpTranslator.GetVertexAttributeTypeString(AttributeType);
            Size                = GLToSharpTranslator.GetAttributeTypeSizeInBytes(AttributeType);
            ComponentType       = GLToSharpTranslator.GetComponentTypeFromAttribType(AttributeType);
        }

        public int CompareTo(object rhs)
        {
            var attribute = (VertexAttribute)rhs;


            return 0;
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
