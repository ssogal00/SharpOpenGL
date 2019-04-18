using System;
using Core;
using Core.Texture;
using System.Diagnostics;
using System.Windows;
using System.Drawing;

namespace SharpOpenGL.PostProcess
{
    public class DepthVisualize : PostProcessBase
    {
        public DepthVisualize ()
            :base()
        { }

        public override void OnGLContextCreated(object sender, EventArgs e)
        {
            base.OnGLContextCreated(sender, e);

            PostProcessMaterial = new DepthVisualizeMaterial.DepthVisualizeMaterial();

            Debug.Assert(PostProcessMaterial != null);
        }

        public override void Render(TextureBase Input0)
        {
            Output.ClearColor = Color.Violet;

            Output.BindAndExecute(PostProcessMaterial, ()=>
            {
                PostProcessMaterial.SetTexture("DepthTex", Input0);
                BlitToScreenSpace();
            });
        }

    }
}
