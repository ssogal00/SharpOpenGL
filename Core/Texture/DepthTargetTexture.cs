using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;

namespace Core.Texture
{
    public class DepthTargetTexture
    {
        public DepthTargetTexture(int widthParam, int heightParam)
        {
            width = widthParam;
            height = heightParam;

            TextureObject = GL.GenTexture();
        }

        protected void RecreateTexture()
        {
            if(TextureObject != 0)
            {
                GL.DeleteTexture(TextureObject);
                TextureObject = GL.GenTexture();
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

        public void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, TextureObject);            
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Depth24Stencil8, width, height, 0, PixelFormat.DepthComponent, PixelType.Float, new IntPtr(0));
        }

        public void Unbind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public int GetTextureObject => TextureObject;

        protected int TextureObject = 0;

        protected int width = 0;
        public int Width => width;
        
        protected int height = 0;
        public int Height => height;
    }
}
