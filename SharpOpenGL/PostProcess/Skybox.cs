using System;
using Core;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Core.MaterialBase;
using Core.Texture;

namespace SharpOpenGL.PostProcess
{
    public class Skybox : PostProcessBase
    {
        public Skybox()
        :base()
        { }

        public override void OnGLContextCreated(object sender, EventArgs e)
        {
            base.OnGLContextCreated(sender, e);

            PostProcessMaterial = new CubemapMaterial.CubemapMaterial();

            cubemapTexture = new CubemapTexture();
          
            cubemapTexture.Load();
        }

        public override void Render()
        {
            Output.BindAndExecute(PostProcessMaterial, () =>
            {
                // 
                PostProcessMaterial.SetUniformVarData("ViewMatrix", ref ViewMatrix);
                PostProcessMaterial.SetTexture("texCubemap", cubemapTexture);
                //
                BlitToScreenSpace();
            });
        }

        protected CubemapTexture cubemapTexture = null;

        public OpenTK.Matrix4 ViewMatrix;
    }
}
