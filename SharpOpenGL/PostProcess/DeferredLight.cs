using System;
using System.Collections.Generic;
using Core;
using Core.CustomAttribute;
using Core.Texture;
using Core.Tickable;
using OpenTK;


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
            lightInfo.LightSpecular = new OpenTK.Vector3(0.01f,0.01f,0.01f);
            lightInfo.LightDir = new OpenTK.Vector3(1,1,1);
        }

        public override void Render(TextureBase colorInput,  TextureBase normalInput, TextureBase positionInput)
        {
            Output.BindAndExecute(PostProcessMaterial, () =>
            {
                var deferredLight = (LightMaterial.LightMaterial) PostProcessMaterial;
                deferredLight.PositionTex2D = positionInput;
                deferredLight.NormalTex2D = normalInput;
                deferredLight.DiffuseTex2D = colorInput;

                deferredLight.CameraTransform_View = CameraManager.Get().CurrentCameraView;
                deferredLight.CameraTransform_Proj = CameraManager.Get().CurrentCameraProj;

                deferredLight.LightPositions = this.LightPositions.ToArray();
                //deferredLight.LightColors = this.LightColors.ToArray();
                

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

        [ExposeUI]
        [UseSlider(0,1)]
        public float Roughness { get; set; } = 0.05f;

        [ExposeUI] public OpenTK.Vector3 LobeEnergy { get; set; } = new OpenTK.Vector3(1,2,2);

        [ExposeUI] public bool TestBool { get; set; } = false;

        [ExposeUI] public TestEnumValue EnumTest { get; set; } = TestEnumValue.EIs;

        [ExposeUI]
        public SharpOpenGL.LightMaterial.Light lightInfo  = new LightMaterial.Light();

        private List<OpenTK.Vector3> LightPositions = new List<OpenTK.Vector3>
        {
            new Vector3(10,20,10),
            new Vector3(-10,20,10),
            new Vector3(30,20,-10),
            new Vector3(50,20,-10),
        };

        private List<OpenTK.Vector3> LightColors = new List<Vector3>()
        {
            new Vector3(300.0f,300.0f,300.0f),
            new Vector3(300.0f,300.0f,300.0f),
            new Vector3(300.0f,300.0f,300.0f),
            new Vector3(300.0f,300.0f,300.0f),
        };
    }
}
