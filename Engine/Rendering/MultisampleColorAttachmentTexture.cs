using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace Core.Texture
{
    public class MultisampleColorAttachmentTexture : ColorAttachmentTexture
    {
        public MultisampleColorAttachmentTexture(int widthParam, int heightParam, 
            PixelInternalFormat pixelInternalFormat = PixelInternalFormat.Rgba16f, PixelFormat pixelformat = PixelFormat.Rgba, PixelType pixelType = PixelType.Float)
        : base(widthParam, heightParam, pixelInternalFormat, pixelformat, pixelType)
        {
        }

        protected override void RecreateTexture()
        {
            if (textureObject != 0)
            {
                GL.DeleteTexture(textureObject);
                textureObject = GL.GenTexture();
            }
        }

        public override void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2DMultisample, textureObject);
        }

        protected override void Alloc()
        {
            GL.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, 4, textureFormat, m_Width, m_Height, false);
        }

        public override void Resize(int newWidth, int newHeight)
        {
            Debug.Assert(newWidth > 0 && newHeight > 0);

            RecreateTexture();
            m_Width = newWidth;
            m_Height = newHeight;
            Bind();
            
            GL.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, 4, textureFormat, m_Width, m_Height, false);
        }
    }
}
