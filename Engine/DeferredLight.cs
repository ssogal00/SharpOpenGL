using Core;
using Core.CustomAttribute;
using Core.Texture;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using CompiledMaterial.DeferredLightMaterial;
using Engine.Light;
using OpenTK.Mathematics;

namespace Engine.PostProcess
{
    public class DeferredLight : PostProcessBase
    {
        public DeferredLight()
            : base()
        {
            this.Name = "deferredLight";
            
        }

        private void UpdateLightInfo()
        {
            IEnumerable<PointLight> lightList = LightManager.Instance.GetLightListOfType<PointLight>();

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

            PostProcessMaterial = ShaderManager.Instance.GetMaterial<DeferredLightMaterial>();

            Output.BindAndExecute(PostProcessMaterial,  () =>
            {

                PostProcessMaterial.SetTexture("PositionTex", positionInput);
                PostProcessMaterial.SetTexture("NormalTex", normalInput);
                PostProcessMaterial.SetTexture("DiffuseTex", colorInput);
                PostProcessMaterial.SetTexture("IrradianceMap", ambientIrradiancemap);
                PostProcessMaterial.SetTexture("BrdfLUT", lutMap);
                PostProcessMaterial.SetTexture("PrefilterMap", prefilterMap);

                PostProcessMaterial.SetUniformVariable("View", CameraManager.Instance.CurrentCameraView);
                PostProcessMaterial.SetUniformVariable("Proj", CameraManager.Instance.CurrentCameraProj);
                //

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

        [ExposeUI] public Vector3 LobeEnergy { get; set; } = new Vector3(1,2,2);

        [ExposeUI] public bool TestBool { get; set; } = false;

        [ExposeUI] public TestEnumValue EnumTest { get; set; } = TestEnumValue.EIs;


        private List<Vector3> LightPositions = new List<Vector3>(64);

        private List<Vector3> LightColors = new List<Vector3>(64);

        private List<Vector2> LightMinMaxs = new List<Vector2>(64);
        
    }
}
