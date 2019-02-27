using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
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

        public bool IsGBufferDump { get; set; } = false;
    }
}
