using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;

namespace Core.Texture
{
    public class DepthTargetTexture : TextureBase
    {
        public DepthTargetTexture(int widthParam, int heightParam)
            : base()
        {
            m_Width = widthParam;
            m_Height = heightParam;
        }

        protected virtual void RecreateTexture()
        {
            if(textureObject != 0)
            {
                GL.DeleteTexture(textureObject);
                textureObject = GL.GenTexture();
            }
        }

        public virtual void Resize(int newWidth, int newHeight)
        {
            Debug.Assert(newWidth > 0 && newHeight > 0);

            RecreateTexture();
            m_Width = newWidth;
            m_Height = newHeight;
            Bind();
            Alloc();
        }

        protected virtual void Alloc()
        {   
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Depth24Stencil8, m_Width, m_Height, 0, PixelFormat.DepthComponent, PixelType.Float, new IntPtr(0));
        }
        

        public int GetTextureObject => textureObject;        
    }
}
