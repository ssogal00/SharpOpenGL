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

        public void Clear()
        {
            GL.ClearColor(1, 1, 1, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
        public void Unbind()
        {
            FrameBufferObject.Unbind();
        }

        public void OnWindowResize(object sender, Core.CustomEvent.ScreenResizeEventArgs e)
        {
            Resize(e.Width, e.Height);
        }

        public void PrepareToDraw()
        {
            GL.Viewport(0, 0, BufferWidth, BufferHeight);

            var attachments = new DrawBuffersEnum[]
            {
                DrawBuffersEnum.ColorAttachment0,
            };

            GL.DrawBuffers(1, attachments);
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
            ColorAttachment = new ColorAttachmentTexture(BufferWidth, BufferHeight);
            DepthAttachment = new DepthTargetTexture(BufferWidth, BufferHeight);

            Resize(BufferWidth, BufferHeight);
        }

        public int GetColorAttachmentTextureObject()
        {
            return ColorAttachment.GetTextureObject;
        }

        protected ColorAttachmentTexture ColorAttachment = null;        
        protected DepthTargetTexture DepthAttachment = null;

        protected Core.Buffer.FrameBuffer FrameBufferObject = null;

        protected int BufferWidth = 0;
        protected int BufferHeight = 0;
    }
}
