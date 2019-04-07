using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Texture;


namespace SharpOpenGL.PostProcess
{
    public class BloomPostProcess : PostProcessBase
    {
        public BloomPostProcess()
            : base()
        {
            this.Name = "Bloom";
            this.bCreateCustomRenderTarget = true;
        }

        protected override void CreateCustomRenderTarget()
        {
            Output = new RenderTarget(1024, 768, 1, false, 0.5f, false);
            Output.Initialize();
        }

        public override void OnGLContextCreated(object sender, EventArgs e)
        {
            base.OnGLContextCreated(sender, e);

            PostProcessMaterial = new BloomMaterial.BloomMaterial();
        }

        public override void Render(TextureBase input)
        {
            Output.BindAndExecute(PostProcessMaterial, () =>
            {
                var bloom = (BloomMaterial.BloomMaterial)(PostProcessMaterial);

                bloom.ColorTex2D = input;

                BlitToScreenSpace();
            });
        }

    }
}
