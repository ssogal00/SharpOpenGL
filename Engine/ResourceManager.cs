using System;
using System.Collections.Generic;
using System.Text;
using Core;
using Core.Buffer;
using Core.Primitive;

namespace Engine
{
    public class ResourceManager : Singleton<ResourceManager>
    {
        public IndexBuffer CreateIndexBuffer()
        {
            var ib = new IndexBuffer();
            mIndexBufferHandles.Add(ib.BufferHandle, ib);
            return ib;
        }

        public void DeleteIndexBuffer(IndexBuffer ib)
        {
            DeleteIndexBuffer(ib.BufferHandle);
        }


        public void DeleteIndexBuffer(int handle)
        {
            if (mIndexBufferHandles.ContainsKey(handle))
            {
                mIndexBufferHandles[handle].Dispose();
                mIndexBufferHandles.Remove(handle);
            }
        }

        public VertexArray CreateVertexArray(string alias = "")
        {
            var va = new VertexArray(alias);
            mVertexArrayHandles.Add(va.Handle, va);
            return va;
        }

        public void DeleteVertexArray(VertexArray array)
        {
            DeleteVertexArray(array.Handle);
        }

        public void DeleteVertexArray(int handle)
        {
            if (mVertexArrayHandles.ContainsKey(handle))
            {
                mVertexArrayHandles[handle].Dispose();
                mVertexArrayHandles.Remove(handle);
            }
        }

        public AOSVertexBuffer<T> CreateAOSVertexBuffer<T>(string alias = "") where T : struct, IGenericVertexAttribute
        {
            var buffer = new AOSVertexBuffer<T>();
            mVertexBufferHandles.Add(buffer.BufferHandle, buffer);
            return buffer;
        }

        public void DeleteVertexBuffer(int handle)
        {
            if (mVertexBufferHandles.ContainsKey(handle))
            {
                mVertexBufferHandles[handle].Dispose();
                mVertexBufferHandles.Remove(handle);
            }
        }

        private Dictionary<int, IndexBuffer> mIndexBufferHandles = new Dictionary<int, IndexBuffer>();
        private Dictionary<int, VertexArray> mVertexArrayHandles = new Dictionary<int, VertexArray>();
        private Dictionary<int, OpenGLBuffer> mVertexBufferHandles = new Dictionary<int, OpenGLBuffer>();
    }
}

