using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using Core.Buffer;
using System.Diagnostics;

namespace Core.Texture
{
    public class RenderTargetTexture
    {
        public RenderTargetTexture(int width, int height)
            :base()
        {
            BufferWidth = width;
            BufferHeight = height;
        }

        public void Bind()
        {
            FrameBufferObject.Bind();
        }

        public void Unbind()
        {
            FrameBufferObject.Unbind();
        }

        public void OnWindowResize(object sender, Core.CustomEvent.ScreenResizeEventArgs e)
        {
            Resize(e.Width, e.Height);
        }

        private void Resize(int newWidth, int newHeight)
        {
            Debug.Assert(newWidth > 0 && newHeight > 0);

            FrameBufferObject.Bind();

            BufferWidth = newWidth;
            BufferHeight = newHeight;

            ColorAttachment.Resize(BufferWidth, BufferHeight);            
            DepthAttachment.Resize(BufferWidth, BufferHeight);
                        
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, ColorAttachment.GetTextureObject, 0);            
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.DepthStencilAttachment, TextureTarget.Texture2D, DepthAttachment.GetTextureObject, 0);

            var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);

            Debug.Assert(status == FramebufferErrorCode.FramebufferComplete);

            FrameBufferObject.Unbind();
        }

        public void Create()
        {
            FrameBufferObject = new FrameBuffer();

            Resize(BufferWidth, BufferHeight);
        }

        protected ColorAttachmentTexture ColorAttachment = null;        
        protected DepthTargetTexture DepthAttachment = null;

        protected Core.Buffer.FrameBuffer FrameBufferObject = null;

        protected int BufferWidth = 0;
        protected int BufferHeight = 0;
    }
}
