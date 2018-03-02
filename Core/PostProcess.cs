using System;
using Core.Texture;
using Core.MaterialBase;

namespace Core
{
    public abstract class PostProcess 
    {   
        public PostProcess(MaterialBase.MaterialBase Material)
        {
            PostProcessMaterial = Material;
        }

        public virtual void Render()
        {
        }        

        protected RenderTargetTexture Output = null;

        protected MaterialBase.MaterialBase PostProcessMaterial = null;
    }
}
