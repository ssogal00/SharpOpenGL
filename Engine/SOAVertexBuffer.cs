using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Core.Primitive;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Core.Buffer
{
    /// <summary>
    /// Structure of Arrays
    /// 2 attributes
    /// </summary>
    public class SOAVertexBuffer<T1, T2> : IBindable, IDisposable
        where T1 : struct, IGenericVertexAttribute
        where T2 : struct, IGenericVertexAttribute
    {
        SOAVertexBuffer()
        {
            mBufferObject1 = GL.GenBuffer();
            mBufferObject2 = GL.GenBuffer();
        }
        public void Bind()
        {
            mVertexAttribute1.VertexAttributeBinding(0);
            mVertexAttribute2.VertexAttributeBinding(1);
        }

        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void BufferData<T1, T2>(ref T1 Data1, ref T2 Data2)
            where T1 : struct, IGenericVertexAttribute
            where T2 : struct, IGenericVertexAttribute
        {
            Bind();

            GL.BindBuffer(BufferTarget.ArrayBuffer, mBufferObject1);
            var Size1 = new IntPtr(Marshal.SizeOf(Data1));
            GL.BufferData<T1>(BufferTarget.ArrayBuffer, Size1, ref Data1, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, mBufferObject2);
            var Size2 = new IntPtr(Marshal.SizeOf(Data2));
            GL.BufferData<T2>(BufferTarget.ArrayBuffer, Size2, ref Data2, BufferUsageHint.StaticDraw);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(mBufferObject1);
            GL.DeleteBuffer(mBufferObject2);

            mBufferObject1 = mBufferObject2 = 0;
        }

        public void BufferData<T1, T2>(ref T1[] Data1, ref T2[] Data2) 
            where T1 : struct, IGenericVertexAttribute
            where T2 : struct, IGenericVertexAttribute
        {
            Bind();

            if (Data1 != null)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, mBufferObject1);
                var Size = new IntPtr(Marshal.SizeOf(Data1[0]) * Data1.Length);
                GL.BufferData<T1>(BufferTarget.ArrayBuffer, Size, Data1, BufferUsageHint.StaticDraw);
            }

            if (Data2 != null)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, mBufferObject2);
                var Size = new IntPtr(Marshal.SizeOf(Data2[0]) * Data2.Length);
                GL.BufferData<T2>(BufferTarget.ArrayBuffer, Size, Data2, BufferUsageHint.StaticDraw);
            }
        }

        private int mBufferObject1 = 0;
        private int mBufferObject2 = 0;

        private T1 mVertexAttribute1 = default(T1);
        private T2 mVertexAttribute2 = default(T2);
    }


    public class SOAVertexBuffer<T1, T2, T3> : IBindable, IDisposable
        where T1 : struct, IGenericVertexAttribute
        where T2 : struct, IGenericVertexAttribute
        where T3 : struct, IGenericVertexAttribute
    {
        SOAVertexBuffer()
        {
            mBufferObject1 = GL.GenBuffer();
            mBufferObject2 = GL.GenBuffer();
            mBufferObject3 = GL.GenBuffer();
        }
        public void Bind()
        {
            mVertexAttribute1.VertexAttributeBinding(0);
            mVertexAttribute2.VertexAttributeBinding(1);
            mVertexAttribute3.VertexAttributeBinding(2);
        }

        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void BufferData<T1, T2, T3>(ref T1 Data1, ref T2 Data2, ref T3 Data3)
            where T1 : struct, IGenericVertexAttribute
            where T2 : struct, IGenericVertexAttribute
            where T3 : struct, IGenericVertexAttribute
        {
            Bind();

            GL.BindBuffer(BufferTarget.ArrayBuffer, mBufferObject1);
            var Size1 = new IntPtr(Marshal.SizeOf(Data1));
            GL.BufferData<T1>(BufferTarget.ArrayBuffer, Size1, ref Data1, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, mBufferObject2);
            var Size2 = new IntPtr(Marshal.SizeOf(Data2));
            GL.BufferData<T2>(BufferTarget.ArrayBuffer, Size2, ref Data2, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, mBufferObject3);
            var Size3 = new IntPtr(Marshal.SizeOf(Data3));
            GL.BufferData<T3>(BufferTarget.ArrayBuffer, Size3, ref Data3, BufferUsageHint.StaticDraw);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(mBufferObject1);
            GL.DeleteBuffer(mBufferObject2);
            GL.DeleteBuffer(mBufferObject3);

            mBufferObject1 = mBufferObject2 = mBufferObject3= 0;
        }

        private int mBufferObject1 = 0;
        private int mBufferObject2 = 0;
        private int mBufferObject3 = 0;

        private T1 mVertexAttribute1 = default(T1);
        private T2 mVertexAttribute2 = default(T2);
        private T3 mVertexAttribute3 = default(T3);
    }
}
