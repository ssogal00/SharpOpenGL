using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Core.Buffer
{
    public class FrameBuffer : IDisposable, IBindable
    {
        static protected int FrameBufferCount = 0;

        public FrameBuffer()
        {
            frameBufferObject = GL.GenFramebuffer();
#if DEBUG
            DebugName = string.Format("FrameBuffer_{0}", FrameBufferCount++);
#endif
        }

        public FrameBuffer(string debugName)
            : this()
        {
#if DEBUG
            DebugName = debugName;
#endif
        }

        public int GetBufferHandle()
        {
            return frameBufferObject;
        }

        public void Dispose()
        {
            DeleteFrameBuffer();
        }

        public void Bind()
        {
#if DEBUG
            BindState.Get().SetBoundFrameBuffer(DebugName);
#endif
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBufferObject);
            bBind = true;
        }

        public void Unbind()
        {
            if (IsBind)
            {
#if DEBUG
                BindState.Get().SetBoundFrameBuffer("");
#endif
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

        protected string DebugName = "";
    }
}
