using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using OpenTK.Graphics.OpenGL;
using Core.Buffer;

namespace Core
{
    public class LineDrawable<T> : DrawableBase<T> where T : struct
    {
        public LineDrawable()
        {            
        }        
        public override void Draw() 
        {
            if (bReadyToDraw)
            {
                VB.Bind();
                IB.Bind();                                

                VB.BindVertexAttribute();

                GL.DrawElements(PrimitiveType.Lines, IndexCount, DrawElementsType.UnsignedInt, 0);
            }
        }
    }
}
