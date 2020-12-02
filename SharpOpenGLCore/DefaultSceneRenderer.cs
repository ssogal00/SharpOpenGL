using System;
using System.Collections.Generic;
using System.Text;
using Core;
using Core.Buffer;
using Core.Texture;
using OpenTK.Windowing.Desktop;
using SharpOpenGL;
using SharpOpenGL.PostProcess;
using SharpOpenGL.Scene;
using SharpOpenGL.Transform;

namespace SharpOpenGLCore
{
    public class DefaultSceneRenderer : SceneRendererBase
    {
        public override void LoadScene()
        {
            mScreenBlit = new BlitToScreen();
            lightPostProcess = new DeferredLight();
            gbufferVisualize = new GBufferVisualize();
            depthVisualize = new DepthVisualize();
            mEquirectToCubemap = new EquirectangleToCubemap();
            convolution = new CubemapConvolutionTransform();
            fxaa = new FXAAPostProcess();
            lut = new LookUpTable2D();
            prefilter = new Prefilter();
            mRenderGBuffer = new GBuffer(1024,768);
            mScreenBlit.SetGridSize(1, 1);
            mSkybox = new Skybox();
            
            mEquirectToCubemap.Transform();
            convolution.SetSourceCubemap(mEquirectToCubemap.ResultCubemap);
            convolution.Transform();

            prefilter.SetEnvMap(mEquirectToCubemap.ResultCubemap);
            prefilter.Transform();

            lut.Render();

            mSkybox.SetCubemapTexture(mEquirectToCubemap.ResultCubemap);
        }

        public override void RenderScene(SceneBase scene)
        {
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
                    scene.Render();
                }
            );
            
            // lighting
            lightPostProcess.Render(mRenderGBuffer.GetColorAttachement, 
                mRenderGBuffer.GetNormalAttachment, 
                mRenderGBuffer.GetPositionAttachment, 
                convolution.ResultCubemap, 
                lut.GetOutputRenderTarget().ColorAttachment0, 
                prefilter.ResultCubemap);

            // blit gbuffer to screen
            // gbufferVisualize.Render(mRenderGBuffer.GetColorAttachement, mRenderGBuffer.GetNormalAttachment, mRenderGBuffer.GetPositionAttachment, mRenderGBuffer.GetMotionAttachment);
            mScreenBlit.Blit(lightPostProcess.OutputColorTex0, 0, 0, 1, 1);

            /*mRenderGBuffer.BindAndExecute(
                () =>
                {
                    mRenderGBuffer.Clear();
                });

            mRenderGBuffer.BindAndExecute
            (
                () =>
                {
                    Engine.Get().CurrentScene.Render();
                }
            );

            if (true)
            {
                gbufferVisualize.Render(mRenderGBuffer.GetColorAttachement, mRenderGBuffer.GetNormalAttachment, mRenderGBuffer.GetPositionAttachment, mRenderGBuffer.GetMotionAttachment);
                mScreenBlit.Blit(gbufferVisualize.OutputColorTex0, 0, 0, 2, 2);
            }
            else if (bTestTextureTest)
            {
                gbufferVisualize.Render(testTexture);
                mScreenBlit.Blit(testTexture, 0, 0, 2, 2);
            }

            else if (DebugDrawer.Get().IsDepthDump)
            {
                depthVisualize.Render(mRenderGBuffer.GetDepthAttachment);
                mScreenBlit.Blit(depthVisualize.OutputColorTex0, 0, 0, 2, 2);
            }
            else
            {
                lightPostProcess.Render(mRenderGBuffer.GetColorAttachement, mRenderGBuffer.GetNormalAttachment, mRenderGBuffer.GetPositionAttachment, convolution.ResultCubemap, lut.GetOutputRenderTarget().ColorAttachment0, prefilter.ResultCubemap);

                GL.Viewport(0, 0, Width, Height);

                if (false)
                {
                    fxaa.Render(lightPostProcess.OutputColorTex0);
                    mScreenBlit.Blit(fxaa.OutputColorTex0, 0, 0, 2, 2);
                }
                else
                {
                    mScreenBlit.Blit(lightPostProcess.OutputColorTex0, 0, 0, 2, 2);
                }
            }*/
        }

        protected BlitToScreen mScreenBlit;

        #region @PostProcess Start
        protected DeferredLight lightPostProcess;
        protected GBufferVisualize gbufferVisualize;
        protected DepthVisualize depthVisualize;
        protected EquirectangleToCubemap mEquirectToCubemap;
        protected CubemapConvolutionTransform convolution;
        protected FXAAPostProcess fxaa;
        #endregion

        protected LookUpTable2D lut;
        protected Prefilter prefilter;
        protected GBuffer mRenderGBuffer;

        protected Skybox mSkybox;

    }
}
