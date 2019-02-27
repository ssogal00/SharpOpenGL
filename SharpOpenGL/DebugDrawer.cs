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


        [ExposeUI]
        public bool IsGBufferDump { get; set; } = false;

        [ExposeUI] public float SceneRoughness { get; set; } = 0.1f;
        [ExposeUI] public float SceneMetallic { get; set; } = 0.5f;
    }
}
