using System;
using Core;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Core.MaterialBase;

namespace SharpOpenGL.PostProcess
{
    public class Skybox : PostProcessBase
    {
        public override void OnGLContextCreated(object sender, EventArgs e)
        {
            base.OnGLContextCreated(sender, e);

            PostProcessMaterial = new CubemapMaterial.CubemapMaterial();
        }

        public override void Render()
        {
            Output.BindAndExecute(PostProcessMaterial, () =>
            {
                //PostProcessMaterial.SetUniformBufferValue<Matrix4>("", );
                //PostProcessMaterial.SetTexture("", );
                BlitToScreenSpace();
            });
        }
    }
}
