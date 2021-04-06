using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Core.Buffer
{
    public class VertexArray : IBindable, IDisposable
    {
        private int mVertexArray = 0;

        public int Handle => mVertexArray;

        private string mAlias = "";

        public VertexArray()
        {
            mVertexArray = GL.GenVertexArray();
        }

        public VertexArray(string alias)
        {
            mVertexArray = GL.GenVertexArray();
            mAlias = alias;
        }

        public void Bind()
        {
            GL.BindVertexArray(mVertexArray);
        }

        public void Unbind()
        {
            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            if (mVertexArray != 0)
            {
                GL.DeleteVertexArray(mVertexArray);
                mVertexArray = 0;
            }
        }
    }
}
