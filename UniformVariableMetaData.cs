using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SharpOpenGL
{
    public class UniformVariableMetaData
    {
        public UniformVariableMetaData(string name, ActiveUniformType type, int nOffset)
        {
            VariableName = name;
            VariableType = type;

            switch(type)
            {
                case ActiveUniformType.Bool:
                    VariableTypeString = "bool";
                    break;
                    
                case ActiveUniformType.FloatVec2:
                    VariableTypeString = typeof(OpenTK.Vector2).ToString();
                    break;

                case ActiveUniformType.FloatVec3:
                    VariableTypeString = typeof(OpenTK.Vector3).ToString();
                    break;

                case ActiveUniformType.FloatVec4:
                    VariableTypeString = typeof(OpenTK.Vector4).ToString();
                    break;

                case ActiveUniformType.FloatMat3:
                    VariableTypeString = typeof(OpenTK.Matrix3).ToString();
                    break;

                case ActiveUniformType.FloatMat4:
                    VariableTypeString = typeof(OpenTK.Matrix4).ToString();
                    break;
                
                case ActiveUniformType.Float:
                    VariableTypeString = "float";
                    break;
            }
            
            VariableOffset = nOffset;
        }

        public string VariableName;
        public ActiveUniformType VariableType;
        public int VariableOffset;
        public string VariableTypeString;
    }
}
