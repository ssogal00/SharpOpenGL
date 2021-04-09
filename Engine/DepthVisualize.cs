using System;
using Core;
using Core.Texture;
using System.Diagnostics;
using System.Windows;
using System.Drawing;
using CompiledMaterial.DepthVisualizeMaterial;

namespace Engine.PostProcess
{
    public class DepthVisualize : PostProcessBase
    {
        public DepthVisualize()
            : base()
        {
            PostProcessMaterial = ShaderManager.Instance.GetMaterial<DepthVisualizeMaterial>();

            Debug.Assert(PostProcessMaterial != null);
        }

        public override void Render(TextureBase Input0)
        {
            var concretemtl = (DepthVisualizeMaterial) (PostProcessMaterial);

            Output.BindAndExecute(concretemtl, ()=>
            {
                concretemtl.DepthTex2D = Input0;
                concretemtl.Near = CameraManager.Instance.CurrentCamera.Near;
                concretemtl.Far = CameraManager.Instance.CurrentCamera.Far;

                BlitToScreenSpace();
            });
        }
    }
}
