using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Core.OpenGLShader;


namespace Core.Buffer
{
    public class TypedDynamicUniformBuffer<T> : DynamicUniformBuffer where T : struct
    {
        protected TypedDynamicUniformBuffer()
        : base()
        {
            var defaultValue = new T();
            int sizeT = Marshal.SizeOf(defaultValue);
            Bind();
            GL.BufferStorage(mBufferTarget, sizeT, ref defaultValue, BufferStorageFlags.DynamicStorageBit);
        }

        public TypedDynamicUniformBuffer(ShaderProgram program, string UniformBlockName)
        : base(program, UniformBlockName)
        {
            var defaultValue = new T();
            int sizeT = Marshal.SizeOf(defaultValue);
            Bind();
            GL.BufferStorage(mBufferTarget, sizeT, ref defaultValue, BufferStorageFlags.DynamicStorageBit);
        }

        public TypedDynamicUniformBuffer(ShaderProgram program, string UniformBlockName, ref T defaultValue)
            : base(program, UniformBlockName)
        {
            int sizeT = Marshal.SizeOf(defaultValue);
            Bind();
            GL.BufferStorage(mBufferTarget, sizeT, ref defaultValue, BufferStorageFlags.DynamicStorageBit);
        }
    }
}
