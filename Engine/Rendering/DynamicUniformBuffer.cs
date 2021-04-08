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
            mBufferTarget  = BufferTarget.UniformBuffer;
            mHint          = BufferUsageHint.DynamicDraw;
        }

        public DynamicUniformBuffer(ShaderProgram programObject, string uniformBlockName)
        : this()
        {
            if (programObject.IsProgramLinked())
            {
                UniformBufferBlockIndex = programObject.GetUniformBlockBindingPoint(uniformBlockName);
            }
        }

        public DynamicUniformBuffer(ShaderProgram programObject, string uniformBlockName, int size)
        : base()
        {
            mBufferTarget = BufferTarget.UniformBuffer;
            mHint = BufferUsageHint.DynamicDraw;

            if (programObject.IsProgramLinked())
            {
                UniformBufferBlockIndex = programObject.GetUniformBlockBindingPoint(uniformBlockName);
            }
            AllocateBuffer(size);
        }

        public DynamicUniformBuffer(int programObject, string uniformBlockName)
        : this()
        {   
            UniformBufferBlockIndex = GL.GetUniformBlockIndex(programObject, uniformBlockName);
        }

        public override void Bind()
        {
            //
            Debug.Assert(UniformBufferBlockIndex != -1);
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, UniformBufferBlockIndex, mBufferHandle);
        }

        public int UniformBufferBlockIndex
        {
            get => mUniformBufferBlockIndex;
            private set => mUniformBufferBlockIndex = value;
        }

        private int mUniformBufferBlockIndex = -1;
    }
}
