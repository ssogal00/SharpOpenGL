using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.CustomAttribute;
using SharpOpenGL.Light;

namespace SharpOpenGL
{
    public class DebugDrawer : Singleton<DebugDrawer>
    {
        public void DebugDraw()
        {
            var pointLights = LightManager.Get().GetLightListOfType<PointLight>();
            foreach (var point in pointLights)
            {
                point.Draw();
            }
        }

        public string Name { get; } = "DebugDrawer";

        [ExposeUI] public bool IsGBufferDump { get; set; } = false;
        [ExposeUI] public bool IsDepthDump { get; set; } = false;
        [ExposeUI] public bool IsBloomEnabled { get; set; } = true;
        [ExposeUI] public bool IsFXAAEnabled { get; set; } = true;

        [ExposeUI] public float SceneRoughness { get; set; } = 0.3f;
        [ExposeUI] public float SceneMetallic { get; set; } = 0.6f;
        [ExposeUI] public float DepthDenominator { get; set; } = 10.0f;

        [ExposeUI, Core.CustomAttribute.Range(300,1000)]
        public float LightIntensity { get; set; } = 300.0f;
    }
}
