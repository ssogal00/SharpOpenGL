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
            ComponentCount      = GetAttributeComponentCount(_AttributeType);
            AttributeType       = _AttributeType;
            Name                = _Name;
            AttributeTypeString = OpenGLTypeConverter.FromVertexAttributeType(AttributeType);
            Size = OpenGLTypeConverter.GetAttributeTypeSize(AttributeType);            
        }

        public static int GetAttributeComponentCount(ActiveAttribType AttrType)
        {
            switch(AttrType)
            { 
                case ActiveAttribType.DoubleVec2:
                case ActiveAttribType.FloatVec2:        
                case ActiveAttribType.IntVec2:
                    return 2;

                case ActiveAttribType.DoubleVec4:
                case ActiveAttribType.FloatVec4:
                case ActiveAttribType.IntVec4:
                    return 4;

                case ActiveAttribType.FloatVec3:
                case ActiveAttribType.DoubleVec3:
                case ActiveAttribType.IntVec3:
                    return 3;

                case ActiveAttribType.Float:
                case ActiveAttribType.Int:
                case ActiveAttribType.Double:
                    return 1;
            }

            return -1;
        }

        public int AttributeLocation { get; set; }
        public int Size { get; set; }
        public int ComponentCount { get; set; }
        public string Name {get;set;}
        public string AttributeTypeString { get; set; }
        public ActiveAttribType AttributeType { get; set; }
    }
}
