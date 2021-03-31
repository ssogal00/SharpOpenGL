using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using Core.OpenGLShader;
using System.Diagnostics;

namespace Core.Buffer
{
    public class DynamicUniformBuffer : OpenGLBuffer
    {
        protected DynamicUniformBuffer()            
        {
            bufferTarget  = BufferTarget.UniformBuffer;
            hint          = BufferUsageHint.DynamicDraw;
        }

        public DynamicUniformBuffer(ShaderProgram ProgramObject, string UniformBlockName)
        : this()
        {
            if (ProgramObject.IsProgramLinked())
            {
                UniformBufferBlockIndex = ProgramObject.GetUniformBlockBindingPoint(UniformBlockName);
            }
        }

        public DynamicUniformBuffer(int ProgramObject, string UniformBlockName)
        : this()
        {   
            UniformBufferBlockIndex = GL.GetUniformBlockIndex(ProgramObject, UniformBlockName);
        }

        public override void Bind()
        {
            base.Bind();
            //
            Debug.Assert(UniformBufferBlockIndex != -1);
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, UniformBufferBlockIndex, mBufferHandle);
        }

        public int UniformBufferBlockIndex = -1;
    }
}
