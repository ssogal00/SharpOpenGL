using Core;
using Core.CustomAttribute;
using Core.Texture;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SharpOpenGL.Light;
using System;
using System.Collections.Generic;
using System.Linq;
using CompiledMaterial.LightMaterial;

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

            PostProcessMaterial = new LightMaterial();

        }

        private void UpdateLightInfo()
        {
            IEnumerable<PointLight> lightList = LightManager.Get().GetLightListOfType<PointLight>();

            var lightCount = lightList.Count();

            if (lightCount != LightPositions.Count)
            {
                LightPositions.Clear();
                LightColors.Clear();
                LightMinMaxs.Clear();

                
                foreach (var light in lightList)
                {
                    LightPositions.Add(light.Translation);
                    LightColors.Add(light.Color);
                    LightMinMaxs.Add(light.MinMaxRadius);
                }
            }
        }

        public override void Render(TextureBase colorInput,  TextureBase normalInput, TextureBase positionInput, TextureBase ambientIrradiancemap, TextureBase lutMap, TextureBase prefilterMap)
        {
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);

            Output.BindAndExecute(PostProcessMaterial, () =>
            {
                var deferredLight = (LightMaterial) PostProcessMaterial;
                deferredLight.PositionTex2D = positionInput;
                deferredLight.NormalTex2D = normalInput;
                deferredLight.DiffuseTex2D = colorInput;
                deferredLight.IrradianceMap2D = ambientIrradiancemap;
                deferredLight.BrdfLUT2D = lutMap;
                deferredLight.PrefilterMap2D = prefilterMap;

                
                deferredLight.CameraTransform_View = CameraManager.Get().CurrentCameraView;
                deferredLight.CameraTransform_Proj = CameraManager.Get().CurrentCameraProj;
                //

                UpdateLightInfo();
                deferredLight.LightCount = this.LightPositions.Count;
                deferredLight.LightPositions = this.LightPositions.ToArray();
                deferredLight.LightColors = this.LightColors.Select(x => x * DebugDrawer.Get().LightIntensity).ToArray();
                deferredLight.LightMinMaxs = this.LightMinMaxs.ToArray();

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


        private List<OpenTK.Vector3> LightPositions = new List<OpenTK.Vector3>(64);

        private List<OpenTK.Vector3> LightColors = new List<Vector3>(64);

        private List<OpenTK.Vector2> LightMinMaxs = new List<Vector2>(64);
        
    }
}
