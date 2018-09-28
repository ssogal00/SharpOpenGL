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
                PositiveX, NegativeX,  NegativeY,PositiveY, PositiveZ, NegativeZ
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

        protected string NegativeX = "./Resources/Cubemap/SKYBOX_xneg.png";
        protected string PositiveX = "./Resources/Cubemap/SKYBOX_xpos.png";
        protected string NegativeY = "./Resources/Cubemap/SKYBOX_yneg.png";
        protected string PositiveY = "./Resources/Cubemap/SKYBOX_ypos.png";
        protected string NegativeZ = "./Resources/Cubemap/SKYBOX_zneg.png";
        protected string PositiveZ = "./Resources/Cubemap/SKYBOX_zpos.png";
    }
}
