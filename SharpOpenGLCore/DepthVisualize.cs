using System;
using Core;
using Core.Texture;
using System.Diagnostics;
using System.Windows;
using System.Drawing;
using CompiledMaterial.DepthVisualizeMaterial;

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

            PostProcessMaterial = ShaderManager.Get().GetMaterial<DepthVisualizeMaterial>();

            Debug.Assert(PostProcessMaterial != null);
        }

        public override void Render(TextureBase Input0)
        {
            var concretemtl = (DepthVisualizeMaterial) (PostProcessMaterial);

            Output.BindAndExecute(concretemtl, ()=>
            {
                concretemtl.DepthTex2D = Input0;
                concretemtl.Near = CameraManager.Get().CurrentCamera.Near;
                concretemtl.Far = CameraManager.Get().CurrentCamera.Far;

                BlitToScreenSpace();
            });
        }
    }
}
