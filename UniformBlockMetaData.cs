using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SharpOpenGL
{
    public class UniformBlockMetaData
    {
        public UniformBlockMetaData(ShaderProgram ProgramToReflect, int nBlockIndex)
        {
            UniformVariableMetaDataList = ProgramToReflect.GetUniformVariableMetaDataList(nBlockIndex);
        }
        
        public List<UniformVariableMetaData> UniformVariableMetaDataList;
    }
}
