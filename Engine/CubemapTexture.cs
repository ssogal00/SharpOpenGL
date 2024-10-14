using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using FreeImageAPI;
using System.Runtime.Versioning;

namespace Core.Texture
{
    public class CubemapTexture : TextureBase
    {
        public CubemapTexture()
        : base()
        {
            
        }

        public override void Bind()
        {
            if (IsValid)
            {
                GL.BindTexture(TextureTarget.TextureCubeMap,textureObject);
            }
        }

        public override void BindShader(TextureUnit Unit, int SamplerLoc)
        {
            if (IsValid)
            {
                //Sampler.DefaultCubemapSampler.BindSampler(Unit);
                GL.Uniform1(SamplerLoc, (int)(Unit - TextureUnit.Texture0));
            }
        }
        [SupportedOSPlatform("windows")]
        public void Load()
        {
            Bind();

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);


            string [] textureList = new string[]
            {
                PositiveXImagePath, NegativeXImagePath,  NegativeYImagePath,PositiveYImagePath, PositiveZImagePath, NegativeZImagePath
            };

            for (int i = 0; i < textureList.Length; ++i)
            {
                using (var texture = new ScopedFreeImage(textureList[i]))
                {
                    GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba, texture.Width, texture.Height,0,
                        OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, texture.Bytes);
                }
            }
        }

        public void LoadFromTexture(
            TextureBase positiveX, TextureBase negativeX,
            TextureBase positiveY, TextureBase negativeY,
            TextureBase positiveZ, TextureBase negativeZ)
        {
            Bind();

            m_Width = positiveX.Width;
            m_Height = positiveY.Height;

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear); 
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
            
            
            // X
            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX, 0, PixelInternalFormat.Rgba, positiveX.Width, positiveX.Height, 0, PixelFormat.Rgba, PixelType.Float, positiveX.GetTexImageAsFloat());
            GL.TexImage2D(TextureTarget.TextureCubeMapNegativeX, 0, PixelInternalFormat.Rgba, negativeX.Width, negativeX.Height, 0, PixelFormat.Rgba, PixelType.Float, negativeX.GetTexImageAsFloat());

            // Y
            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveY, 0, PixelInternalFormat.Rgba, positiveY.Width, positiveY.Height, 0, PixelFormat.Rgba, PixelType.Float, positiveY.GetTexImageAsFloat());
            GL.TexImage2D(TextureTarget.TextureCubeMapNegativeY, 0, PixelInternalFormat.Rgba, negativeY.Width, negativeY.Height, 0, PixelFormat.Rgba, PixelType.Float, negativeY.GetTexImageAsFloat());

            // Z
            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveZ, 0, PixelInternalFormat.Rgba, positiveZ.Width, positiveZ.Height, 0, PixelFormat.Rgba, PixelType.Float, positiveZ.GetTexImageAsFloat());
            GL.TexImage2D(TextureTarget.TextureCubeMapNegativeZ, 0, PixelInternalFormat.Rgba, negativeZ.Width, negativeZ.Height, 0, PixelFormat.Rgba, PixelType.Float, negativeZ.GetTexImageAsFloat());
        }

        protected TextureBase NegativeXTex = null;
        protected TextureBase PositiveXTex = null;

        protected TextureBase NegativeYTex = null;
        protected TextureBase PositiveYTex = null;

        protected TextureBase NegativeZTex = null;
        protected TextureBase PositiveZTex = null;


        protected string NegativeXImagePath = "./Resources/Cubemap/SKYBOX_xneg.png";
        protected string PositiveXImagePath = "./Resources/Cubemap/SKYBOX_xpos.png";
        protected string NegativeYImagePath = "./Resources/Cubemap/SKYBOX_yneg.png";
        protected string PositiveYImagePath = "./Resources/Cubemap/SKYBOX_ypos.png";
        protected string NegativeZImagePath = "./Resources/Cubemap/SKYBOX_zneg.png";
        protected string PositiveZImagePath = "./Resources/Cubemap/SKYBOX_zpos.png";
    }
}
