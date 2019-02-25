using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace SharpOpenGL.Light
{
    public class LightManager : Singleton<LightManager>
    {
        public void AddLight(LightBase light)
        {
            lightDictionary.TryAdd(light.Name, light);
        }

        public IEnumerable<LightBase> GetLightList()
        {
            return lightDictionary.Values;
        }

        public void Initialize()
        {
            var random = new Random();

            for (int i = 0; i < 10; ++i)
            {
                var light1 = new PointLight();

                var X = (float) random.Next(-100, 100);
                var Y = (float)random.Next(0, 100);
                var Z = (float)random.Next(-100, 100);

                var R = (float)random.Next(300, 600);
                var G = (float)random.Next(300, 600);
                var B = (float)random.Next(300, 600);

                light1.Translation = new OpenTK.Vector3(X,Y,Z);
                light1.Color = new OpenTK.Vector3(R, G, B);
                AddLight(light1);
            }
        }

        private ConcurrentDictionary<string, LightBase> lightDictionary = new ConcurrentDictionary<string, LightBase>();
    }
}
