using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace Core.Texture
{
    public class CubemapRenderTarget : TextureBase
    {
        public CubemapRenderTarget(int width, int height, bool bGenerateMips)
        {
            this.Width = width;
            this.Height = height;
            this.bGenerateMips = bGenerateMips;
        }

        public override void Bind()
        {
            if (IsValid)
            {
                GL.BindTexture(TextureTarget.TextureCubeMap, textureObject);
            }
        }

        public override void Unbind()
        {   
            GL.BindTexture(TextureTarget.TextureCubeMap, 0);
        }

        public void Initialize()
        {
            Bind();

            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX, 0, CubemapPixelInternalFormat, Width, Height, 0, CubemapPixelFormat, CubemapPixelType, new IntPtr());
            GL.TexImage2D(TextureTarget.TextureCubeMapNegativeX, 0, CubemapPixelInternalFormat, Width, Height, 0, CubemapPixelFormat, CubemapPixelType, new IntPtr());

            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveY, 0, CubemapPixelInternalFormat, Width, Height, 0, CubemapPixelFormat, CubemapPixelType, new IntPtr());
            GL.TexImage2D(TextureTarget.TextureCubeMapNegativeY, 0, CubemapPixelInternalFormat, Width, Height, 0, CubemapPixelFormat, CubemapPixelType, new IntPtr());

            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveZ, 0, CubemapPixelInternalFormat, Width, Height, 0, CubemapPixelFormat, CubemapPixelType, new IntPtr());
            GL.TexImage2D(TextureTarget.TextureCubeMapNegativeZ, 0, CubemapPixelInternalFormat, Width, Height, 0, CubemapPixelFormat, CubemapPixelType, new IntPtr());
            
            if (bGenerateMips)
            {
                GL.GenerateMipmap(GenerateMipmapTarget.TextureCubeMap);
            }

            Unbind();
        }

        protected int Width = 0;
        protected int Height = 0;

        protected bool bGenerateMips = false;
        private PixelType CubemapPixelType = PixelType.Float;
        private PixelFormat CubemapPixelFormat = PixelFormat.Rgb;
        private PixelInternalFormat CubemapPixelInternalFormat = PixelInternalFormat.Rgb;
    }
}
