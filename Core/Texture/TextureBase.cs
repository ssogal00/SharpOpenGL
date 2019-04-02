
using OpenTK.Graphics.OpenGL;
using System;
using System.IO;

namespace Core.Texture
{
    public class TextureBase : IDisposable
    {
        public TextureBase()
        {
            textureObject = GL.GenTexture();
        }

        public void Dispose()
        {
            if (IsValid)
            {
                GL.DeleteTexture(textureObject);
                textureObject = -1;
            }
        }

        public virtual void Load(string path)
        {
        }

        public virtual void Load(string path, PixelInternalFormat internalFormat, PixelFormat pixelFormat)
        {
        }

        public virtual void Load(byte[] data, int width, int height, PixelInternalFormat internalFormat, PixelFormat pixelFormat)
        { }
        public virtual void Load(float[] data, int width, int height, PixelInternalFormat internalFormat, PixelFormat pixelFormat)
        { }

        public virtual byte[] GetTexImageAsByte()
        {
            return null;
        }

        public virtual float[] GetTexImageAsFloat()
        {
            return null;
        }

        public virtual void Bind()
        {
            if (IsValid)
            {
                GL.BindTexture(TextureTarget.Texture2D, textureObject);
            }
        }

        public virtual void Unbind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }


        public virtual void BindShader(TextureUnit Unit, int SamplerLoc)
        {
            if (IsValid)
            {
                // Sampler.DefaultLinearSampler.BindSampler(Unit);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter ,(int) TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                GL.Uniform1(SamplerLoc, (int)(Unit - TextureUnit.Texture0));
            }
        }

        public bool IsValid
        {
            get
            {
                return textureObject != -1;
            }
        }

        public int Width { get { return m_Width; } }
        public int Height { get { return m_Height; } }
        public int TextureObject { get { return textureObject; } }

        protected int m_Width = 0;
        protected int m_Height = 0;
        protected int textureObject = -1;

        protected Sampler m_Sampler = null;
        protected string m_TextureName = "";
    }
}
