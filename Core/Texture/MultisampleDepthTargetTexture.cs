using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;

namespace Core.Texture
{
    public class MultisampleDepthTargetTexture : DepthTargetTexture
    {
        public MultisampleDepthTargetTexture(int widthParam, int heightParam)
            : base(widthParam, heightParam)
        {
        }

        public override void Bind()
        {
            if (IsValid)
            {
                GL.BindTexture(TextureTarget.Texture2DMultisample, textureObject);
            }
        }

        public override void Resize(int newWidth, int newHeight)
        {
            Debug.Assert(newWidth > 0 && newHeight > 0);

            RecreateTexture();
            m_Width = newWidth;
            m_Height = newHeight;
            Bind();
            Alloc();
        }

        protected override void Alloc()
        {   
            GL.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, 4, PixelInternalFormat.Depth24Stencil8, m_Width, m_Height, false);
        }

        protected int SampleCount = 4;
    }
}
