
using OpenTK.Graphics.OpenGL;
using System;
using System.IO;

namespace Core.Texture
{
    public class TextureBase : IDisposable
    {
        public TextureBase()
        {
            m_TextureObject = GL.GenTexture();
            m_Sampler = new Sampler();
            m_Sampler.SetMagFilter(TextureMagFilter.Linear);
            m_Sampler.SetMinFilter(TextureMinFilter.Linear);
        }

        public void Dispose()
        {
            if (IsValid)
            {
                GL.DeleteTexture(m_TextureObject);
                m_TextureObject = -1;
            }
        }

        public virtual void Bind()
        {
            if (IsValid)
            {
                GL.BindTexture(TextureTarget.Texture2D, m_TextureObject);
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
                m_Sampler.BindSampler(Unit);
                GL.Uniform1(SamplerLoc, (int)(Unit - TextureUnit.Texture0));
            }
        }

        public bool IsValid
        {
            get
            {
                return m_TextureObject != -1;
            }
        }

        public int Width { get { return m_Width; } }
        public int Height { get { return m_Height; } }
        public int TextureObject { get { return m_TextureObject; } }

        protected int m_Width = 0;
        protected int m_Height = 0;
        protected int m_TextureObject = -1;

        protected Sampler m_Sampler = null;
        protected string m_TextureName = "";
    }
}
