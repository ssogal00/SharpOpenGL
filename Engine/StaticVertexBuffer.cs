using System;
using System.Collections.Generic;
using System.Linq;
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

    public class StaticVertexBuffer<T> : OpenGLBuffer where T : struct, IGenericVertexAttribute
    {   
        public StaticVertexBuffer()
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
