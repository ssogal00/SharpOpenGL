using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace Core.OpenGLType
{
    public static class GLToSharpTranslator 
    {
        public static string GetVertexAttributeTypeString(ActiveAttribType eType)
        {
            switch(eType)
            {
                case ActiveAttribType.FloatVec3:
                    return typeof(OpenTK.Vector3).ToString();

                case ActiveAttribType.FloatVec2:                    
                    return typeof(OpenTK.Vector2).ToString();

                case ActiveAttribType.FloatVec4:
                    return typeof(OpenTK.Vector4).ToString();

                case ActiveAttribType.FloatMat3:
                    return typeof(OpenTK.Matrix3).ToString();  
                
                case ActiveAttribType.FloatMat4:
                    return typeof(OpenTK.Matrix4).ToString();
            }

            return "";
        }      

        public static int GetUniformTypeSize(ActiveUniformType eType)
        { 
            switch(eType)
            { 
                case ActiveUniformType.FloatVec2:
                    return OpenTK.Vector2.SizeInBytes;
               
                case ActiveUniformType.FloatVec3:
                    return OpenTK.Vector3.SizeInBytes;

                case ActiveUniformType.FloatVec4:
                    return OpenTK.Vector4.SizeInBytes;
            }

            return -1;
        }    
    
        public static int GetAttributeTypeSizeInBytes(ActiveAttribType eType)
        {
            switch(eType)
            {
                case ActiveAttribType.FloatVec2:
                    return OpenTK.Vector2.SizeInBytes;

                case ActiveAttribType.FloatVec3:
                    return OpenTK.Vector3.SizeInBytes;

                case ActiveAttribType.FloatVec4:
                    return OpenTK.Vector4.SizeInBytes;

                case ActiveAttribType.DoubleVec2:
                    return OpenTK.Vector2d.SizeInBytes;

                case ActiveAttribType.DoubleVec3:
                    return OpenTK.Vector3d.SizeInBytes;

                case ActiveAttribType.DoubleVec4:
                    return OpenTK.Vector4d.SizeInBytes;
            }

            return -1;
        }

        public static VertexAttribPointerType GetComponentTypeFromAttribType(ActiveAttribType AttrType)
        {
            switch (AttrType)
            {
                case ActiveAttribType.Double:
                case ActiveAttribType.DoubleVec2:
                case ActiveAttribType.DoubleVec3:
                case ActiveAttribType.DoubleVec4:
                    return VertexAttribPointerType.Double;

                case ActiveAttribType.Float:
                case ActiveAttribType.FloatVec2:
                case ActiveAttribType.FloatVec3:
                case ActiveAttribType.FloatVec4:
                    return VertexAttribPointerType.Float;

                case ActiveAttribType.Int:
                case ActiveAttribType.IntVec2:
                case ActiveAttribType.IntVec3:
                case ActiveAttribType.IntVec4:
                    return VertexAttribPointerType.Int;

                case ActiveAttribType.UnsignedInt:
                case ActiveAttribType.UnsignedIntVec2:
                case ActiveAttribType.UnsignedIntVec3:
                case ActiveAttribType.UnsignedIntVec4:
                    return VertexAttribPointerType.UnsignedInt;

            }

            return VertexAttribPointerType.Float;
        }

        public static int GetAttributeComponentCount(ActiveAttribType AttrType)
        {
            switch (AttrType)
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

        public static string GetUniformTypeString(ActiveUniformType eType)
        {
            switch (eType)
            {
                case ActiveUniformType.Bool:
                    return typeof(bool).ToString();

                case ActiveUniformType.Int:
                    return typeof(int).ToString();
                    
                case ActiveUniformType.FloatVec2:
                    return typeof(OpenTK.Vector2).ToString();                    

                case ActiveUniformType.FloatVec3:
                    return typeof(OpenTK.Vector3).ToString();                    

                case ActiveUniformType.FloatVec4:
                    return typeof(OpenTK.Vector4).ToString();                    

                case ActiveUniformType.FloatMat3:
                    return typeof(OpenTK.Matrix3).ToString();                    

                case ActiveUniformType.FloatMat4:
                    return typeof(OpenTK.Matrix4).ToString();                    

                case ActiveUniformType.Float:
                    return typeof(float).ToString();

                default:
                    Debug.Assert(false, "Not supported format {0}", eType.ToString());
                    return "";
            }

            return "";
        }
    }
}
