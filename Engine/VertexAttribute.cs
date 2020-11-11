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

        public bool IsCompatible(VertexAttribute rhs)
        {
            if (AttributeLocation != rhs.AttributeLocation ||
                ComponentCount != rhs.ComponentCount ||
                AttributeType != rhs.AttributeType)
            {
                return false;
            }

            return true;
        }

        public int CompareTo(object rhs)
        {
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
