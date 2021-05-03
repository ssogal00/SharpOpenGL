using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace Engine.Query
{
    public class QueryObject : IDisposable
    {
        public QueryObject()
        {
            Debug.Assert(RenderingThread.IsInRenderingThread());
            mQueryObject = GL.GenQuery();
        }

        public void Dispose()
        {
            GL.DeleteQuery(mQueryObject);
        }

        protected int mQueryObject = -1;
    }
}
