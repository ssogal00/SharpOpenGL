using Core.Buffer;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Configuration;
using System.Runtime.Remoting;
using System.Windows.Forms;

namespace Core.Texture
{
    public class RenderTarget : RenderResource, IBindable, IResizable
    {
        public RenderTarget(int width, int height, int attachmentCount)
        {
            ResizableManager.Get().AddResizable(this);
            BufferWidth = width;
            BufferHeight = height;
            AttachmentCount = attachmentCount;
        }

        public RenderTarget(int width, int height, int attachmentCount, PixelInternalFormat internalFormat, bool includeDepthAttachment )
        : this(width, height, attachmentCount)
        {
            PixelFormat = internalFormat;
            bIncludeDepthAttachment = includeDepthAttachment;
        }

        public void Bind()
        {
            FrameBufferObject.Bind();
            PrepareToDraw();
        }

        public void Clear()
        {
            GL.ClearColor(ClearColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
        
        public void Unbind()
        {
            FrameBufferObject.Unbind();
        }

        public void OnResize(object sender, Core.CustomEvent.ScreenResizeEventArgs e)
        {
            Resize(e.Width, e.Height);
        }
        
        public virtual void PrepareToDraw()
        {
            Clear();

            GL.Viewport(0, 0, BufferWidth, BufferHeight);            

            GL.DrawBuffers(1, AttchmentsEnums);
        }

        public virtual void Copy(RenderTarget target)
        {
            FrameBufferObject.Bind();
            
            // 
            GL.FramebufferTexture2D(FramebufferTarget.ReadFramebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, ColorAttachment0.GetTextureObject, 0);
            GL.ReadBuffer(ReadBufferMode.ColorAttachment0);

            //
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment1, TextureTarget.Texture2D, target.GetColorAttachment0TextureObject(), 0);
            GL.DrawBuffer(DrawBufferMode.ColorAttachment1);

            GL.BlitFramebuffer(0, 0, this.BufferWidth, this.BufferHeight, 
                               0, 0, target.BufferWidth, target.BufferHeight, 
                               ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Linear);

            FrameBufferObject.Unbind();
        }

        public virtual void Copy(ColorAttachmentTexture target)
        {
            FrameBufferObject.Bind();

            // 
            GL.FramebufferTexture2D(FramebufferTarget.ReadFramebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, ColorAttachment0.GetTextureObject, 0);
            GL.ReadBuffer(ReadBufferMode.ColorAttachment0);

            //
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment1, TextureTarget.Texture2D, target.TextureObject, 0);
            GL.DrawBuffer(DrawBufferMode.ColorAttachment1);

            GL.BlitFramebuffer(0, 0, this.BufferWidth, this.BufferHeight,
                               0, 0, target.Width, target.Height,
                               ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Linear);

            FrameBufferObject.Unbind();
        }

        protected virtual void Resize(int newWidth, int newHeight)
        {
            Debug.Assert(newWidth > 0 && newHeight > 0);

            FrameBufferObject.Bind();

            BufferWidth = newWidth;
            BufferHeight = newHeight;

            ColorAttachment0.Resize(BufferWidth, BufferHeight);

            if (bIncludeDepthAttachment)
            {
                DepthAttachment.Resize(BufferWidth, BufferHeight);
            }

            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, ColorAttachment0.GetTextureObject, 0);

            if (bIncludeDepthAttachment)
            {
                GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.DepthStencilAttachment, TextureTarget.Texture2D, DepthAttachment.GetTextureObject, 0);
            }

            //
            if (AttachmentCount > 1)
            {
                ColorAttachment1.Resize(BufferWidth, BufferHeight);
                GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment1, TextureTarget.Texture2D, ColorAttachment1.GetTextureObject, 0);
            }

            //
            if(AttachmentCount > 2)
            {
                ColorAttachment2.Resize(BufferWidth, BufferHeight);
                GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment2, TextureTarget.Texture2D, ColorAttachment2.GetTextureObject, 0);
            }

            var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);

            Debug.Assert(status == FramebufferErrorCode.FramebufferComplete);

            FrameBufferObject.Unbind();
        }

        public override void Initialize()
        {
            FrameBufferObject = new FrameBuffer();

            ColorAttachment0 = new ColorAttachmentTexture(BufferWidth, BufferHeight, PixelFormat);

            if (bIncludeDepthAttachment)
            {
                DepthAttachment = new DepthTargetTexture(BufferWidth, BufferHeight);
            }

            if(AttachmentCount > 1)
            {
                ColorAttachment1 = new ColorAttachmentTexture(BufferWidth, BufferHeight);
            }

            if(AttachmentCount > 2)
            {
                ColorAttachment2 = new ColorAttachmentTexture(BufferWidth, BufferHeight);
            }
            
            if(AttachmentCount == 1)
            {
                AttchmentsEnums = new DrawBuffersEnum [] 
                {
                    DrawBuffersEnum.ColorAttachment0
                };
            }
            else if(AttachmentCount == 2)
            {
                AttchmentsEnums = new DrawBuffersEnum[] 
                {
                    DrawBuffersEnum.ColorAttachment0,
                    DrawBuffersEnum.ColorAttachment1
                };
            }
            else if(AttachmentCount == 3)
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

        public ColorAttachmentTexture GetColorAttachment0Texture()
        {
            return ColorAttachment0;
        }

        public int GetColorAttachment1TextureObject()
        {
            return ColorAttachment1.GetTextureObject;
        }

        public ColorAttachmentTexture GetColorAttachment1Texture()
        {
            return ColorAttachment1;
        }

        public int GetColorAttachment2TextureObject()
        {
            return ColorAttachment2.GetTextureObject;
        }

        public ColorAttachmentTexture GetColorAttachment2Texture()
        {
            return ColorAttachment2;
        }

        public DepthTargetTexture GetDepthTargetTexture()
        {
            return DepthAttachment;
        }

        // max 3 color attachment
        protected ColorAttachmentTexture ColorAttachment0 = null;
        protected ColorAttachmentTexture ColorAttachment1 = null;
        protected ColorAttachmentTexture ColorAttachment2 = null;

        // and 1 depth attachment
        protected DepthTargetTexture DepthAttachment = null;

        private PixelInternalFormat PixelFormat = PixelInternalFormat.Rgba16f;

        protected Core.Buffer.FrameBuffer FrameBufferObject = null;

        protected DrawBuffersEnum[] AttchmentsEnums = null;

        public int RenderTargetWidth => BufferWidth;
        public int RenderTargetHeight => BufferHeight;

        private bool bIncludeDepthAttachment = true;

        protected int BufferWidth = 640;
        protected int BufferHeight = 480;
        
        protected int AttachmentCount = 1;

        public System.Drawing.Color ClearColor = Color.White;
    }
}
