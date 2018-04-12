using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Core.Buffer
{
    public class FrameBuffer : IDisposable
    {
        public FrameBuffer()
        {
            frameBufferObject = GL.GenFramebuffer();
        }

        public void Dispose()
        {
            DeleteFrameBuffer();
        }

        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBufferObject);
            bBind = true;
        }

        public void Unbind()
        {
            if (IsBind)
            {
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
                bBind = false;
            }
        }

        public void DeleteFrameBuffer()
        {
            if(frameBufferObject != 0)
            {
                Unbind();
                GL.DeleteFramebuffer(frameBufferObject);
                frameBufferObject = 0;
            }
        }

        public bool IsBind
        {
            get
            {
                return bBind;
            }
        }


        protected int frameBufferObject = 0;

        protected bool bBind = false;
    }
}
