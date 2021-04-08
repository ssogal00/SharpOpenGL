using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Core.Primitive;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Core.Buffer
{
    public class SOAVertexBuffer<T1> : OpenGLBuffer
        where T1 : struct, IGenericVertexAttribute
    {
        public SOAVertexBuffer()
        {
            mBufferTarget = BufferTarget.ArrayBuffer;
        }
        
        public virtual void BindVertexAttribute(int index)
        {
            mVertexAttribute1.VertexAttributeBinding(index);
        }

        protected void SetArrayData<T>(int bufferObject, ref T[] data) where T : struct
        {
            if (data != null)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, mBufferHandle);
                var size = new IntPtr(Marshal.SizeOf(data[0]) * data.Length);
                GL.BufferData<T>(BufferTarget.ArrayBuffer, size, data, BufferUsageHint.StaticDraw);
            }
        }

        public void BufferData<T>(ref T[] Data1) where T : struct
        {
            SetArrayData(mBufferHandle, ref Data1);
        }

        protected T1 mVertexAttribute1 = default(T1);
    }
}
