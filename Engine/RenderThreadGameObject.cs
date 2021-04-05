﻿using System;
using System.Collections.Generic;
using System.Text;
using Core;
using Core.Buffer;
using Core.Primitive;

namespace Engine
{
    public class RenderThreadGameObject : IDisposable
    {
        public RenderThreadGameObject(GameObject gameObject)
        {
            mGameObject = gameObject;

            
        }

        public virtual void Dispose()
        {

        }


        public virtual void Render()
        {

        }

        protected void Intialize()
        {
            mVertexArray = ResourceManager.Instance.CreateVertexArray();
            mIndexBuffer = ResourceManager.Instance.CreateIndexBuffer();
        }

        //
        protected void UpdateMaterialParams()
        {

        }

        private GameObject mGameObject = null;

        private VertexArray mVertexArray;

        private IndexBuffer mIndexBuffer;

        private Dictionary<int, OpenGLBuffer> mVertexAttributeMap = new Dictionary<int, OpenGLBuffer>();
    }
}
