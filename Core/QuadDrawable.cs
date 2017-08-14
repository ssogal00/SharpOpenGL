using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Core
{
    public class QuadDrawable<T> : DrawableBase<T> where T : struct
    {
        public QuadDrawable()
        {
        }
        public virtual void Draw()
        {
            if (bReadyToDraw)
            {
                VB.Bind();
                IB.Bind();

                VB.BindVertexAttribute();

                GL.DrawElements(PrimitiveType.Quads, IndexCount, DrawElementsType.UnsignedInt, 0);
            }
        }
    }
}
