using System;
using System.Collections.Generic;
using System.Text;
using Core;
using OpenTK.Graphics.OpenGL;

namespace Engine.Rendering
{
    public class ImageTexture : IDisposable, IBindable
    {
        public ImageTexture(int width, int height)
        {
            mTextureObject = GL.GenTexture();
            
        }

        public void Dispose()
        {
            if (mTextureObject != -1)
            {
                GL.DeleteTexture(mTextureObject);
                mTextureObject = -1;
            }
        }

        public void Bind()
        {
            GL.BindImageTexture(0, mTextureObject, 0, false, 0, TextureAccess.ReadWrite, SizedInternalFormat.Rg32i);
        }

        public void Unbind()
        {

        }

        protected int mTextureObject = -1;
        protected SizedInternalFormat mSizedInternalFormat = SizedInternalFormat.Rg32i;
    }
}
