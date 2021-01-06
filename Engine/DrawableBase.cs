using System;
using System.Collections.Generic;
using System.Diagnostics;
using Core.Buffer;
using System.Linq;
using System.Linq.Expressions;
using Core.OpenGLShader;
using Core.Primitive;
using GLTF;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;


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

    public class GenericMeshDrawable : IDisposable, IBindable
    {
        public GenericMeshDrawable()
        {
            mIndexBuffer = new IndexBuffer();
            mVertexArray = new VertexArray();
        }

        public void Dispose()
        {
            mIndexBuffer?.Dispose();
            mVertexArray?.Dispose();
            mVertexBuffers?.ForEach((x)=> x.Dispose());
        }

        public void Bind()
        {
            mVertexArray.Bind();
            
        }

        public void Unbind()
        {
            mVertexArray.Unbind();
            
        }

        public void SetIndexBufferData(ref uint[] indexList)
        {
            mIndexBuffer.Bind();
            mIndexBuffer.BufferData(ref indexList);
            mIndexCount = indexList.Length;
            mIndexBuffer.Unbind();
            mIndexType = DrawElementsType.UnsignedInt;
        }

        public void SetIndexBufferData(ref ushort[] indexList)
        {
            mIndexBuffer.Bind();
            mIndexBuffer.BufferData(ref indexList);
            mIndexArray = indexList;
            mIndexCount = indexList.Length;
            mIndexBuffer.Unbind();
            mIndexType = DrawElementsType.UnsignedShort;
        }

        public void SetVertexBufferData<T>(int index, ref T[] vertexAttributeData) where T : struct
        {
            Debug.Assert(vertexAttributeData != null && vertexAttributeData.Length > 0);
            
            if (vertexAttributeData[0] is Vector3)
            {
                var vb = new SOAVertexBuffer<Vec3_VertexAttribute>();
                mVertexBuffers.Add(vb);
                vb.BufferData<T>(ref vertexAttributeData);
                vb.BindAtIndex(index);
            }
            else if (vertexAttributeData[0] is Vector2)
            {
                var vb = new SOAVertexBuffer<Vec2_VertexAttribute>();
                mVertexBuffers.Add(vb);
                vb.BufferData<T>(ref vertexAttributeData);
                vb.BindAtIndex(index);
            }
            else if (vertexAttributeData[0] is Vector4)
            {
                var vb = new SOAVertexBuffer<Vec4_VertexAttribute>();
                mVertexBuffers.Add(vb);
                vb.BufferData<T>(ref vertexAttributeData);
                vb.BindAtIndex(index);
            }
        }
        
        public void DrawIndexed()
        {
            mVertexArray.Bind();
            mIndexBuffer.Bind();
            
            GL.DrawElements(PrimitiveType.Triangles, mIndexCount, mIndexType, 0);

            mVertexArray.Unbind();
            mIndexBuffer.Unbind();
        }

        public void DrawIndexed(ref uint[] indexArray)
        {
            mVertexArray.Bind();
            
            GL.DrawElements(PrimitiveType.Triangles, indexArray.Length, DrawElementsType.UnsignedInt, indexArray);

            mVertexArray.Unbind();
        }

        public void DrawIndexed(ref ushort[] indexArray)
        {
            mVertexArray.Bind();

            GL.DrawElements(PrimitiveType.Triangles, indexArray.Length, DrawElementsType.UnsignedShort, indexArray);

            mVertexArray.Unbind();
        }
        

        protected List<IDisposable> mVertexBuffers = new List<IDisposable>();

        protected IndexBuffer mIndexBuffer = null;

        protected ushort[] mIndexArray = null;

        protected int mIndexCount = 0;

        protected VertexArray mVertexArray = null;

        protected DrawElementsType mIndexType = DrawElementsType.UnsignedInt;
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


    public class MeshDrawableBase<T1, T2, T3, T4> : MeshDrawableBase<T1, T2, T3>
        where T1 : struct, IGenericVertexAttribute
        where T2 : struct, IGenericVertexAttribute
        where T3 : struct, IGenericVertexAttribute
        where T4 : struct, IGenericVertexAttribute
    {
        public MeshDrawableBase()
            : base()
        {
            mVB4 = new SOAVertexBuffer<T4>();
        }

        public override void SetupData(ref T1[] vertexAttributeList1, ref uint[] indexList)
        {
            throw new InvalidOperationException();
        }

        public override void SetupData(ref T1[] vertexAttributeList1, ref T2[] vertexAttributeList2, ref uint[] indexList)
        {
            throw new InvalidOperationException();
        }

        public override void SetupData(ref T1[] vertexAttributeList1, ref T2[] vertexAttributeList2,
            ref T3[] vertexAttributeList3, ref uint[] indexList)
        {
            throw new InvalidOperationException();
        }

        public virtual void SetupData(ref T1[] vertexAttributeList1, ref T2[] vertexAttributeList2, ref T3[] vertexAttributeList3, ref T4[] vertexAttributeList4, ref uint[] indexList)
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

            mVB4.BindAtIndex(3);
            mVB4.BufferData<T4>(ref vertexAttributeList4);

            mIB.BufferData<uint>(ref indexList);
            mIndexCount = indexList.Count();

            mbReadyToDraw = true;

            mVA.Unbind();
            mVB1.Unbind();
            mVB2.Unbind();
            mVB3.Unbind();
            mVB4.Unbind();
            mIB.Unbind();
        }

        protected SOAVertexBuffer<T4> mVB4 = null;
    }
}
