using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Core.OpenGLShader;

namespace Core.Buffer
{
    public class DynamicUniformBuffer : OpenGLBuffer
    {
        public DynamicUniformBuffer()            
        {
            bufferTarget  = BufferTarget.UniformBuffer;
            hint          = BufferUsageHint.DynamicDraw;            
        }

        public DynamicUniformBuffer(ShaderProgram ProgramObject, string UniformBlockName)
        {
            bufferTarget = BufferTarget.UniformBuffer;
            hint = BufferUsageHint.DynamicDraw;

            if (ProgramObject.IsProgramLinked())
            {
                BindingPoint = ProgramObject.GetUniformBlockBindingPoint(UniformBlockName);
            }
        }

        int BindingPoint = -1;
    }
}
