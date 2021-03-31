using System;
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

        private GameObject mGameObject = null;

        private VertexArray mVertexArray;

        
    }

    public class CubeRenderThreadObject
    {

    }
}
