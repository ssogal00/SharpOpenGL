﻿using Core;
using OpenTK.Mathematics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Engine.Light
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

            for (int i = 0; i < 12; ++i)
            {
                var light1 = new PointLight();

                var X = i * 100 - 1000;
                var Y =  60 + 60* (i % 2) ;
                var Z = 0;

                var R = (float)random.Next(1, 2);
                var G = (float)random.Next(1, 2);
                var B = (float)random.Next(1, 2);

                light1.Translation = new Vector3(X,Y,Z);
                light1.Color = new Vector3(R, G, B);
                AddLight(light1);
            }
        }

        private ConcurrentDictionary<string, LightBase> lightDictionary = new ConcurrentDictionary<string, LightBase>();
    }
}
