using OpenTK.Graphics.OpenGL;
using System;
using Core.Primitive;

namespace Core
{
    public class LineDrawable<T> : DrawableBase<T> where T : struct, IGenericVertexAttribute
    {
        public LineDrawable()
        {
        }

        public override void Draw()
        {
            if (mbReadyToDraw)
            {
                BindVertexArray();
                GL.DrawElements(PrimitiveType.Lines, mIndexCount, DrawElementsType.UnsignedInt, 0);
                UnbindVertexArray();
            }
        }

        public override void Draw(uint Offset, uint Count)
        {
            if (mbReadyToDraw)
            {
                var ByteOffset = new IntPtr(Offset * sizeof(uint));

                GL.DrawElements(PrimitiveType.Lines, (int)Count, DrawElementsType.UnsignedInt, ByteOffset);
            }
        }
    }
}
