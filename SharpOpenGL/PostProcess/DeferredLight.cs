using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.CustomAttribute;
using Core.Texture;
using Core.Tickable;
using OpenTK;
using SharpOpenGL.Light;


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

        }

        private void UpdateLightInfo()
        {
            IEnumerable<LightBase> lightList = LightManager.Get().GetLightList();

            var lightCount = lightList.Count();

            if (lightCount != LightPositions.Count)
            {
                LightPositions.Clear();
                LightColors.Clear();

                int index = 0;
                foreach (var light in lightList)
                {
                    LightPositions.Add(light.Translation);
                    LightColors.Add(light.Color);
                }
            }
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
                //

                UpdateLightInfo();
                deferredLight.LightCount = this.LightPositions.Count;
                deferredLight.LightPositions = this.LightPositions.ToArray();
                deferredLight.LightColors = this.LightColors.ToArray();

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

        private List<OpenTK.Vector3> LightPositions = new List<OpenTK.Vector3>(64);

        private List<OpenTK.Vector3> LightColors = new List<Vector3>(64);
        
    }
}
