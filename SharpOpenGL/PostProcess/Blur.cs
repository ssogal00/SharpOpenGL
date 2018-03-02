using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Texture;

namespace SharpOpenGL.PostProcess
{
    public class Blur : Core.PostProcess
    {
        public Blur(Core.MaterialBase.MaterialBase material, RenderTargetTexture input)
            : base(material)
        {
            
        }

        public override void Render()
        {
            //PostProcessMaterial.SetUniformVector2ArrayData("BlurOffsets", );
            //PostProcessMaterial.SetUniformVector2ArrayData("BlurWeights", );
            PostProcessMaterial.SetTexture("ColorTex", Input.GetTextureObject);
        }

        protected RenderTargetTexture Input = null;
        protected RenderTargetTexture Output = null;


    }
}
