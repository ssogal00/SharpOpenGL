using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Core;
using Core.Buffer;
using Engine.PostProcess;
using Engine.Scene;
using Engine.Transform;
using Engine.UI;

namespace Engine
{
    public class RenderThreadTestSceneRenderer : SceneRendererBase
    {
        public RenderThreadTestSceneRenderer(SceneBase scene)
        : base(scene)
        {
        }

        public override void Initialize()
        {
            Debug.Assert(RenderingThread.IsInRenderingThread());
            mScreenBlit = new BlitToScreen();
            mLightPostProcess = new DeferredLight();

            mEquirectToCubemap = new EquirectangleToCubemap();
            mConvolution = new CubemapConvolutionTransform();
            mFxaa = new FXAAPostProcess();
            mLut = new LookUpTable2D();
            mPrefilter = new Prefilter();
            mRenderGBuffer = new GBuffer(1024, 768);
            mScreenBlit.SetGridSize(1, 1);
            mSkybox = new Skybox();

            mEquirectToCubemap.Transform();
            mConvolution.SetSourceCubemap(mEquirectToCubemap.ResultCubemap);
            mConvolution.Transform();

            mPrefilter.SetEnvMap(mEquirectToCubemap.ResultCubemap);
            mPrefilter.Transform();

            mLut.Render();

            mSkybox.SetCubemapTexture(mEquirectToCubemap.ResultCubemap);
        }

        protected override void SyncGameObjectWithRenderObject()
        {
            foreach (var go in mReferenceScene.GameObjectList)
            {
                if (!mRenderThreadGameObjectMap.ContainsKey(go.ID))
                {
                    var ro = new RenderThreadGameObject(go);
                    mRenderThreadGameObjectMap.Add(go.ID, ro);
                }
            }
        }

        public override void RenderScene(SceneBase scene)
        {
            SyncGameObjectWithRenderObject();
            // clear gbuffer
            mRenderGBuffer.BindAndExecute(
                () =>
                {
                    mRenderGBuffer.Clear();
                });

            // render scene
            // fill gbuffer
            mRenderGBuffer.BindAndExecute
            (
                () =>
                {
                    mSkybox.Render();
                    foreach (var ro in mRenderThreadGameObjectMap)
                    {
                        ro.Value.Render();
                    }
                    UIManager.Instance.RenderUI();
                }
            );

            // lighting
            mLightPostProcess.Render(mRenderGBuffer.GetColorAttachement,
                mRenderGBuffer.GetNormalAttachment,
                mRenderGBuffer.GetPositionAttachment,
                mConvolution.ResultCubemap,
                mLut.GetOutputRenderTarget().ColorAttachment0,
                mPrefilter.ResultCubemap);

            mScreenBlit.Blit(mLightPostProcess.OutputColorTex0, 0, 0, 1, 1);
        }

        protected BlitToScreen mScreenBlit;

        #region @PostProcess Start
        protected DeferredLight mLightPostProcess;
        protected EquirectangleToCubemap mEquirectToCubemap;
        protected CubemapConvolutionTransform mConvolution;
        protected FXAAPostProcess mFxaa;
        #endregion

        protected LookUpTable2D mLut;
        protected Prefilter mPrefilter;
        protected GBuffer mRenderGBuffer;

        protected Skybox mSkybox;
    }
}
