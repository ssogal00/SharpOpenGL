using OpenTK.Graphics.OpenGL;
using System;
using Core.Primitive;

namespace Core
{
    //@ T : VertexAttribute
    public class TriangleDrawable<T> : DrawableBase<T> where T : struct, IGenericVertexAttribute
    {
        public TriangleDrawable()
        {  
        }

        public override void Draw()
        {
            if (bReadyToDraw)
            {
                BindVertexAndIndexBuffer();
                GL.DrawElements(PrimitiveType.Triangles, IndexCount, DrawElementsType.UnsignedInt, 0);
            }
        }

        public override void Draw(uint Offset, uint Count)
        {
            if(bReadyToDraw)
            {
                var ByteOffset = new IntPtr(Offset * sizeof(uint));

                GL.DrawElements(PrimitiveType.Triangles, (int)Count, DrawElementsType.UnsignedInt, ByteOffset);
            }
        }
    }
}
