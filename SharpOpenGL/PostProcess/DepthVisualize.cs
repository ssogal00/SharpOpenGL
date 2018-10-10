using System;
using Core;
using Core.Texture;
using SharpOpenGL.Asset;
using System.Diagnostics;
using SharpOpenGL.DepthVisualizeMaterial;

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

            PostProcessMaterial = AssetManager.GetAsset<DepthVisualizeMaterial.DepthVisualizeMaterial>("DepthVisualizeMaterial");

            Debug.Assert(PostProcessMaterial != null);
        }

        public override void Render(TextureBase Input0)
        {
            Output.BindAndExecute(PostProcessMaterial, ()=>
            {
                PostProcessMaterial.SetTexture("DepthTex", Input0);
                PostProcessMaterial.SetUniformVarData("MaxDepth", 10000.0f);
                BlitToScreenSpace();
            });
        }

    }
}
