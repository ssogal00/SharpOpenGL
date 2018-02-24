using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;

namespace Core.Texture
{
    public class RenderTargetTexture : IDisposable
    {
        public RenderTargetTexture(int widthParam, int heightParam, PixelInternalFormat formatParam)
        {
            TextureObject = GL.GenTexture();
            width = widthParam;
            height = heightParam;
            textureFormat = formatParam;
        }

        protected void RecreateTexture()
        {
            if (TextureObject != 0)
            {
                GL.DeleteTexture(TextureObject);
                TextureObject = GL.GenTexture();
            }
        }

        public void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, TextureObject);            
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba16, width, height, 0, PixelFormat.Rgba, PixelType.Float, new IntPtr(0));            
        }

        public void Unbind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Dispose()
        {
            if(TextureObject != 0)
            {
                GL.DeleteTexture(TextureObject);
                TextureObject = 0;
            }
        }

        public void Resize(int newWidth, int newHeight)
        {
            Debug.Assert(newWidth > 0 && newHeight > 0);

            RecreateTexture();
            width = newWidth;
            height = newHeight;
            Bind();
        }

        protected int width;
        public int Width => width;

        protected int height;
        public int Height => height;

        protected PixelInternalFormat textureFormat;
        public PixelInternalFormat TextureFormat => textureFormat;

        protected int TextureObject;
    }
}
