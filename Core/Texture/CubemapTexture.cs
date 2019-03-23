using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using FreeImageAPI;

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
        
        public void Load()
        {
            Bind();

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
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

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
            
            
            
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
