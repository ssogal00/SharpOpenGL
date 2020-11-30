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
            VA = new VertexArray();
            VB = new StaticVertexBuffer<T>();
            IB = new IndexBuffer();
        }


        public void SetupVertexData(ref T[] VertexList)
        {
            
            VA.Bind();
            VB.Bind();
            
            VB.BufferData<T>(ref VertexList);
            VertexCount = VertexList.Count();
            bReadyToDraw = true;
            
            VA.Unbind();
            VB.Unbind();
        }

        public void BindVertexArray()
        {
            VA.Bind();
        }

        public void UnbindVertexArray()
        {
            VA.Unbind();
        }

        public void SetupData(ref T[] VertexList, ref uint[] IndexList)
        {
            VA.Bind();
            VB.Bind();
            IB.Bind();
            
            VB.BufferData<T>(ref VertexList);
            VertexCount = VertexList.Count();

            IB.BufferData<uint>(ref IndexList);
            IndexCount = IndexList.Count();

            bReadyToDraw = true;

            VA.Unbind();
            VB.Unbind();
            IB.Unbind();
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
                VA.Bind();
                {
                    GL.DrawArrays(type, 0, VertexCount);
                }
                VA.Unbind();
            }
        }

        public virtual void DrawArraysInstanced(PrimitiveType type, int instancecount)
        {
            if (bReadyToDraw)
            {
                VA.Bind();
                GL.DrawArraysInstanced(type, 0, VertexCount, instancecount);
                VA.Unbind();
            }
        }

        public virtual void DrawElementsInstanced(ref uint[] IndexList, PrimitiveType type, int instanceCount)
        {
            if (bReadyToDraw)
            {
                //BindVertexArray();
                //GL.DrawElementsInstancedBaseInstance(type, IndexCount, DrawElementsType.UnsignedInt,new IntPtr(ref IndexList), instanceCount,0);
            }
        }

        public virtual void DrawElements(PrimitiveType type)
        {
            if(bReadyToDraw)
            {
                BindVertexArray();
                GL.DrawElements(type, IndexCount, DrawElementsType.UnsignedInt, 0);
            }
        }

        
        public virtual void DrawLinesElements()
        {
            if(bReadyToDraw)
            {
                BindVertexArray();
                GL.DrawElements(PrimitiveType.Lines, IndexCount, DrawElementsType.UnsignedInt, 0);
            }
        }

        public virtual void DrawTrianglesElements()
        {
            if(bReadyToDraw)
            {
                BindVertexArray();
                GL.DrawElements(PrimitiveType.Triangles, IndexCount, DrawElementsType.UnsignedInt, 0);
            }
        }

        public virtual void DrawLineStripElements()
        {
            if(bReadyToDraw)
            {
                BindVertexArray();
                GL.DrawElements(PrimitiveType.LineStrip, IndexCount, DrawElementsType.UnsignedInt, 0);
            }
        }
        

        protected StaticVertexBuffer<T> VB = null;
        protected IndexBuffer IB = null;
        protected VertexArray VA = null;

        protected int VertexCount = 0;
        protected int IndexCount = 0;
        protected bool bReadyToDraw = false;
    }
}
