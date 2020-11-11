using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompiledMaterial.FXAAMaterial;
using Core;
using Core.Texture;

namespace SharpOpenGL.PostProcess
{
    public class FXAAPostProcess : PostProcessBase
    {
        public FXAAPostProcess()
        {
            this.Name = "FXAAPostProcess";
        }

        public override void OnGLContextCreated(object sender, EventArgs e)
        {
            base.OnGLContextCreated(sender, e);
            PostProcessMaterial = ShaderManager.Get().GetMaterial<FXAAMaterial>();
        }

        public override void Render(TextureBase Input)
        {
            var fxaaMaterial = (FXAAMaterial) PostProcessMaterial;

            Output.BindAndExecute(fxaaMaterial, () =>
            {
                fxaaMaterial.ScreenTex2D = Input;
                fxaaMaterial.InverseScreenSize = OutputRenderTarget.InverseRenderTargetSize;
                BlitToScreenSpace();
            });
        }
    }
}
