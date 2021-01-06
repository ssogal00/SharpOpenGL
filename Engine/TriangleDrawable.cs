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
            if (mbReadyToDraw)
            {
                BindVertexArray();
                BindIndexBuffer();
                GL.DrawElements(PrimitiveType.Triangles, mIndexCount, DrawElementsType.UnsignedInt, 0);
                UnbindIndexBuffer();
                UnbindVertexArray();
            }
        }

        public override void Draw(uint Offset, uint Count)
        {
            if(mbReadyToDraw)
            {
                BindVertexArray();
                BindIndexBuffer();
                var ByteOffset = new IntPtr(Offset * sizeof(uint));
                GL.DrawElements(PrimitiveType.Triangles, (int)Count, DrawElementsType.UnsignedInt, ByteOffset);
                UnbindIndexBuffer();
                UnbindVertexArray();
            }
        }
    }
}
