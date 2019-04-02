using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Core.Texture
{
    public class Sampler : IDisposable
    {
        public Sampler()
        {
            m_SamplerObject = GL.GenSampler();
        }
        
        public bool IsValid
        {
            get
            {
                return m_SamplerObject != -1;
            }
        }

        public void Dispose()
        {
            if(IsValid)
            {
                GL.DeleteSampler(m_SamplerObject);
                m_SamplerObject = -1;
            }            
        }
        
        public void SetMagFilter(TextureMagFilter Filter)
        {
            if(IsValid)
            {
                GL.SamplerParameter(m_SamplerObject, SamplerParameterName.TextureMagFilter, (int) Filter);
            }
        }

        public void SetMinFilter(TextureMinFilter Filter)
        {
            if(IsValid)
            {
                GL.SamplerParameter(m_SamplerObject, SamplerParameterName.TextureMinFilter, (int)Filter);
            }
        }

        public void SetWrapS(TextureWrapMode WrapMode)
        {
            if (IsValid)
            {
                GL.SamplerParameter(m_SamplerObject, SamplerParameterName.TextureWrapS, (int)WrapMode);
            }
        }

        public void SetWrapT(TextureWrapMode WrapMode)
        {
            if(IsValid)
            {
                GL.SamplerParameter(m_SamplerObject, SamplerParameterName.TextureWrapT, (int)WrapMode);
            }
        }

        public void SetWrapR(TextureWrapMode WrapMode)
        {
            if (IsValid)
            {
                GL.SamplerParameter(m_SamplerObject, SamplerParameterName.TextureWrapR, (int)WrapMode);
            }
        }

        public void BindSampler(TextureUnit UnitToBind)
        {
            if(IsValid)
            {
                GL.BindSampler(((int)UnitToBind - (int)TextureUnit.Texture0) , m_SamplerObject);
            }
        }

        public static void OnResourceCreate(object sender, EventArgs e)
        {
            DefaultLinearSampler = new Sampler();
            DefaultLinearSampler.SetMagFilter(TextureMagFilter.Linear);
            DefaultLinearSampler.SetMinFilter(TextureMinFilter.Linear);

            DefaultPointSampler = new Sampler();
            DefaultPointSampler.SetMagFilter(TextureMagFilter.Nearest);
            DefaultPointSampler.SetMinFilter(TextureMinFilter.Nearest);

            DefaultCubemapSampler = new Sampler();
            DefaultCubemapSampler.SetMinFilter(TextureMinFilter.LinearMipmapLinear);
            DefaultCubemapSampler.SetMagFilter(TextureMagFilter.Linear);
            DefaultCubemapSampler.SetWrapS(TextureWrapMode.ClampToEdge);
            DefaultCubemapSampler.SetWrapT(TextureWrapMode.ClampToEdge);
            DefaultCubemapSampler.SetWrapR(TextureWrapMode.ClampToEdge);

        }

        public static Sampler DefaultLinearSampler = null;
        public static Sampler DefaultPointSampler = null;
        public static Sampler DefaultCubemapSampler = null;

        protected int m_SamplerObject = -1;
    }
}
