using System;
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
            
        }

    }
}
