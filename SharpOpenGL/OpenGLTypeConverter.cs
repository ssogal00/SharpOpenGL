using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SharpOpenGL
{
    public static class OpenGLTypeConverter 
    {
        public static string FromVertexAttributeType(ActiveAttribType eType)
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
            }

            return -1;
        }    
    
        public static int GetAttributeTypeSize(ActiveAttribType eType)
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
            }

            return -1;
        }


        public static string FromUniformType(ActiveUniformType eType)
        {
            switch (eType)
            {
                case ActiveUniformType.Bool:
                    return typeof(bool).ToString();
                    
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
            }

            return "";
        }
    }
}
