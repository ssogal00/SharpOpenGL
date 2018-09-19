using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Texture;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace Core.Buffer
{
    public class MultisampleGBuffer : GBuffer
    {
        protected override void CreateGBuffer()
        {
            FrameBufferObject = new FrameBuffer("GBuffer Framebuffer");
            FrameBufferObject.Bind();

            PositionAttachment = new MultisampleColorAttachmentTexture(BufferWidth, BufferHeight);
            PositionAttachment.Resize(BufferWidth, BufferHeight);

            ColorAttachment = new MultisampleColorAttachmentTexture(BufferWidth, BufferHeight);
            ColorAttachment.Resize(BufferWidth, BufferHeight);

            NormalAttachment = new MultisampleColorAttachmentTexture(BufferWidth, BufferHeight);
            NormalAttachment.Resize(BufferWidth, BufferHeight);

            DepthAttachment = new MultisampleDepthTargetTexture(BufferWidth, BufferHeight);
            DepthAttachment.Resize(BufferWidth, BufferHeight);

            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2DMultisample, PositionAttachment.GetTextureObject, 0);
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment1, TextureTarget.Texture2DMultisample, ColorAttachment.GetTextureObject, 0);
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment2, TextureTarget.Texture2DMultisample, NormalAttachment.GetTextureObject, 0);
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.DepthStencilAttachment, TextureTarget.Texture2DMultisample, DepthAttachment.GetTextureObject, 0);

            var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);

            Debug.Assert(status == FramebufferErrorCode.FramebufferComplete);

            FrameBufferObject.Unbind();
        }

        protected override void Resize(int newWidth, int newHeight)
        {
            Debug.Assert(newWidth > 0 && newHeight > 0);

            FrameBufferObject.Bind();

            BufferWidth = newWidth;
            BufferHeight = newHeight;

            PositionAttachment.Resize(BufferWidth, BufferHeight);
            ColorAttachment.Resize(BufferWidth, BufferHeight);
            NormalAttachment.Resize(BufferWidth, BufferHeight);
            DepthAttachment.Resize(BufferWidth, BufferHeight);

            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2DMultisample, PositionAttachment.GetTextureObject, 0);
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment1, TextureTarget.Texture2DMultisample, ColorAttachment.GetTextureObject, 0);
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment2, TextureTarget.Texture2DMultisample, NormalAttachment.GetTextureObject, 0);
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.DepthStencilAttachment, TextureTarget.Texture2DMultisample, DepthAttachment.GetTextureObject, 0);

            var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);

            Debug.Assert(status == FramebufferErrorCode.FramebufferComplete);

            FrameBufferObject.Unbind();
        }
    }
}
