﻿using OpenTK.Graphics.OpenGL;
using Core.Texture;
using System.Diagnostics;
using System;
using Core.CustomEvent;
using System.Runtime.Versioning;

namespace Core.Buffer
{
    public class GBuffer : RenderingThreadObject, IBindable, IResizable
    {        
        public GBuffer(int width, int height)
        {
            ResizableManager.Instance.AddResizable(this);
            BufferHeight = height;
            BufferWidth = width;

            Initialize();
        }

        public GBuffer()
        {
            ResizableManager.Instance.AddResizable(this);
            BufferWidth = 1024;
            BufferHeight = 768;
            Initialize();
        }

        protected virtual void CreateGBuffer()
        {
            // 
            FrameBufferObject = new FrameBuffer("GBuffer Framebuffer");
            FrameBufferObject.Bind();

            PositionAttachment = new ColorAttachmentTexture(BufferWidth, BufferHeight, PixelInternalFormat.Rgba16f, PixelFormat.Rgba, PixelType.Float);
            PositionAttachment.Resize(BufferWidth, BufferHeight);

            ColorAttachment = new ColorAttachmentTexture(BufferWidth, BufferHeight, PixelInternalFormat.Rgba16f, PixelFormat.Rgba, PixelType.Float);
            ColorAttachment.Resize(BufferWidth, BufferHeight);

            NormalAttachment = new ColorAttachmentTexture(BufferWidth, BufferHeight, PixelInternalFormat.Rgba16f, PixelFormat.Rgba, PixelType.Float);
            NormalAttachment.Resize(BufferWidth, BufferHeight);

            MotionAttachment = new ColorAttachmentTexture(BufferWidth, BufferHeight, PixelInternalFormat.Rgba16f, PixelFormat.Rgba, PixelType.Float);
            MotionAttachment.Resize(BufferWidth, BufferHeight);

            DepthAttachment = new DepthTargetTexture(BufferWidth, BufferHeight);
            DepthAttachment.Resize(BufferWidth, BufferHeight);

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, PositionAttachment.GetTextureObject, 0);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment1, TextureTarget.Texture2D, ColorAttachment.GetTextureObject, 0);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment2, TextureTarget.Texture2D, NormalAttachment.GetTextureObject, 0);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment3, TextureTarget.Texture2D, MotionAttachment.GetTextureObject, 0);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, DepthAttachment.GetTextureObject, 0);

            var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);

            Debug.Assert(status == FramebufferErrorCode.FramebufferComplete);

            FrameBufferObject.Unbind();
        }

        public override void Initialize()
        {
            base.Initialize();
            CreateGBuffer();
        }

        public void OnResize(int width, int height)
        {
            Resize(width, height);
        }

        public void Bind()
        {
            FrameBufferObject.Bind();
            PrepareToDraw();
        }

        public void Clear()
        {
            GL.ClearColor(1, 0, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void Clear(System.Drawing.Color clearColor)
        {
            GL.ClearColor(clearColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void Unbind()
        {
            FrameBufferObject.Unbind();
        }

        public int ColorBufferObject => ColorAttachment.GetTextureObject;
        public int PositionBufferObject => PositionAttachment.GetTextureObject;
        public int NormalBufferObject => NormalAttachment.GetTextureObject;
        public int DepthBufferObject => DepthAttachment.GetTextureObject;
        public int MotionBufferObject => MotionAttachment.GetTextureObject;

        public void PrepareToDraw()
        {
            GL.Viewport(0,0,BufferWidth, BufferHeight);
      
            var attachments = new DrawBuffersEnum[]
            {
                DrawBuffersEnum.ColorAttachment0,
                DrawBuffersEnum.ColorAttachment1,
                DrawBuffersEnum.ColorAttachment2,
                DrawBuffersEnum.ColorAttachment3,
            };
            
            GL.DrawBuffers(4, attachments);
        }
        [SupportedOSPlatform("windows")]
        public void SaveColorAttachmentAsBmp(string filename)
        {
            ColorAttachment.SaveAsBmp(filename);
        }
        [SupportedOSPlatform("windows")]
        public void SaveNormalAttachmentAsBmp(string filename)
        {
            NormalAttachment.SaveAsBmp(filename);
        }

        protected virtual void Resize(int newWidth, int newHeight)
        {
            Debug.Assert(newWidth > 0 && newHeight > 0);

            using (var dummy = new ScopedFrameBufferBound(FrameBufferObject))
            {
                BufferWidth = newWidth;
                BufferHeight = newHeight;

                PositionAttachment.Resize(BufferWidth, BufferHeight);
                ColorAttachment.Resize(BufferWidth, BufferHeight);
                NormalAttachment.Resize(BufferWidth, BufferHeight);
                MotionAttachment.Resize(BufferWidth, BufferHeight);
                DepthAttachment.Resize(BufferWidth, BufferHeight);

                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, PositionAttachment.GetTextureObject, 0);
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment1, TextureTarget.Texture2D, ColorAttachment.GetTextureObject, 0);
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment2, TextureTarget.Texture2D, NormalAttachment.GetTextureObject, 0);
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment3, TextureTarget.Texture2D, MotionAttachment.GetTextureObject, 0);
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, DepthAttachment.GetTextureObject, 0);
                //GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, TextureTarget.Texture2D, DepthAttachment.GetTextureObject, 0);

                var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);

                Debug.Assert(status == FramebufferErrorCode.FramebufferComplete);
            }
        }

        public ColorAttachmentTexture GetColorAttachement
        {
            get { return ColorAttachment; }
        }

        public ColorAttachmentTexture GetPositionAttachment
        {
            get { return PositionAttachment; }
        }

        public ColorAttachmentTexture GetNormalAttachment
        {
            get { return NormalAttachment; }
        }

        public ColorAttachmentTexture GetMotionAttachment
        {
            get { return MotionAttachment; }
        }

        public DepthTargetTexture GetDepthAttachment
        {
            get { return DepthAttachment; }
        }

        protected ColorAttachmentTexture PositionAttachment = null;
        protected ColorAttachmentTexture ColorAttachment = null;
        protected ColorAttachmentTexture NormalAttachment = null;
        protected ColorAttachmentTexture MotionAttachment = null;
        protected DepthTargetTexture DepthAttachment = null;

        protected Core.Buffer.FrameBuffer FrameBufferObject = null;

        protected int BufferWidth = 0;
        protected int BufferHeight = 0;
    }
}
