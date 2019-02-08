using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.OpenGLType;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Core.OpenGLShader
{
    public class UniformVariableMetaData
    {
        public UniformVariableMetaData(string name, ActiveUniformType type, bool bIsArray = false)
        {
            VariableName = name;
            VariableType = type;
            VariableTypeString = GLToSharpTranslator.GetUniformTypeString(type);
            VariableOffset = 0;
            IsArray = bIsArray;
        }

        public UniformVariableMetaData(string name, ActiveUniformType type, int nOffset, bool bIsArray=false)
        {
            VariableName = name;
            VariableType = type;
            VariableTypeString = GLToSharpTranslator.GetUniformTypeString(type);
            VariableOffset = nOffset;
            IsArray = bIsArray;
        }

        public string VariableName;
        public ActiveUniformType VariableType;
        public int VariableOffset;
        public string VariableTypeString;
        public bool IsArray = false;
    }
}
