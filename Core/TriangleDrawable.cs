using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using OpenTK;
using OpenTK.Graphics.OpenGL;
using Core.Buffer;
using Core.OpenGLShader;
using Core.Texture;

namespace Core
{
    //@ T : VertexAttribute
    public class TriangleDrawable<T>  where T : struct
    {
        public TriangleDrawable()
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

        public void Bind()
        {            
            VB.Bind();
            IB.Bind();
        }

        public void Draw()
        {
            if (bReadyToDraw)
            {
                VB.Bind();
                IB.Bind();

                VB.BindVertexAttribute();

                GL.DrawElements(PrimitiveType.Triangles, IndexCount, DrawElementsType.UnsignedInt, 0);
            }
        }

        protected StaticVertexBuffer<T> VB = null;
        protected IndexBuffer IB = null;
        
        int IndexCount = 0;
        bool bReadyToDraw = false;
    }
}
