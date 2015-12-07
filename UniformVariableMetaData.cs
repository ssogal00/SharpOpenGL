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
            VariableOffset = nOffset;
        }

        public string VariableName;
        public ActiveUniformType VariableType;
        public int VariableOffset;
    }
}
