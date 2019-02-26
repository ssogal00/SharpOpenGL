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

        public IEnumerable<T> GetLightListOfType<T>() where T : LightBase
        {
            foreach (var light in lightDictionary.Values)
            {
                if (light is T)
                {
                    yield return (T) light;
                }
            }
        }

        public void Initialize()
        {
            var random = new Random();

            for (int i = 0; i < 20; ++i)
            {
                var light1 = new PointLight();

                var X = i * 30 - 200;
                var Y = 40 + (i % 2) * 20;
                var Z = (i % 2) * 20;

                var R = (float)random.Next(500, 600);
                var G = (float)random.Next(500, 600);
                var B = (float)random.Next(500, 600);

                light1.Translation = new OpenTK.Vector3(X,Y,Z);
                light1.Color = new OpenTK.Vector3(R, G, B);
                AddLight(light1);
            }
        }

        private ConcurrentDictionary<string, LightBase> lightDictionary = new ConcurrentDictionary<string, LightBase>();
    }
}
