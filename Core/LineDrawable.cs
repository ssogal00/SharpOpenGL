

using Core.Primitive;
using OpenTK.Graphics.OpenGL;

namespace Core
{
    public class LineDrawable<T> : DrawableBase<T> where T : struct, IGenericVertexAttribute
    {
        public LineDrawable()
        {            
        }        
        public override void Draw() 
        {
            if (bReadyToDraw)
            {
                BindVertexAndIndexBuffer();

                GL.DrawElements(PrimitiveType.Lines, IndexCount, DrawElementsType.UnsignedInt, 0);
            }
        }
    }
}
