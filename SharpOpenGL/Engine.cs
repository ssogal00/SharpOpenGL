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
using SharpOpenGL;

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

            bInitialized = true;
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

        private TestSceneObject testSceneObject = null;

    }
}
