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

        public override void OnWindowResized(object sender, ScreenResizeEventArgs e)
        {
            base.OnWindowResized(sender, e);            
        }

        public override void OnResourceCreate(object sender, EventArgs e)
        {
            base.OnResourceCreate(sender, e);

            PostProcessMaterial = new SharpOpenGL.LightMaterial.LightMaterial();
            m_LightInfo.LightAmbient = new OpenTK.Vector3(0.1f, 0.1f, 0.1f);
            m_LightInfo.LightDiffuse = new OpenTK.Vector3(0.7f, 0.7f, 0.70f);
            m_LightInfo.LightDir = new OpenTK.Vector3(0,1,1);
        }

        public override void Render(TextureBase positionInput, TextureBase colorInput, TextureBase normalInput)
        {
            PostProcessMaterial.Setup();
            PostProcessMaterial.SetTexture("PositionTex", positionInput);
            PostProcessMaterial.SetTexture("DiffuseTex", colorInput);
            PostProcessMaterial.SetTexture("NormalTex", normalInput);

            PostProcessMaterial.SetUniformBufferValue<SharpOpenGL.LightMaterial.Light>("Light", ref m_LightInfo);

            Output.BindAndExecute(() =>
            {
                this.BlitToScreenSpace();
            });
        }

        //

        protected SharpOpenGL.LightMaterial.Light m_LightInfo = new LightMaterial.Light();
    }
}
