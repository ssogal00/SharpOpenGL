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
            if (mbReadyToDraw)
            {
                BindVertexArray();
                GL.DrawElements(PrimitiveType.Quads, mIndexCount, DrawElementsType.UnsignedInt, 0);
                UnbindVertexArray();
            }
        }
    }
}
