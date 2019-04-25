using Core;
using Core.Texture;
using System;
using CompiledMaterial.BloomMaterial;


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
            Output = new RenderTarget(1024, 768, 1, false, 1.0f, false);
            Output.Initialize();
        }

        public override void OnGLContextCreated(object sender, EventArgs e)
        {
            base.OnGLContextCreated(sender, e);

            PostProcessMaterial = new BloomMaterial();
        }

        public override void Render(TextureBase input)
        {
            Output.BindAndExecute(PostProcessMaterial, () =>
            {
                var bloom = (BloomMaterial)(PostProcessMaterial);

                bloom.ColorTex2D = input;

                BlitToScreenSpace();
            });
        }

    }
}
