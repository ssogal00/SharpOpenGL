using OpenTK.Graphics.OpenGL;
using System;

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
