using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Core.Primitive;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Core.Buffer
{
    public interface IVertexBuffer
    {
    }

    public class SOAVertexBuffer<T1> : IBindable, IDisposable
        where T1 : struct, IGenericVertexAttribute
    {
        public SOAVertexBuffer()
        {
            mBufferObject1 = GL.GenBuffer();
        }
        
        public virtual void Bind()
        {
        }

        public virtual void BindAtIndex(int index)
        {
            mVertexAttribute1.VertexAttributeBinding(index);
        }

        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        public virtual void Dispose()
        {
            GL.DeleteBuffer(mBufferObject1);

            mBufferObject1 = 0;
        }

        protected void SetData<T>(int bufferObject, ref T data) where T : struct
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, mBufferObject1);
            var size = new IntPtr(Marshal.SizeOf(data));
            GL.BufferData<T>(BufferTarget.ArrayBuffer, size, ref data, BufferUsageHint.StaticDraw);
        }

        protected void SetArrayData<T>(int bufferObject, ref T[] data) where T : struct
        {
            if (data != null)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, mBufferObject1);
                var size = new IntPtr(Marshal.SizeOf(data[0]) * data.Length);
                GL.BufferData<T>(BufferTarget.ArrayBuffer, size, data, BufferUsageHint.StaticDraw);
            }
        }

        public void BufferData<T>(ref T Data1) where T : struct
        {
            SetData<T>(mBufferObject1, ref Data1);
        }

        public void BufferData<T>(ref T[] Data1) where T : struct
        {
            SetArrayData(mBufferObject1, ref Data1);
        }

        protected int mBufferObject1 = 0;

        protected T1 mVertexAttribute1 = default(T1);
    }

    /// <summary>
    /// Structure of Arrays
    /// 2 attributes
    /// </summary>
    public class SOAVertexBuffer<T1, T2> : SOAVertexBuffer<T1>
        where T1 : struct, IGenericVertexAttribute
        where T2 : struct, IGenericVertexAttribute
    {
        public SOAVertexBuffer()
        :base()
        {
            mBufferObject2 = GL.GenBuffer();
        }
        public override void Bind()
        {
            mVertexAttribute1.VertexAttributeBinding(0);
            mVertexAttribute2.VertexAttributeBinding(1);
        }

        public override void BindAtIndex(int index)
        {
            throw new InvalidOperationException("Invalid operation");
        }

        public override void Dispose()
        {
            GL.DeleteBuffer(mBufferObject1);
            GL.DeleteBuffer(mBufferObject2);

            mBufferObject1 = mBufferObject2 = 0;
        }

        public void BufferData<T1, T2>(ref T1 Data1, ref T2 Data2)
            where T1 : struct, IGenericVertexAttribute
            where T2 : struct, IGenericVertexAttribute
        {
            Bind();
            SetData<T1>(mBufferObject1, ref Data1);
            SetData<T2>(mBufferObject2, ref Data2);
        }
        
        public void BufferData<T1, T2>(ref T1[] Data1, ref T2[] Data2) 
            where T1 : struct, IGenericVertexAttribute
            where T2 : struct, IGenericVertexAttribute
        {
            Bind();
            SetArrayData(mBufferObject1, ref Data1);
            SetArrayData(mBufferObject2, ref Data2);
        }

        protected int mBufferObject2 = 0;
        protected T2 mVertexAttribute2 = default(T2);
    }


    public class SOAVertexBuffer<T1, T2, T3> : SOAVertexBuffer<T1,T2>
        where T1 : struct, IGenericVertexAttribute
        where T2 : struct, IGenericVertexAttribute
        where T3 : struct, IGenericVertexAttribute
    {
        public SOAVertexBuffer()
        : base()
        {
            mBufferObject3 = GL.GenBuffer();
        }
        public override void Bind()
        {
            base.Bind();
            mVertexAttribute3.VertexAttributeBinding(2);
        }

        public void BufferData<T1, T2, T3>(ref T1 Data1, ref T2 Data2, ref T3 Data3)
            where T1 : struct, IGenericVertexAttribute
            where T2 : struct, IGenericVertexAttribute
            where T3 : struct, IGenericVertexAttribute
        {
            Bind();

            SetData(mBufferObject1, ref Data1);

            SetData(mBufferObject2, ref Data2);

            SetData(mBufferObject3, ref Data3);
        }

        public void BufferData<T1, T2, T3>(ref T1[] Data1, ref T2[] Data2, ref T3[] Data3)
            where T1 : struct, IGenericVertexAttribute
            where T2 : struct, IGenericVertexAttribute
            where T3 : struct, IGenericVertexAttribute
        {
            Bind();

            SetArrayData(mBufferObject1, ref Data1);
            SetArrayData(mBufferObject2, ref Data2);
            SetArrayData(mBufferObject3, ref Data3);
        }

        public override void Dispose()
        {
            base.Dispose();
            GL.DeleteBuffer(mBufferObject3);

            mBufferObject1 = mBufferObject2 = mBufferObject3= 0;
        }
        
        protected int mBufferObject3 = 0;
        protected T3 mVertexAttribute3 = default(T3);
    }

    public class SOAVertexBuffer<T1, T2, T3,T4> : SOAVertexBuffer<T1,T2,T3>
        where T1 : struct, IGenericVertexAttribute
        where T2 : struct, IGenericVertexAttribute
        where T3 : struct, IGenericVertexAttribute
        where T4 : struct, IGenericVertexAttribute
    {
        public SOAVertexBuffer()
        : base()
        {
            mBufferObject4 = GL.GenBuffer();
        }
        public override void Bind()
        {
            base.Bind();
            mVertexAttribute4.VertexAttributeBinding(3);
        }

        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void BufferData<T1, T2, T3, T4>(ref T1 Data1, ref T2 Data2, ref T3 Data3, ref T4 Data4)
            where T1 : struct, IGenericVertexAttribute
            where T2 : struct, IGenericVertexAttribute
            where T3 : struct, IGenericVertexAttribute
            where T4 : struct, IGenericVertexAttribute
        {
            Bind();

            SetData(mBufferObject1, ref Data1);
            SetData(mBufferObject2, ref Data2);
            SetData(mBufferObject3, ref Data3);
            SetData(mBufferObject4, ref Data4);
        }

        public override void Dispose()
        {
            GL.DeleteBuffer(mBufferObject1);
            GL.DeleteBuffer(mBufferObject2);
            GL.DeleteBuffer(mBufferObject3);
            GL.DeleteBuffer(mBufferObject4);

            mBufferObject1 = mBufferObject2 = mBufferObject3 = mBufferObject4 = 0;
        }

        
        protected int mBufferObject4 = 0;
        protected T4 mVertexAttribute4 = default(T4);
    }
}
