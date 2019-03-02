using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.CustomSerialize;
using ZeroFormatter.Formatters;
using System.Threading;
using Core.Primitive;
using Core.Tickable;
using SharpOpenGL.Asset;
using SharpOpenGL.Font;
using ObjectEditor;
using OpenTK;
using SharpOpenGL;
using SharpOpenGL.Light;

namespace SharpOpenGL
{
    public class Engine : Singleton<Engine>
    {
        protected int MainThreadId = 0;

        protected Stopwatch stopwatch = new Stopwatch();

        protected bool bFirstTick = true;

        protected bool bInitialized = false;

        public bool IsInitialized => bInitialized;

        public void Initialize()
        {
            Formatter<DefaultResolver, OpenTK.Vector3>.Register(new Vector3Formatter<DefaultResolver>());
            Formatter<DefaultResolver, OpenTK.Vector2>.Register(new Vector2Formatter<DefaultResolver>());
            Formatter<DefaultResolver, OpenTK.Vector4>.Register(new Vector4Formatter<DefaultResolver>());

            MainThreadId = Thread.CurrentThread.ManagedThreadId;
            
            OpenGLContext.Get().SetMainThreadId(MainThreadId);

            FontManager.Get().Initialize();

            LightManager.Get().Initialize();

            // for PBR demonstration
            PreCreatePBRSpheres();
            
            bInitialized = true;
        }

        private void PreCreatePBRSpheres()
        {
            for (float metallic = 0; metallic <= 1.0f; metallic += 0.2f)
            {
                float X = -75 + metallic * 150;
                float Y = 10;
                float Z = 10;

                for (float roughness = 0; roughness <= 1.0f; roughness += 0.2f)
                {
                    Y = 10 + roughness * 150;

                    var sphere = SceneObjectManager.Get().CreateSceneObject<Sphere>();
                    sphere.Metallic = metallic;
                    sphere.Roughness = roughness;
                    sphere.Translation = new Vector3(X, Y, Z);
                }
            }
        }

        public void OnObjectCreate(object sender, EventArgs args)
        {
            SceneObjectManager.Get().CreateSceneObject<Sphere>();
        }

        public bool SeperateRenderingThreadEnabled => bIsSperateRenderingThread;

        public void Tick()
        {
            if (bFirstTick == true)
            {
                bFirstTick = false;
            }

            if (bFirstTick == false)
            {
                TickableObjectManager.Tick(stopwatch.ElapsedMilliseconds * 0.001);
                SceneObjectManager.Get().Tick(stopwatch.ElapsedMilliseconds * 0.001);
                stopwatch.Reset();
            }

            stopwatch.Start();
        }

        public void RequestExit()
        {
            bIsRequestExit = true;
        }

        public bool IsRequestExit => bIsRequestExit;

        protected bool bIsSperateRenderingThread = true;

        protected bool bIsRequestExit = false;

        

    }
}
