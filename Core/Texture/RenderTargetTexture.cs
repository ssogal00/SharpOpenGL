using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Core.Texture
{
    public class RenderTargetTexture : TextureBase
    {
        public RenderTargetTexture(int width, int height)
            :base()
        {
            m_Width = width;
            m_Height = height;

            m_FrameBufferObject = GL.GenFramebuffer();

            
        }

        int m_FrameBufferObject = 0;
    }
}
