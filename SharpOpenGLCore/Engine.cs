using Core;
using Core.CustomSerialize;
using Core.Primitive;
using Core.Tickable;
using OpenTK.Mathematics;
using SharpOpenGL.Font;
using SharpOpenGL.Light;
using System;
using System.Diagnostics;
using System.Threading;
using ZeroFormatter.Formatters;

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
            Formatter<DefaultResolver, Vector3>.Register(new Vector3Formatter<DefaultResolver>());
            Formatter<DefaultResolver, Vector2>.Register(new Vector2Formatter<DefaultResolver>());
            Formatter<DefaultResolver, Vector4>.Register(new Vector4Formatter<DefaultResolver>());

            MainThreadId = Thread.CurrentThread.ManagedThreadId;
            
            OpenGLContext.Get().SetMainThreadId(MainThreadId);

            var rusted = SceneObjectManager.Get().CreateSceneObject<PBRSphere>();
            rusted.Scale = 1.5f;
            rusted.Translation = new Vector3(-190, 30, -80);
            rusted.SetNormalTex("./Imported/Resources/Texture/rustediron/rustediron2_normal.imported");
            rusted.SetDiffuseTex("./Imported/Resources/Texture/rustediron/rustediron2_basecolor.imported");
            rusted.SetMetallicTex("./Imported/Resources/Texture/rustediron/rustediron2_metallic.imported");
            rusted.SetRoughnessTex("./Imported/Resources/Texture/rustediron/rustediron2_roughness.imported");

            var gold = SceneObjectManager.Get().CreateSceneObject<PBRSphere>();
            gold.Scale = 1.5f;
            gold.Translation = new Vector3(-190,30,-40);
            gold.SetNormalTex("./Imported/Resources/Texture/gold/gold-scuffed_normal.imported");
            gold.SetDiffuseTex("./Imported/Resources/Texture/gold/gold-scuffed_basecolor.imported");
            gold.SetMetallicTex("./Imported/Resources/Texture/gold/gold-scuffed_metallic.imported");
            gold.SetRoughnessTex("./Imported/Resources/Texture/gold/gold-scuffed_roughness.imported");

            var wood = SceneObjectManager.Get().CreateSceneObject<PBRSphere>();
            wood.Scale = 1.5f;
            wood.Translation = new Vector3(-190, 30, 0);
            wood.SetNormalTex("./Imported/Resources/Texture/floor/mahogfloor_normal.imported");
            wood.SetDiffuseTex("./Imported/Resources/Texture/floor/mahogfloor_basecolor.imported");
            wood.SetRoughnessTex("./Imported/Resources/Texture/floor/mahogfloor_roughness.imported");

            var bamboo = SceneObjectManager.Get().CreateSceneObject<PBRSphere>();
            bamboo.Scale = 1.5f;
            bamboo.Translation = new Vector3(-190, 30, 40);
            bamboo.SetNormalTex("./Imported/Resources/Texture/bamboo/bamboo-wood-semigloss-normal.imported");
            bamboo.SetDiffuseTex("./Imported/Resources/Texture/bamboo/bamboo-wood-semigloss-albedo.imported");
            bamboo.SetRoughnessTex("./Imported/Resources/Texture/bamboo/bamboo-wood-semigloss-roughness.imported");
            bamboo.SetMetallicTex("./Imported/Resources/Texture/bamboo/bamboo-wood-semigloss-metal.imported");

            var metal = SceneObjectManager.Get().CreateSceneObject<PBRSphere>();
            metal.Scale = 1.5f;
            metal.Translation = new Vector3(-190, 30, 80);
            metal.SetNormalTex("./Imported/Resources/Texture/metal/metal-splotchy-normal-dx.imported");
            metal.SetDiffuseTex("./Imported/Resources/Texture/metal/metal-splotchy-albedo.imported");
            metal.SetRoughnessTex("./Imported/Resources/Texture/metal/metal-splotchy-rough.imported");
            metal.SetMetallicTex("./Imported/Resources/Texture/metal/metal-splotchy-metal.imported");

            var tile = SceneObjectManager.Get().CreateSceneObject<PBRSphere>();
            tile.Scale = 1.5f;
            tile.Translation = new Vector3(-190, 30, 120);
            tile.SetNormalTex("./Imported/Resources/Texture/tile/Tiles32_nrm.imported");
            tile.SetDiffuseTex("./Imported/Resources/Texture/tile/Tiles32_col.imported");
            tile.SetRoughnessTex("./Imported/Resources/Texture/tile/Tiles32_rgh.imported");

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
