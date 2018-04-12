using Core.Buffer;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace Core
{
    public class DrawableBase<T>  where T : struct 
    {
        public DrawableBase()
        {
            VB = new StaticVertexBuffer<T>();
            IB = new IndexBuffer();
        }

        protected void SetupVertexData(ref T[] VertexList)
        {
            VB.BufferData<T>(ref VertexList);
        }

        protected void SetupIndexData(ref uint[] IndexList)
        {
            IB.BufferData<uint>(ref IndexList);
        }

        public void BindVertexAndIndexBuffer()
        {
            VB.Bind();
            IB.Bind();
            VB.BindVertexAttribute();
        }

        public void SetupData(ref T[] VertexList, ref uint[] IndexList)
        {
            VB.Bind();
            VB.BufferData<T>(ref VertexList);

            IB.Bind();
            IB.BufferData<uint>(ref IndexList);
            IndexCount = IndexList.Count();
            bReadyToDraw = true;
        }
      
        public virtual void Draw()
        {
            
        }

        public virtual void DrawPrimitive(PrimitiveType type)
        {
            if(bReadyToDraw)
            {
                BindVertexAndIndexBuffer();
                GL.DrawElements(type, IndexCount, DrawElementsType.UnsignedInt, 0);
            }
        }
        
        public virtual void DrawLinesPrimitive()
        {
            if(bReadyToDraw)
            {
                BindVertexAndIndexBuffer();
                GL.DrawElements(PrimitiveType.Lines, IndexCount, DrawElementsType.UnsignedInt, 0);
            }
        }

        public virtual void DrawTrianglesPrimitive()
        {
            if(bReadyToDraw)
            {
                BindVertexAndIndexBuffer();
                GL.DrawElements(PrimitiveType.Triangles, IndexCount, DrawElementsType.UnsignedInt, 0);
            }
        }

        public virtual void DrawLineStripPrimitive()
        {
            if(bReadyToDraw)
            {
                BindVertexAndIndexBuffer();
                GL.DrawElements(PrimitiveType.LineStrip, IndexCount, DrawElementsType.UnsignedInt, 0);
            }
        }

        public virtual void Draw(uint Offset , uint Count)
        {
            //
        }

        protected StaticVertexBuffer<T> VB = null;
        protected IndexBuffer IB = null;

        protected int IndexCount = 0;
        protected bool bReadyToDraw = false;
    }
}
