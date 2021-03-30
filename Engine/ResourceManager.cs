using System;
using System.Collections.Generic;
using System.Text;
using Core;
using Core.Buffer;

namespace Engine
{
    public class ResourceManager : Singleton<ResizableManager>
    {
        public IndexBuffer CreateIndexBuffer()
        {
            var ib = new IndexBuffer();
            mIndexBufferHandles.Add(ib.BufferObject, ib);
            return ib;
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

        public void DeleteVertexArray(int handle)
        {
            if (mVertexArrayHandles.ContainsKey(handle))
            {
                mVertexArrayHandles[handle].Dispose();
                mVertexArrayHandles.Remove(handle);
            }
        }

        public void CreateAOSVertexBuffer<T>(string alias = "")
        {
            new AOSVertexBuffer<t>()
        }

        private Dictionary<int, IndexBuffer> mIndexBufferHandles = new Dictionary<int, IndexBuffer>();
        private Dictionary<int, VertexArray> mVertexArrayHandles = new Dictionary<int, VertexArray>();
    }
}

