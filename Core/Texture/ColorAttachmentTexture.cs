using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;

namespace Core.Texture
{
    public class ColorAttachmentTexture : TextureBase
    {
        public ColorAttachmentTexture(int widthParam, int heightParam)
            : base()
        {
            m_Width = widthParam;
            m_Height = heightParam;
        }

        protected void RecreateTexture()
        {
            if (textureObject != 0)
            {
                GL.DeleteTexture(textureObject);
                textureObject = GL.GenTexture();
            }
        }

        public override void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, textureObject);            
        }

        public void Alloc()
        {
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb16f, m_Width, m_Height, 0, PixelFormat.Rgba, PixelType.Float, new IntPtr(0));
        }

        public void Resize(int newWidth, int newHeight)
        {
            Debug.Assert(newWidth > 0 && newHeight > 0);

            RecreateTexture();
            m_Width = newWidth;
            m_Height = newHeight;
            Bind();
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb16f, m_Width, m_Height, 0, PixelFormat.Rgba, PixelType.Float, new IntPtr(0));            
        }

        protected PixelInternalFormat textureFormat;
        public PixelInternalFormat TextureFormat => textureFormat;

        public int GetTextureObject => textureObject;        
    }
}
