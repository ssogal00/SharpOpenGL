
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

        public virtual void Bind()
        {
            if (IsValid)
            {
                GL.BindTexture(TextureTarget.Texture2D, textureObject);
            }
        }

        public void Unbind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }


        public void BindShader(TextureUnit Unit, int SamplerLoc)
        {
            if (IsValid)
            {
                Sampler.DefaultLinearSampler.BindSampler(Unit);
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
