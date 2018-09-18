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
    public class MultisampleRenderTarget : RenderTarget
    {
        public MultisampleRenderTarget(int width, int height, int attachmentCount)
        : base(width, height, attachmentCount)
        {
        }

        protected override void Resize(int newWidth, int newHeight)
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
    }
}
