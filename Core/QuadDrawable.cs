using System;
using Core.Primitive;
using OpenTK.Graphics.OpenGL;

namespace Core
{
    public class QuadDrawable<T> : DrawableBase<T> where T : struct, IGenericVertexAttribute
    {
        public QuadDrawable()
        {
        }
        public override void Draw()
        {
            if (bReadyToDraw)
            {
                BindVertexAndIndexBuffer();

                GL.DrawElements(PrimitiveType.Quads, IndexCount, DrawElementsType.UnsignedInt, 0);
            }
        }
    }
}
