using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.CustomEvent;
using Core.Texture;

namespace SharpOpenGL.PostProcess
{
    public class ResolvePostProcess : PostProcessBase
    {

        public ResolvePostProcess()
            : base()
        {
        }
        
        public override void OnGLContextCreated(object sender, EventArgs e)
        {
            base.OnGLContextCreated(sender, e);

            PostProcessMaterial = ShaderManager.Get().GetMaterial<Blur.Blur>();
        }

        public override void Render(TextureBase input0, TextureBase input1)
        {
            Output.BindAndExecute(PostProcessMaterial,
                () =>
                {


                }
            );
        }
    }
}
