using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Core.Primitive;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Core.Buffer
{
    public class VertexArray : IBindable, IDisposable
    {
        private int mVertexArray = 0;

        public VertexArray()
        {
            mVertexArray = GL.GenVertexArray();
        }

        public void Bind()
        {
            GL.BindVertexArray(mVertexArray);
        }

        public void Unbind()
        {
            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            if (mVertexArray != 0)
            {
                GL.DeleteVertexArray(mVertexArray);
                mVertexArray = 0;
            }
        }
    }

    /// <summary>
    /// Structure of Arrays
    /// </summary>
    public class SOAVertexBuffer<T1, T2> : IBindable, IDisposable
    {
        SOAVertexBuffer()
        {
            mBufferObject1 = GL.GenBuffer();
            mBufferObject2 = GL.GenBuffer();
        }
        public void Bind()
        {
            
        }

        public void Unbind()
        {

        }

        public void BufferData<T1,T2>(ref T1 Data1, ref T2 Data2) where T1: struct
                                                    where T2 : struct
        {
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

        private int mBufferObject1 = 0;
        private int mBufferObject2 = 0;
    }


    ///
    /// Array of Structures 
    /// 
    public class AOSVertexBuffer<T> : OpenGLBuffer where T : struct, IGenericVertexAttribute
    {   
        public AOSVertexBuffer()
        {
            bufferTarget = BufferTarget.ArrayBuffer;
            hint = BufferUsageHint.StaticDraw;
            Count++;
        }

        public new void Dispose()
        {
            base.Dispose();
            Count--;
        }

        public override void Bind()
        {
            base.Bind();
            vertexAttributeInstance.VertexAttributeBinding();
        }

        protected static int Count = 0;

        public static int StaticVertexBufferCount => Count;

        private T vertexAttributeInstance = default(T);

        
        public void BindVertexAttribute()
        {
            vertexAttributeInstance.VertexAttributeBinding();
        }

        public bool IsCompatible(MaterialBase.MaterialBase material)
        {
            var vbAttributes = vertexAttributeInstance.GetVertexAttributes();
            var materialAttributes = material.GetVertexAttributes();

            if (vbAttributes.Count != materialAttributes.Count)
            {
                return false;
            }

            for (int i = 0; i < vbAttributes.Count; ++i)
            {
                if (vbAttributes[i].IsCompatible(materialAttributes[i]) == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
