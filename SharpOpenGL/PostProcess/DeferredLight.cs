using System;
using Core;
using Core.Texture;
using Core.CustomEvent;


namespace SharpOpenGL.PostProcess
{
    public class DeferredLight : SharpOpenGL.PostProcess.PostProcessBase
    {
        public DeferredLight()
            : base()
        {
        }

        public override void OnWindowResize(object sender, ScreenResizeEventArgs e)
        {
            base.OnWindowResize(sender, e);            
        }

        public override void OnGLContextCreated(object sender, EventArgs e)
        {
            base.OnGLContextCreated(sender, e);

            PostProcessMaterial = new SharpOpenGL.LightMaterial.LightMaterial();
            m_LightInfo.LightAmbient = new OpenTK.Vector3(0.1f, 0.1f, 0.1f);
            m_LightInfo.LightDiffuse = new OpenTK.Vector3(1.0f, 1.0f, 1.0f);
            m_LightInfo.LightDir = new OpenTK.Vector3(0,1,1);
        }

        public override void Render(TextureBase positionInput, TextureBase colorInput, TextureBase normalInput)
        {
            Output.BindAndExecute(PostProcessMaterial, () =>
            {
                PostProcessMaterial.SetTexture("PositionTex", positionInput);
                PostProcessMaterial.SetTexture("DiffuseTex", colorInput);
                PostProcessMaterial.SetTexture("NormalTex", normalInput);

                PostProcessMaterial.SetUniformBufferValue<SharpOpenGL.LightMaterial.Light>("Light", ref m_LightInfo);

                BlitToScreenSpace();
            });
        }

        //

        protected SharpOpenGL.LightMaterial.Light m_LightInfo = new LightMaterial.Light();
    }
}
