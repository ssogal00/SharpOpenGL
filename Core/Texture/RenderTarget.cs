using Core.Buffer;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Configuration;
using System.Runtime.Remoting;
using System.Windows.Forms;
using OpenTK;

namespace Core.Texture
{
    public class RenderTarget : RenderResource, IBindable, IResizable
    {

        public RenderTarget(int width, int height, int attachementCount,  bool bFixedSize = false, float customScale = 1.0f, bool bIncludeDepthAttachment = true)
        {
            if (bFixedSize == false)
            {
                ResizableManager.Get().AddResizable(this);
            }

            this.bIncludeDepthAttachment = bIncludeDepthAttachment;
            CustomScale = customScale;
            BufferWidth = (int) ((width) * customScale);
            BufferHeight = (int) ((height) * customScale);

            this.bFixedSize = bFixedSize;

            AttachmentCount = attachementCount;
        }

        public RenderTarget(int width, int height, int attachmentCount, PixelInternalFormat internalFormat, PixelFormat renderTargetPixelFormat, bool bFixedSize = false, float customScale = 1.0f, bool includeDepthAttachment = true)
        : this(width, height, attachmentCount, bFixedSize, customScale )
        {
            RenderTargetPixelInternalFormat = internalFormat;
            RenderTargetPixelFormat = renderTargetPixelFormat;
            bIncludeDepthAttachment = includeDepthAttachment;
            bFixedSize = false;
        }

        public override void ReleaseResource()
        {
            if (ColorAttachment0 != null)
            {
                ColorAttachment0.Dispose();
            }

            if (ColorAttachment1 != null)
            {
                ColorAttachment1.Dispose();
            }

            if (ColorAttachment2 != null)
            {
                ColorAttachment2.Dispose();
            }

            if (DepthAttachment != null)
            {
                DepthAttachment.Dispose();
            }
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
            GL.FramebufferTexture2D(FramebufferTarget.ReadFramebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, colorAttachment0.GetTextureObject, 0);
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
            using (var dummy = new ScopedFrameBufferBound(FrameBufferObject))
            {
                // 
                GL.FramebufferTexture2D(FramebufferTarget.ReadFramebuffer, FramebufferAttachment.ColorAttachment0,
                    TextureTarget.Texture2D, colorAttachment0.GetTextureObject, 0);
                GL.ReadBuffer(ReadBufferMode.ColorAttachment0);

                //
                GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment1,
                    TextureTarget.Texture2D, target.TextureObject, 0);
                GL.DrawBuffer(DrawBufferMode.ColorAttachment1);

                GL.BlitFramebuffer(0, 0, this.BufferWidth, this.BufferHeight,
                    0, 0, target.Width, target.Height,
                    ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Linear);
            }
        }

        public virtual void Resize(int newWidth, int newHeight)
        {
            Debug.Assert(newWidth > 0 && newHeight > 0);

            using (var dummy = new ScopedFrameBufferBound(FrameBufferObject))
            {
                if (bFixedSize == false)
                {
                    BufferWidth = (int)((newWidth) * CustomScale);
                    BufferHeight = (int)((newHeight) * CustomScale);
                }
                
                Debug.Assert(BufferHeight > 0 && BufferWidth > 0);

                colorAttachment0.Resize(BufferWidth, BufferHeight);

                if (bIncludeDepthAttachment)
                {
                    DepthAttachment.Resize(BufferWidth, BufferHeight);
                }

                GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, colorAttachment0.GetTextureObject, 0);

                if (bIncludeDepthAttachment)
                {
                    GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, DepthAttachment.GetTextureObject, 0);
                }

                //
                if (AttachmentCount > 1)
                {
                    colorAttachment1.Resize(BufferWidth, BufferHeight);
                    GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment1, TextureTarget.Texture2D, colorAttachment1.GetTextureObject, 0);
                }

                //
                if (AttachmentCount > 2)
                {
                    colorAttachment2.Resize(BufferWidth, BufferHeight);
                    GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment2, TextureTarget.Texture2D, colorAttachment2.GetTextureObject, 0);
                }

                var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);

                Debug.Assert(status == FramebufferErrorCode.FramebufferComplete);
            }
        }

        public override void Initialize()
        {
            FrameBufferObject = new FrameBuffer();

            colorAttachment0 = new ColorAttachmentTexture(BufferWidth, BufferHeight, RenderTargetPixelInternalFormat, RenderTargetPixelFormat, RenderTargetPixelDataType);

            if (bIncludeDepthAttachment)
            {
                DepthAttachment = new DepthTargetTexture(BufferWidth, BufferHeight);
            }

            if(AttachmentCount > 1)
            {
                colorAttachment1 = new ColorAttachmentTexture(BufferWidth, BufferHeight, RenderTargetPixelInternalFormat, RenderTargetPixelFormat, RenderTargetPixelDataType);
            }

            if(AttachmentCount > 2)
            {
                colorAttachment2 = new ColorAttachmentTexture(BufferWidth, BufferHeight, RenderTargetPixelInternalFormat, RenderTargetPixelFormat, RenderTargetPixelDataType);
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
            return colorAttachment0.GetTextureObject;
        }

        public ColorAttachmentTexture ColorAttachment0 => colorAttachment0;
        
        public int GetColorAttachment1TextureObject()
        {
            return colorAttachment1.GetTextureObject;
        }

        public ColorAttachmentTexture ColorAttachment1 => colorAttachment1;

        public int GetColorAttachment2TextureObject()
        {
            return colorAttachment2.GetTextureObject;
        }

        public ColorAttachmentTexture ColorAttachment2 => colorAttachment2;

        public DepthTargetTexture GetDepthTargetTexture()
        {
            return DepthAttachment;
        }

        // max 3 color attachment
        protected ColorAttachmentTexture colorAttachment0 = null;
        protected ColorAttachmentTexture colorAttachment1 = null;
        protected ColorAttachmentTexture colorAttachment2 = null;

        // and 1 depth attachment
        protected DepthTargetTexture DepthAttachment = null;

        private PixelInternalFormat RenderTargetPixelInternalFormat = PixelInternalFormat.Rgba16f;
        private PixelFormat RenderTargetPixelFormat = PixelFormat.Rgba;
        private PixelType RenderTargetPixelDataType = PixelType.Float;

        protected Core.Buffer.FrameBuffer FrameBufferObject = null;

        protected DrawBuffersEnum[] AttchmentsEnums = null;

        public int RenderTargetWidth => BufferWidth;
        public int RenderTargetHeight => BufferHeight;

        public OpenTK.Vector2 RenderTargetSize => new Vector2(RenderTargetWidth, RenderTargetHeight);
        public OpenTK.Vector2 InverseRenderTargetSize => new Vector2(1.0f / (float) RenderTargetWidth, 1.0f / (float)RenderTargetHeight);

        private bool bIncludeDepthAttachment = true;

        protected int BufferWidth = 640;
        protected int BufferHeight = 480;
        protected float CustomScale = 1.0f;
        
        protected int AttachmentCount = 1;

        public System.Drawing.Color ClearColor = Color.White;

        public float Scale { get; set; } = 1.0f;

        protected bool bFixedSize = false;

        public string RenderTargetName { get; set; } = "";
    }
}
