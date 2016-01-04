using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SharpOpenGL.Texture
{
    public class Sampler : IDisposable
    {
        public Sampler()
        {
            m_SamplerObject = GL.GenSampler();
        }

        public void Dispose()
        {
            if(m_SamplerObject != -1)
            {
                GL.DeleteSampler(m_SamplerObject);
                m_SamplerObject = -1;
            }            
        }
        
        public void SetMagFilter(TextureMagFilter Filter)
        {
            if(m_SamplerObject != -1)
            {
                GL.SamplerParameter(m_SamplerObject, SamplerParameterName.TextureMagFilter, (int) Filter);
            }
        }

        public void SetMinFilter(TextureMinFilter Filter)
        {
            if(m_SamplerObject != -1)
            {
                GL.SamplerParameter(m_SamplerObject, SamplerParameterName.TextureMinFilter, (int)Filter);
            }
        }

        public void SetWrapS(TextureWrapMode WrapMode)
        {
            if (m_SamplerObject != -1)
            {
                GL.SamplerParameter(m_SamplerObject, SamplerParameterName.TextureWrapS, (int)WrapMode);
            }
        }

        public void SetWrapT(TextureWrapMode WrapMode)
        {
            if(m_SamplerObject != -1)
            {
                GL.SamplerParameter(m_SamplerObject, SamplerParameterName.TextureWrapT, (int)WrapMode);
            }
        }

        public void BindSampler(TextureUnit UnitToBind)
        {
            if(m_SamplerObject != -1)
            {
                GL.BindSampler(((int)UnitToBind - (int)TextureUnit.Texture0) , m_SamplerObject);
            }
        }

        protected int m_SamplerObject = -1;
    }
}
