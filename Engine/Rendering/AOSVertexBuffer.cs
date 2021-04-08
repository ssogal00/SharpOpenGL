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
    
    ///
    /// Array of Structures 
    /// 
    public class AOSVertexBuffer<T> : OpenGLBuffer where T : struct, IGenericVertexAttribute
    {   
        public AOSVertexBuffer()
        {
            mBufferTarget = BufferTarget.ArrayBuffer;
            mHint = BufferUsageHint.StaticDraw;
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
    }
}
