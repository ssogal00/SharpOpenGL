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
using Core.Tickable;
using SharpOpenGL.Asset;

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

            //AssetManager.Get().DiscoverShader();
            AssetManager.Get().DiscoverStaticMesh();

            while (RenderingThread.Get().IsIdle() == false)
            {
                Thread.Sleep(0);
            }

            bInitialized = true;
        }

        public void Tick()
        {
            if (bFirstTick == true)
            {
                bFirstTick = false;
            }

            if (bFirstTick == false)
            {
                TickableObjectManager.Tick(stopwatch.ElapsedMilliseconds * 0.001);
                stopwatch.Reset();
            }
            
            stopwatch.Start();
        }

        public void RequestExit()
        {
            bIsRequestExit = true;
        }

        public bool IsRequestExit => bIsRequestExit;

        protected bool bIsRequestExit = false;
        
    }
}
