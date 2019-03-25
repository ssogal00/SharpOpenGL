using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Core.Buffer;

namespace Core.Texture
{
    public class CubemapRenderTarget : TextureBase
    {
        public CubemapRenderTarget(int width, int height, bool bGenerateMips)
        {
            Debug.Assert(width > 0 && height > 0);
            m_Width = width;
            m_Height = height;
            this.bGenerateMips = bGenerateMips;

            Initialize();
        }

        public void BindForRendering()
        {
            frameBuffer.Bind();
            renderBuffer.Bind();
        }

        public void UnbindForRendering()
        {
            frameBuffer.Unbind();
            renderBuffer.Unbind();
        }

        public override void Bind()
        {
            if (IsValid)
            {
                GL.BindTexture(TextureTarget.TextureCubeMap, textureObject);
            }
        }

        public void BindFaceForRendering(TextureTarget targetFace, int mip = 0)
        {
            if (IsValidTextureTarget(targetFace))
            {
                renderBuffer.AllocStorage(RenderbufferStorage.Depth24Stencil8, Width, Height);
                GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, frameBuffer.GetBufferHandle());
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, targetFace, textureObject, mip);
                
                var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);

                GL.Viewport(0, 0, Width, Height);
                GL.ClearColor(Color.BlueViolet);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            }
        }

        private bool IsValidTextureTarget(TextureTarget targetFace)
        {
            if (targetFace == TextureTarget.TextureCubeMapPositiveX ||
                targetFace == TextureTarget.TextureCubeMapNegativeX ||
                targetFace == TextureTarget.TextureCubeMapPositiveY ||
                targetFace == TextureTarget.TextureCubeMapNegativeY ||
                targetFace == TextureTarget.TextureCubeMapPositiveZ ||
                targetFace == TextureTarget.TextureCubeMapNegativeZ)
            {
                return true;
            }

            return false;
        }

        public override void Unbind()
        {   
            GL.BindTexture(TextureTarget.TextureCubeMap, 0);
        }

        public float[] GetCubemapTexImageAsFloat(TextureTarget targetFace, int mip)
        {
            Bind();

            float[] data = null;

            if (CubemapPixelFormat == PixelFormat.Rgb)
            {
                data = new float[Width * Height * 3];
            }
            else if (CubemapPixelFormat == PixelFormat.Rgba)
            {
                data = new float[Width * Height * 4];
            }

            GL.GetTexImage<float>(targetFace, mip, CubemapPixelFormat, CubemapPixelType, data);

            Unbind();

            return data;
        }

        public byte[] GetCubemapTexImageAsByte(TextureTarget targetFace, int mip)
        {
            Bind();

            byte[] data = null;

            if (CubemapPixelFormat == PixelFormat.Rgb)
            {
                data = new byte[Width * Height * 3];
            }
            else if (CubemapPixelFormat == PixelFormat.Rgba)
            {
                data = new byte[Width * Height * 4];
            }

            GL.GetTexImage<byte>(targetFace, mip, CubemapPixelFormat, CubemapPixelType, data);

            Unbind();

            return data;
        }

        public void Initialize()
        {
            Bind();

            frameBuffer = new FrameBuffer();
            renderBuffer = new RenderBuffer();
            
            frameBuffer.Bind();
            renderBuffer.Bind();
            renderBuffer.AllocStorage(RenderbufferStorage.Depth24Stencil8, Width, Height);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, frameBuffer.GetBufferHandle());

            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX, 0, CubemapPixelInternalFormat, Width, Height, 0, CubemapPixelFormat, CubemapPixelType, new IntPtr(0));
            GL.TexImage2D(TextureTarget.TextureCubeMapNegativeX, 0, CubemapPixelInternalFormat, Width, Height, 0, CubemapPixelFormat, CubemapPixelType, new IntPtr(0));

            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveY, 0, CubemapPixelInternalFormat, Width, Height, 0, CubemapPixelFormat, CubemapPixelType, new IntPtr(0));
            GL.TexImage2D(TextureTarget.TextureCubeMapNegativeY, 0, CubemapPixelInternalFormat, Width, Height, 0, CubemapPixelFormat, CubemapPixelType, new IntPtr(0));

            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveZ, 0, CubemapPixelInternalFormat, Width, Height, 0, CubemapPixelFormat, CubemapPixelType, new IntPtr(0));
            GL.TexImage2D(TextureTarget.TextureCubeMapNegativeZ, 0, CubemapPixelInternalFormat, Width, Height, 0, CubemapPixelFormat, CubemapPixelType, new IntPtr(0));
            
            if (bGenerateMips)
            {
                GL.GenerateMipmap(GenerateMipmapTarget.TextureCubeMap);
            }

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.TextureCubeMapPositiveX, textureObject, 0);
            var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);

            Unbind();
        }

        protected bool bGenerateMips = false;
        private PixelType CubemapPixelType = PixelType.Float;
        private PixelFormat CubemapPixelFormat = PixelFormat.Rgba;
        private PixelInternalFormat CubemapPixelInternalFormat = PixelInternalFormat.Rgba16f;

        private FrameBuffer frameBuffer = null;
        private RenderBuffer renderBuffer = null;
    }
}
