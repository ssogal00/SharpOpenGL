using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using OpenTK.Graphics.OpenGL;
using Core.Buffer;


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

        public void SetupData(ref T[] VertexList, ref uint[] IndexList)
        {
            VB.BufferData<T>(ref VertexList);
            IB.BufferData<uint>(ref IndexList);
            IndexCount = IndexList.Count();
            bReadyToDraw = true;
        }
      
        public virtual void Draw()
        {            
        }

        protected StaticVertexBuffer<T> VB = null;
        protected IndexBuffer IB = null;

        protected int IndexCount = 0;
        protected bool bReadyToDraw = false;
    }
}
