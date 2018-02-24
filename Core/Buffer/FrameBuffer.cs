using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Core.Buffer
{
    public class FrameBuffer
    {
        public FrameBuffer()
        {
            FrameBufferObject = GL.GenFramebuffer();
        }

        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FrameBufferObject);
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
            if(FrameBufferObject != 0)
            {
                GL.DeleteFramebuffer(FrameBufferObject);
            }
        }

        public bool IsBind
        {
            get
            {
                return bBind;
            }
        }


        protected int FrameBufferObject = 0;

        protected bool bBind = false;
    }
}
