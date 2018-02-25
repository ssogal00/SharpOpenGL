using OpenTK.Graphics.OpenGL;
using Core.Texture;
using System.Diagnostics;

namespace Core.Buffer
{
    public class GBuffer
    {        
        public GBuffer(int width, int height)
        {
            Height = height;
            Width = width;
            // 
            FrameBufferObject = new FrameBuffer();
            FrameBufferObject.Bind();

            PositionAttachment = new RenderTargetTexture(width, height);
            PositionAttachment.Bind();

            ColorAttachment = new RenderTargetTexture(width, height);
            ColorAttachment.Bind();

            NormalAttachment = new RenderTargetTexture(width, height);
            NormalAttachment.Bind();

            DepthAttachment = new DepthTargetTexture(width, height);
            DepthAttachment.Bind();

            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, PositionAttachment.GetTextureObject, 0);
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment1, TextureTarget.Texture2D, ColorAttachment.GetTextureObject, 0);
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment2, TextureTarget.Texture2D, NormalAttachment.GetTextureObject, 0);
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.DepthStencilAttachment, TextureTarget.Texture2D, DepthAttachment.GetTextureObject, 0);

            var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);

            Debug.Assert(status == FramebufferErrorCode.FramebufferComplete);

            FrameBufferObject.Unbind();            
        }

        public void Bind()
        {
            FrameBufferObject.Bind();
        }

        public void Unbind()
        {
            FrameBufferObject.Unbind();
        }

        public int ColorBufferObject => ColorAttachment.GetTextureObject;
        public int PositionBufferObject => PositionAttachment.GetTextureObject;
        public int NormalBufferObject => NormalAttachment.GetTextureObject;
        public int DepthBufferObject => DepthAttachment.GetTextureObject;

        public void PrepareToDraw()
        {
            GL.Viewport(0,0,Width, Height);
      
            var attachments = new DrawBuffersEnum[]
            {
                DrawBuffersEnum.ColorAttachment0,
                DrawBuffersEnum.ColorAttachment1,
                DrawBuffersEnum.ColorAttachment2,
            };
            
            GL.DrawBuffers(3, attachments);
        }

        protected void Clear()
        {
            GL.ClearColor(1, 1, 1, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        protected RenderTargetTexture PositionAttachment = null;
        protected RenderTargetTexture ColorAttachment = null;
        protected RenderTargetTexture NormalAttachment = null;
        protected DepthTargetTexture DepthAttachment = null;

        protected Core.Buffer.FrameBuffer FrameBufferObject = null;

        protected int Width = 0;
        protected int Height = 0;
    }
}
