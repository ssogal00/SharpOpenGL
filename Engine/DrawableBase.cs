using System;
using System.Collections.Generic;
using System.Diagnostics;
using Core.Buffer;
using System.Linq;
using System.Linq.Expressions;
using Core.OpenGLShader;
using Core.Primitive;
using OpenTK.Graphics.OpenGL;


namespace Core
{
    public class DrawableBase<T> where T : struct, IGenericVertexAttribute
    {
        public DrawableBase()
        {
            mVA = new VertexArray();
            mVB = new AOSVertexBuffer<T>();
            mIB = new IndexBuffer();
        }

        public virtual void SetupVertexData(ref T[] VertexList)
        {
            mVA.Bind();
            mVB.Bind();

            mVB.BufferData<T>(ref VertexList);
            mVertexCount = VertexList.Count();
            mbReadyToDraw = true;

            mVA.Unbind();
            mVB.Unbind();
        }

        public void BindVertexArray()
        {
            mVA.Bind();
        }

        public void UnbindVertexArray()
        {
            mVA.Unbind();
        }

        public void SetupData(ref T[] vertexList, ref uint[] indexList)
        {
            mVA.Bind();
            mVB.Bind();
            mIB.Bind();

            mVB.BufferData<T>(ref vertexList);
            mVertexCount = vertexList.Count();

            mIB.BufferData<uint>(ref indexList);
            mIndexCount = indexList.Count();

            mbReadyToDraw = true;

            mVA.Unbind();
            mVB.Unbind();
            mIB.Unbind();
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
            if (mbReadyToDraw)
            {
                mVA.Bind();
                {
                    GL.DrawArrays(type, 0, mVertexCount);
                }
                mVA.Unbind();
            }
        }

        public virtual void DrawArraysInstanced(PrimitiveType type, int instancecount)
        {
            if (mbReadyToDraw)
            {
                mVA.Bind();
                GL.DrawArraysInstanced(type, 0, mVertexCount, instancecount);
                mVA.Unbind();
            }
        }

        public virtual void DrawElementsInstanced(ref uint[] IndexList, PrimitiveType type, int instanceCount)
        {
            if (mbReadyToDraw)
            {
                //BindVertexArray();
                //GL.DrawElementsInstancedBaseInstance(type, mIndexCount, DrawElementsType.UnsignedInt,new IntPtr(ref indexList), instanceCount,0);
            }
        }

        public virtual void DrawElements(PrimitiveType type)
        {
            if (mbReadyToDraw)
            {
                BindVertexArray();
                GL.DrawElements(type, mIndexCount, DrawElementsType.UnsignedInt, 0);
            }
        }


        public virtual void DrawLinesElements()
        {
            if (mbReadyToDraw)
            {
                BindVertexArray();
                GL.DrawElements(PrimitiveType.Lines, mIndexCount, DrawElementsType.UnsignedInt, 0);
            }
        }

        public virtual void DrawTrianglesElements()
        {
            if (mbReadyToDraw)
            {
                BindVertexArray();
                GL.DrawElements(PrimitiveType.Triangles, mIndexCount, DrawElementsType.UnsignedInt, 0);
            }
        }

        public virtual void DrawLineStripElements()
        {
            if (mbReadyToDraw)
            {
                BindVertexArray();
                GL.DrawElements(PrimitiveType.LineStrip, mIndexCount, DrawElementsType.UnsignedInt, 0);
            }
        }


        protected AOSVertexBuffer<T> mVB = null;
        protected IndexBuffer mIB = null;
        protected VertexArray mVA = null;

        protected int mVertexCount = 0;
        protected int mIndexCount = 0;
        protected bool mbReadyToDraw = false;
    }

    /// use seperate vertex attributes
    /// Structure of array way
    ///

    public class MeshDrawableBase<T1> where T1 : struct, IGenericVertexAttribute
    {
        public MeshDrawableBase()
        {
            mVB1 = new SOAVertexBuffer<T1>();
            mIB = new IndexBuffer();
            mVA = new VertexArray();
        }

        public virtual void SetupData(ref T1[] vertexAttributeList1, ref uint[] indexList)
        {
            Debug.Assert(vertexAttributeList1 != null);

            mVA.Bind();
            mIB.Bind();

            mVB1.BindAtIndex(0);
            mVB1.BufferData<T1>(ref vertexAttributeList1);
            mVertexCount = vertexAttributeList1.Count();

            mIB.BufferData<uint>(ref indexList);
            mIndexCount = indexList.Count();

            mbReadyToDraw = true;

            mVA.Unbind();
            mVB1.Unbind();
            mIB.Unbind();
        }

        public virtual void DrawArrays(PrimitiveType type)
        {
            if (mbReadyToDraw)
            {
                mVA.Bind();
                {
                    GL.DrawArrays(type, 0, mVertexCount);
                }
                mVA.Unbind();
            }
        }

        public virtual void DrawArraysInstanced(PrimitiveType type, int instancecount)
        {
            if (mbReadyToDraw)
            {
                mVA.Bind();
                GL.DrawArraysInstanced(type, 0, mVertexCount, instancecount);
                mVA.Unbind();
            }
        }

        protected SOAVertexBuffer<T1> mVB1 = null;
        protected IndexBuffer mIB = null;
        protected VertexArray mVA = null;

        protected int mVertexCount = 0;
        protected int mIndexCount = 0;
        protected bool mbReadyToDraw = false;
    }

    public class MeshDrawableBase<T1, T2> : MeshDrawableBase<T1>
        where T1 : struct, IGenericVertexAttribute
        where T2 : struct, IGenericVertexAttribute
    {
        public MeshDrawableBase()
        :base()
        {
            mVB2 = new SOAVertexBuffer<T2>();
        }

        public override void SetupData(ref T1[] vertexAttributeList1, ref uint[] indexList)
        {
            throw new InvalidOperationException();
        }

        public virtual void SetupData(ref T1[] vertexAttributeList1, ref T2[] vertexAttributeList2, ref uint[] indexList)
        {
            Debug.Assert(vertexAttributeList1 != null && vertexAttributeList2 != null);
            Debug.Assert(vertexAttributeList1.Length == vertexAttributeList2.Length);

            mVA.Bind();
            mIB.Bind();

            mVB1.BindAtIndex(0);
            mVB1.BufferData<T1>(ref vertexAttributeList1);
            mVertexCount = vertexAttributeList1.Count();

            mVB2.BindAtIndex(1);
            mVB2.BufferData<T2>(ref vertexAttributeList2);

            mIB.BufferData<uint>(ref indexList);
            mIndexCount = indexList.Count();

            mbReadyToDraw = true;

            mVA.Unbind();
            mVB1.Unbind();
            mVB2.Unbind();
            mIB.Unbind();
        }

        protected SOAVertexBuffer<T2> mVB2 = null;
    }

    public class MeshDrawableBase<T1, T2,T3> : MeshDrawableBase<T1,T2>
        where T1 : struct, IGenericVertexAttribute
        where T2 : struct, IGenericVertexAttribute
        where T3 : struct, IGenericVertexAttribute
    {
        public MeshDrawableBase()
            : base()
        {
            mVB3 = new SOAVertexBuffer<T3>();
        }

        public override void SetupData(ref T1[] vertexAttributeList1, ref uint[] indexList)
        {
            throw new InvalidOperationException();
        }

        public override void SetupData(ref T1[] vertexAttributeList1, ref T2[] vertexAttributeList2, ref uint[] indexList)
        {
            throw new InvalidOperationException();
        }

        public virtual void SetupData(ref T1[] vertexAttributeList1, ref T2[] vertexAttributeList2, ref T3[] vertexAttributeList3, ref uint[] indexList)
        {
            Debug.Assert(vertexAttributeList1 != null && vertexAttributeList2 != null);
            Debug.Assert(vertexAttributeList1.Length == vertexAttributeList2.Length);

            mVA.Bind();
            mIB.Bind();

            // attribute 1
            mVB1.BindAtIndex(0);
            mVB1.BufferData<T1>(ref vertexAttributeList1);
            mVertexCount = vertexAttributeList1.Count();

            // attribute 2
            mVB2.BindAtIndex(1);
            mVB2.BufferData<T2>(ref vertexAttributeList2);

            // attribute 3
            mVB3.BindAtIndex(2);
            mVB3.BufferData<T3>(ref vertexAttributeList3);

            mIB.BufferData<uint>(ref indexList);
            mIndexCount = indexList.Count();

            mbReadyToDraw = true;

            mVA.Unbind();
            mVB1.Unbind();
            mVB2.Unbind();
            mVB3.Unbind();
            mIB.Unbind();
        }

        protected SOAVertexBuffer<T3> mVB3 = null;
    }
}
