using Core;
using Core.CustomSerialize;
using Core.Primitive;
using Core.Tickable;
using OpenTK.Mathematics;
using System.Diagnostics;
using System.DirectoryServices;
using System.Threading;
using Core.Asset;
using ZeroFormatter.Formatters;
using DirectXTexWrapper;
using FreeTypeGLWrapper;
using SharpOpenGL.Scene;
using SharpOpenGLCore;

namespace SharpOpenGL
{
    public class Engine : Singleton<Engine>
    {

        protected int MainThreadId = 0;

        protected Stopwatch stopwatch = new Stopwatch();

        protected bool bFirstTick = true;

        protected bool bInitialized = false;

        public bool IsInitialized => bInitialized;

        //private SceneBase mCurrentScene =  new SponzaScene();
        private SceneBase mCurrentScene = new SphereScene();

        public SceneBase CurrentScene => mCurrentScene;

        public async void Initialize()
        {
            // register resolver
            Formatter<DefaultResolver, Vector3>.Register(new Vector3Formatter<DefaultResolver>());
            Formatter<DefaultResolver, Vector2>.Register(new Vector2Formatter<DefaultResolver>());
            Formatter<DefaultResolver, Vector4>.Register(new Vector4Formatter<DefaultResolver>());
            
            MainThreadId = Thread.CurrentThread.ManagedThreadId;
            OpenGLContext.Get().SetMainThreadId(MainThreadId);

            AssetManager.Get().ImportStaticMeshes();

            mCurrentScene.InitializeScene();

            ManagedTextureAtlas result = FreeTypeGL.GenerateTextureAtlas(512, 512, 72, "./Resources/Fonts/Vera.ttf");

            bInitialized = true;
        }
        
        public void Tick()
        {
            // frametime in milliseconds
            long frameMilliseconds = stopwatch.ElapsedMilliseconds;
            // gamethread 120fps
            float frameCap = (1.0f / 120.0f) * 1000;

            if (stopwatch.ElapsedMilliseconds < frameCap)
            {
                int sleeptime = (int)(frameCap - frameMilliseconds);
                Thread.Sleep(sleeptime);
            }

            TickableObjectManager.Tick(stopwatch.ElapsedMilliseconds * 0.001f);
            SceneObjectManager.Get().Tick(stopwatch.ElapsedMilliseconds * 0.001f);

            GameThreadDone.Set();

            stopwatch.Reset();
            stopwatch.Start();
        }

        public void WaitForGameThread(int miliseconds=30)
        {
            GameThreadDone.WaitOne(miliseconds);
        }

        public void RequestExit()
        {
            bIsRequestExit = true;
        }

        public bool IsRequestExit => bIsRequestExit;

        protected bool bIsRequestExit = false;

        
        public AutoResetEvent GameThreadDone = new AutoResetEvent(false);
    }
}
