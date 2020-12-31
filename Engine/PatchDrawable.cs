using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Primitive;
using OpenTK.Graphics.OpenGL;


namespace Core
{
    public class PatchDrawable<T> : DrawableBase<T> where T : struct, IGenericVertexAttribute
    {
        public PatchDrawable() { }

        public override void Draw()
        {
            if (mbReadyToDraw)
            {
                BindVertexArray();
                GL.DrawElements(PrimitiveType.Patches, mIndexCount, DrawElementsType.UnsignedInt, 0);
                UnbindVertexArray();
            }
        }
    
    }

}
