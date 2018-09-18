using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Buffer;
using OpenTK.Graphics.OpenGL;

namespace Core.Texture
{
    public class MultisampleRenderTarget : RenderResource, IBindable
    {
        public MultisampleRenderTarget(int width, int height, int attachmentCount)
        {
            BufferWidth = width;
            BufferHeight = height;
            AttachmentCount = attachmentCount;
        }

        public void Bind()
        {
            FrameBufferObject.Bind();
            PrepareToDraw();
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

        public virtual void PrepareToDraw()
        {
            Clear();

            GL.Viewport(0, 0, BufferWidth, BufferHeight);

            GL.DrawBuffers(1, AttchmentsEnums);
        }

        private void Resize(int newWidth, int newHeight)
        {
            Debug.Assert(newWidth > 0 && newHeight > 0);

            FrameBufferObject.Bind();

            BufferWidth = newWidth;
            BufferHeight = newHeight;

            ColorAttachment0.Resize(BufferWidth, BufferHeight);
            DepthAttachment.Resize(BufferWidth, BufferHeight);

            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2DMultisample, ColorAttachment0.GetTextureObject, 0);
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.DepthStencilAttachment, TextureTarget.Texture2DMultisample, DepthAttachment.GetTextureObject, 0);

            //
            if (AttachmentCount > 1)
            {
                ColorAttachment1.Resize(BufferWidth, BufferHeight);
                GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment1, TextureTarget.Texture2DMultisample, ColorAttachment1.GetTextureObject, 0);
            }

            //
            if (AttachmentCount > 2)
            {
                ColorAttachment2.Resize(BufferWidth, BufferHeight);
                GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment2, TextureTarget.Texture2DMultisample, ColorAttachment2.GetTextureObject, 0);
            }

            var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);

            Debug.Assert(status == FramebufferErrorCode.FramebufferComplete);

            FrameBufferObject.Unbind();
        }

        public override void Initialize()
        {
            FrameBufferObject = new FrameBuffer();
            ColorAttachment0 = new MultisampleColorAttachmentTexture(BufferWidth, BufferHeight);
            DepthAttachment = new MultisampleDepthTargetTexture(BufferWidth, BufferHeight);

            if (AttachmentCount > 1)
            {
                ColorAttachment1 = new MultisampleColorAttachmentTexture(BufferWidth, BufferHeight);
            }

            if (AttachmentCount > 2)
            {
                ColorAttachment2 = new MultisampleColorAttachmentTexture(BufferWidth, BufferHeight);
            }

            if (AttachmentCount == 1)
            {
                AttchmentsEnums = new DrawBuffersEnum[]
                {
                    DrawBuffersEnum.ColorAttachment0
                };
            }
            else if (AttachmentCount == 2)
            {
                AttchmentsEnums = new DrawBuffersEnum[]
                {
                    DrawBuffersEnum.ColorAttachment0,
                    DrawBuffersEnum.ColorAttachment1
                };
            }
            else if (AttachmentCount == 3)
            {
                AttchmentsEnums = new DrawBuffersEnum[]
                {
                    DrawBuffersEnum.ColorAttachment0,
                    DrawBuffersEnum.ColorAttachment1,
                    DrawBuffersEnum.ColorAttachment2,
                };
            }


            Resize(BufferWidth, BufferHeight);
        }

        public int GetColorAttachment0TextureObject()
        {
            return ColorAttachment0.GetTextureObject;
        }

        public MultisampleColorAttachmentTexture GetColorAttachment0Texture()
        {
            return ColorAttachment0;
        }

        public int GetColorAttachment1TextureObject()
        {
            return ColorAttachment1.GetTextureObject;
        }

        public MultisampleColorAttachmentTexture GetColorAttachment1Texture()
        {
            return ColorAttachment1;
        }

        public int GetColorAttachment2TextureObject()
        {
            return ColorAttachment2.GetTextureObject;
        }

        public MultisampleColorAttachmentTexture GetColorAttachment2Texture()
        {
            return ColorAttachment2;
        }

        public MultisampleDepthTargetTexture GetDepthTargetTexture()
        {
            return DepthAttachment;
        }

        // max 3 color attachment
        protected MultisampleColorAttachmentTexture ColorAttachment0 = null;
        protected MultisampleColorAttachmentTexture ColorAttachment1 = null;
        protected MultisampleColorAttachmentTexture ColorAttachment2 = null;

        // and 1 depth attachment
        protected MultisampleDepthTargetTexture DepthAttachment = null;

        protected Core.Buffer.FrameBuffer FrameBufferObject = null;

        DrawBuffersEnum[] AttchmentsEnums = null;

        public int RenderTargetWidth => BufferWidth;
        public int RenderTargetHeight => BufferHeight;

        protected int BufferWidth = 640;
        protected int BufferHeight = 480;

        protected int AttachmentCount = 1;
    }
}
