using System;
using Core;
using Core.CustomAttribute;
using Core.Texture;
using Core.Tickable;


namespace SharpOpenGL.PostProcess
{
    public class DeferredLight : SharpOpenGL.PostProcess.PostProcessBase
    {
        public DeferredLight()
            : base()
        {
            this.Name = "deferredLight";
        }

        public override void OnGLContextCreated(object sender, EventArgs e)
        {
            base.OnGLContextCreated(sender, e);

            PostProcessMaterial = new SharpOpenGL.LightMaterial.LightMaterial();
            lightInfo.LightAmbient = new OpenTK.Vector3(0.3f, 0.3f, 0.3f);
            lightInfo.LightDiffuse = new OpenTK.Vector3(0.7f, 0.7f, 0.7f);
            lightInfo.LightDir = new OpenTK.Vector3(1,1,1);
        }

        public override void Render(TextureBase positionInput, TextureBase colorInput, TextureBase normalInput)
        {
            Output.BindAndExecute(PostProcessMaterial, () =>
            {
                PostProcessMaterial.SetTexture("PositionTex", positionInput);
                PostProcessMaterial.SetTexture("DiffuseTex", colorInput);
                PostProcessMaterial.SetTexture("NormalTex", normalInput);
                PostProcessMaterial.SetUniformVarData("Roughness", Roughness);
                PostProcessMaterial.SetUniformVarData("LobeEnergy", LobeEnergy);

                lightInfo.LightDir = LightDir;

                PostProcessMaterial.SetUniformBufferValue<SharpOpenGL.LightMaterial.Light>("Light", ref lightInfo);

                BlitToScreenSpace();
            });
        }

        public enum TestEnumValue
        {
            EThis,
            EIs,
            ESample,
        };

        //

        [ExposeUI] public float Roughness { get; set; } = 0.05f;

        [ExposeUI] public OpenTK.Vector3 LightDir { get; set; } = new OpenTK.Vector3(1, 1, 1);

        [ExposeUI] public OpenTK.Vector3 LobeEnergy { get; set; } = new OpenTK.Vector3(1,2,2);

        [ExposeUI] public bool TestBool { get; set; } = false;

        [ExposeUI] public TestEnumValue EnumTest { get; set; } = TestEnumValue.EIs;

        [ExposeUI]
        public SharpOpenGL.LightMaterial.Light lightInfo  = new LightMaterial.Light();
    }
}
