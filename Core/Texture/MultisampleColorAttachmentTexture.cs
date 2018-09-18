using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace Core.Texture
{
    class MultisampleColorAttachmentTexture : TextureBase
    {
        public MultisampleColorAttachmentTexture(int widthParam, int heightParam, PixelInternalFormat format = PixelInternalFormat.Rgba16f)
        {
            m_Width = widthParam;
            m_Height = heightParam;
            textureFormat = format;
        }

        protected void RecreateTexture()
        {
            if (textureObject != 0)
            {
                GL.DeleteTexture(textureObject);
                GL.CreateTextures(TextureTarget.Texture2DMultisample, 1, out textureObject);
            }
        }

        public override void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2DMultisample, textureObject);
        }

        public void Alloc()
        {
            GL.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, 4, textureFormat, m_Width, m_Height, false);
        }

        public void Resize(int newWidth, int newHeight)
        {
            Debug.Assert(newWidth > 0 && newHeight > 0);

            RecreateTexture();
            m_Width = newWidth;
            m_Height = newHeight;
            Bind();
            
            
            GL.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, 4, textureFormat, m_Width, m_Height, false);
        }

        protected PixelInternalFormat textureFormat;
        public PixelInternalFormat TextureFormat => textureFormat;

        public int GetTextureObject => textureObject;
    }
}
