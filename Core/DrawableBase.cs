using System;
using System.Collections.Generic;
using Core.Buffer;
using System.Linq;
using System.Linq.Expressions;
using Core.OpenGLShader;
using Core.Primitive;
using OpenTK.Graphics.OpenGL;


namespace Core
{
    public class DrawableBase<T>  where T : struct, IGenericVertexAttribute
    {
        public DrawableBase()
        {
            VB = new StaticVertexBuffer<T>();
            IB = new IndexBuffer();
        }


        public void SetupVertexData(ref T[] VertexList)
        {
            VB.Bind();
            VB.BufferData<T>(ref VertexList);
            VertexCount = VertexList.Count();

            bReadyToDraw = true;
        }

        protected void BindVertexBuffer()
        {
            VB.Bind();
        }

        protected void BindIndexBuffer()
        {
            IB.Bind();
        }

        public void BindVertexAndIndexBuffer()
        {
            BindVertexBuffer();
            BindIndexBuffer();
        }

        public void SetupData(ref T[] VertexList, ref uint[] IndexList)
        {
            VB.Bind();
            VB.BufferData<T>(ref VertexList);
            VertexCount = VertexList.Count();

            IB.Bind();
            IB.BufferData<uint>(ref IndexList);
            IndexCount = IndexList.Count();

            bReadyToDraw = true;
        }

        public virtual void Draw(uint Offset, uint Count)
        {
            //
        }

        public virtual void Draw()
        {
            
        }

        public virtual void DrawArrays(PrimitiveType type)
        {
            if (bReadyToDraw)
            {
                BindVertexBuffer();
                GL.DrawArrays(type, 0, VertexCount);
            }
        }

        public virtual void DrawArraysInstanced(PrimitiveType type, int instancecount)
        {
            if (bReadyToDraw)
            {
                BindVertexBuffer();
                GL.DrawArraysInstanced(type, 0, VertexCount, instancecount);
            }
        }

        public virtual void DrawElementsInstanced(ref uint[] IndexList, PrimitiveType type, int instanceCount)
        {
            if (bReadyToDraw)
            {
                //BindVertexAndIndexBuffer();
                //GL.DrawElementsInstancedBaseInstance(type, IndexCount, DrawElementsType.UnsignedInt,new IntPtr(ref IndexList), instanceCount,0);
            }
        }

        public virtual void DrawElements(PrimitiveType type)
        {
            if(bReadyToDraw)
            {
                BindVertexAndIndexBuffer();
                GL.DrawElements(type, IndexCount, DrawElementsType.UnsignedInt, 0);
            }
        }

        
        public virtual void DrawLinesElements()
        {
            if(bReadyToDraw)
            {
                BindVertexAndIndexBuffer();
                GL.DrawElements(PrimitiveType.Lines, IndexCount, DrawElementsType.UnsignedInt, 0);
            }
        }

        public virtual void DrawTrianglesElements()
        {
            if(bReadyToDraw)
            {
                BindVertexAndIndexBuffer();
                GL.DrawElements(PrimitiveType.Triangles, IndexCount, DrawElementsType.UnsignedInt, 0);
            }
        }

        public virtual void DrawLineStripElements()
        {
            if(bReadyToDraw)
            {
                BindVertexAndIndexBuffer();
                GL.DrawElements(PrimitiveType.LineStrip, IndexCount, DrawElementsType.UnsignedInt, 0);
            }
        }
        

        protected StaticVertexBuffer<T> VB = null;
        protected IndexBuffer IB = null;

        protected int VertexCount = 0;
        protected int IndexCount = 0;
        protected bool bReadyToDraw = false;
    }
}
