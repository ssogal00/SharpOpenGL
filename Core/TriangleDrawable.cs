using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using OpenTK;
using OpenTK.Graphics.OpenGL;
using Core.Buffer;
using Core.OpenGLShader;
using Core.Texture;

namespace Core
{
    //@ T : VertexAttribute
    public class TriangleDrawable<T> : DrawableBase<T> where T : struct
    {
        public TriangleDrawable()
        {  
        }

        public virtual void Draw()
        {
            if (bReadyToDraw)
            {
                VB.Bind();
                IB.Bind();

                VB.BindVertexAttribute();

                GL.DrawElements(PrimitiveType.Triangles, IndexCount, DrawElementsType.UnsignedInt, 0);
            }
        }

        public virtual void Draw(uint Offset, uint Count)
        {
            if(bReadyToDraw)
            {
                VB.Bind();
                IB.Bind();
                VB.BindVertexAttribute();

                var ByteOffset = new IntPtr(Offset * sizeof(uint));

                GL.DrawElements(PrimitiveType.Triangles, (int)Count, DrawElementsType.UnsignedInt, ByteOffset);
            }
        }
    }
}
