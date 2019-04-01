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

        public void Resize(int newWidth, int newHeight)
        {
            m_Width = newWidth;
            m_Height = newHeight;
        }

        public void BindFaceForRendering(TextureTarget targetFace, int mip)
        {
            System.Drawing.Color[] Colors =
            {
                Color.Red, Color.Blue, Color.Green,
                Color.Yellow, Color.Brown, Color.Black
            };

            if (IsValidTextureTarget(targetFace))
            {
                renderBuffer.AllocStorage(RenderbufferStorage.DepthComponent24, Width, Height);

                frameBuffer.Bind();
                GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, frameBuffer.GetBufferHandle());
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, targetFace, textureObject, mip);
                
                var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
                Debug.Assert(status == FramebufferErrorCode.FramebufferComplete);

                GL.Viewport(0, 0, Width, Height);
                GL.ClearColor(Colors[mip]);
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

        public float[] GetCubemapTexImageAsFloat(TextureTarget targetFace, int mip, int width, int height)
        {
            Bind();

            float[] data = null;

            if (CubemapPixelFormat == PixelFormat.Rgb)
            {
                data = new float[width * width * 3];
            }
            else if (CubemapPixelFormat == PixelFormat.Rgba)
            {
                data = new float[width * width * 4];
            }

            GL.GetTexImage<float>(targetFace, mip, CubemapPixelFormat, CubemapPixelType, data);

            Unbind();

            return data;
        }

        public void Save(int mip)
        {
            {

                {
                    var colorDataX = GetCubemapTexImageAsByte(TextureTarget.TextureCubeMapPositiveX, mip, 512,512);
                    FreeImageHelper.SaveAsBmp(ref colorDataX, 512, 512, "PrefilterDebug_X.bmp");

                }
            }
        }

        public byte[] GetCubemapTexImageAsByte(TextureTarget targetFace, int mip, int width, int height)
        {
            Bind();

            byte[] data = null;

            if (CubemapPixelFormat == PixelFormat.Rgb)
            {
                data = new byte[width * height * 3];
            }
            else if (CubemapPixelFormat == PixelFormat.Rgba)
            {
                data = new byte[width * height * 4];
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
            renderBuffer.AllocStorage(RenderbufferStorage.DepthComponent24, Width, Height);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, frameBuffer.GetBufferHandle());

            frameBuffer.Unbind();
            renderBuffer.Unbind();

            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX, 0, CubemapPixelInternalFormat, Width, Height, 0, CubemapPixelFormat, CubemapPixelType, new IntPtr(0));
            GL.TexImage2D(TextureTarget.TextureCubeMapNegativeX, 0, CubemapPixelInternalFormat, Width, Height, 0, CubemapPixelFormat, CubemapPixelType, new IntPtr(0));

            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveY, 0, CubemapPixelInternalFormat, Width, Height, 0, CubemapPixelFormat, CubemapPixelType, new IntPtr(0));
            GL.TexImage2D(TextureTarget.TextureCubeMapNegativeY, 0, CubemapPixelInternalFormat, Width, Height, 0, CubemapPixelFormat, CubemapPixelType, new IntPtr(0));

            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveZ, 0, CubemapPixelInternalFormat, Width, Height, 0, CubemapPixelFormat, CubemapPixelType, new IntPtr(0));
            GL.TexImage2D(TextureTarget.TextureCubeMapNegativeZ, 0, CubemapPixelInternalFormat, Width, Height, 0, CubemapPixelFormat, CubemapPixelType, new IntPtr(0));
            
            if (bGenerateMips)
            {
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                //GL.GenerateMipmap(GenerateMipmapTarget.TextureCubeMap);
                GL.GenerateTextureMipmap(textureObject);
            }

            Unbind();
        }

        protected bool bGenerateMips = false;
        private PixelType CubemapPixelType = PixelType.Float;
        private PixelFormat CubemapPixelFormat = PixelFormat.Rgb;
        private PixelInternalFormat CubemapPixelInternalFormat = PixelInternalFormat.Rgb16f;

        private FrameBuffer frameBuffer = null;
        private RenderBuffer renderBuffer = null;
    }
}
