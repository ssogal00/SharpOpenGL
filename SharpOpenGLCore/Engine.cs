using Core;
using Core.CustomSerialize;
using Core.Primitive;
using Core.Tickable;
using OpenTK.Mathematics;
using System.Diagnostics;
using System.Threading;
using ZeroFormatter.Formatters;
using DirectXTexWrapper;
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

        private SceneBase mCurrentScene =  new SphereScene();

        public SceneBase CurrentScene => mCurrentScene;

        public async void Initialize()
        {
            Formatter<DefaultResolver, Vector3>.Register(new Vector3Formatter<DefaultResolver>());
            Formatter<DefaultResolver, Vector2>.Register(new Vector2Formatter<DefaultResolver>());
            Formatter<DefaultResolver, Vector4>.Register(new Vector4Formatter<DefaultResolver>());

            MainThreadId = Thread.CurrentThread.ManagedThreadId;
            
            OpenGLContext.Get().SetMainThreadId(MainThreadId);

            mCurrentScene.InitializeScene();
            
            var rusted = SceneObjectManager.Get().CreateSceneObject<PBRSphere>();
            rusted.Scale = 1.5f;
            rusted.Translation = new Vector3(-190, 30, -80);
            rusted.SetNormalTex("./Resources/Texture/rustediron/rustediron2_normal.dds");
            rusted.SetDiffuseTex("./Resources/Texture/rustediron/rustediron2_basecolor.dds");
            rusted.SetMetallicTex("./Resources/Texture/rustediron/rustediron2_metallic.dds");
            rusted.SetRoughnessTex("./Resources/Texture/rustediron/rustediron2_roughness.dds");

            var gold = SceneObjectManager.Get().CreateSceneObject<PBRSphere>();
            gold.Scale = 1.5f;
            gold.Translation = new Vector3(-190,30,-40);
            gold.SetNormalTex("./Resources/Texture/gold/gold-scuffed_normal.dds");
            gold.SetDiffuseTex("./Resources/Texture/gold/gold-scuffed_basecolor.dds");
            gold.SetMetallicTex("./Resources/Texture/gold/gold-scuffed_metallic.dds");
            gold.SetRoughnessTex("./Resources/Texture/gold/gold-scuffed_roughness.dds");

            var wood = SceneObjectManager.Get().CreateSceneObject<PBRSphere>();
            wood.Scale = 1.5f;
            wood.Translation = new Vector3(-190, 30, 0);
            wood.SetNormalTex("./Resources/Texture/floor/mahogfloor_normal.dds");
            wood.SetDiffuseTex("./Resources/Texture/floor/mahogfloor_basecolor.dds");
            wood.SetRoughnessTex("./Resources/Texture/floor/mahogfloor_roughness.dds");

            var bamboo = SceneObjectManager.Get().CreateSceneObject<PBRSphere>();
            bamboo.Scale = 1.5f;
            bamboo.Translation = new Vector3(-190, 30, 40);
            bamboo.SetNormalTex("./Resources/Texture/bamboo/bamboo-wood-semigloss-normal.dds");
            bamboo.SetDiffuseTex("./Resources/Texture/bamboo/bamboo-wood-semigloss-albedo.dds");
            bamboo.SetRoughnessTex("./Resources/Texture/bamboo/bamboo-wood-semigloss-roughness.dds");
            bamboo.SetMetallicTex("./Resources/Texture/bamboo/bamboo-wood-semigloss-metal.dds");

            var metal = SceneObjectManager.Get().CreateSceneObject<PBRSphere>();
            metal.Scale = 1.5f;
            metal.Translation = new Vector3(-190, 30, 80);
            metal.SetNormalTex("./Resources/Texture/metal/metal-splotchy-normal-dx.dds");
            metal.SetDiffuseTex("./Resources/Texture/metal/metal-splotchy-albedo.dds");
            metal.SetRoughnessTex("./Resources/Texture/metal/metal-splotchy-rough.dds");
            metal.SetMetallicTex("./Resources/Texture/metal/metal-splotchy-metal.dds");

            var tile = SceneObjectManager.Get().CreateSceneObject<PBRSphere>();
            tile.Scale = 1.5f;
            tile.Translation = new Vector3(-190, 30, 120);
            tile.SetNormalTex("./Resources/Texture/tile/Tiles32_nrm.dds");
            tile.SetDiffuseTex("./Resources/Texture/tile/Tiles32_col.dds");
            tile.SetRoughnessTex("./Resources/Texture/tile/Tiles32_rgh.dds");

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
