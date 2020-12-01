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
        public override void Initialize()
        {
            ScreenBlit = new BlitToScreen();
            lightPostProcess = new DeferredLight();
            gbufferVisualize = new GBufferVisualize();
            depthVisualize = new DepthVisualize();
            equirectToCube = new EquirectangleToCubemap();
            convolution = new CubemapConvolutionTransform();
            fxaa = new FXAAPostProcess();
            lut = new LookUpTable2D();
            prefilter = new Prefilter();
            renderGBuffer = new GBuffer(1024,768);
            ScreenBlit.SetGridSize(1, 1);

            /* 
            if (equirectToCube.IsTransformCompleted == false)
            {
                equirectToCube.Transform();
                convolution.SetSourceCubemap(equirectToCube.ResultCubemap);
                convolution.Transform();
            }

            prefilter.SetEnvMap(equirectToCube.ResultCubemap);
            prefilter.Transform();

            lut.Render();

            skyboxPostProcess.SetCubemapTexture(equirectToCube.ResultCubemap);
             */
        }

        public override void RenderScene(SceneBase scene)
        {
            // clear gbuffer
            renderGBuffer.BindAndExecute(
                () =>
                {
                    renderGBuffer.Clear();
                });

            // render scene
            renderGBuffer.BindAndExecute
            (
                () =>
                {
                    scene.Render();
                }
            );

            // blit gbuffer to screen
            gbufferVisualize.Render(renderGBuffer.GetColorAttachement, renderGBuffer.GetNormalAttachment, renderGBuffer.GetPositionAttachment, renderGBuffer.GetMotionAttachment);
            ScreenBlit.Blit(gbufferVisualize.OutputColorTex0, 0, 0, 1, 1);

            /*renderGBuffer.BindAndExecute(
                () =>
                {
                    renderGBuffer.Clear();
                });

            renderGBuffer.BindAndExecute
            (
                () =>
                {
                    Engine.Get().CurrentScene.Render();
                }
            );

            if (true)
            {
                gbufferVisualize.Render(renderGBuffer.GetColorAttachement, renderGBuffer.GetNormalAttachment, renderGBuffer.GetPositionAttachment, renderGBuffer.GetMotionAttachment);
                ScreenBlit.Blit(gbufferVisualize.OutputColorTex0, 0, 0, 2, 2);
            }
            else if (bTestTextureTest)
            {
                gbufferVisualize.Render(testTexture);
                ScreenBlit.Blit(testTexture, 0, 0, 2, 2);
            }

            else if (DebugDrawer.Get().IsDepthDump)
            {
                depthVisualize.Render(renderGBuffer.GetDepthAttachment);
                ScreenBlit.Blit(depthVisualize.OutputColorTex0, 0, 0, 2, 2);
            }
            else
            {
                lightPostProcess.Render(renderGBuffer.GetColorAttachement, renderGBuffer.GetNormalAttachment, renderGBuffer.GetPositionAttachment, convolution.ResultCubemap, lut.GetOutputRenderTarget().ColorAttachment0, prefilter.ResultCubemap);

                GL.Viewport(0, 0, Width, Height);

                if (false)
                {
                    fxaa.Render(lightPostProcess.OutputColorTex0);
                    ScreenBlit.Blit(fxaa.OutputColorTex0, 0, 0, 2, 2);
                }
                else
                {
                    ScreenBlit.Blit(lightPostProcess.OutputColorTex0, 0, 0, 2, 2);
                }
            }*/
        }

        protected BlitToScreen ScreenBlit;

        #region @PostProcess Start
        protected DeferredLight lightPostProcess;
        protected GBufferVisualize gbufferVisualize;
        protected DepthVisualize depthVisualize;
        protected EquirectangleToCubemap equirectToCube;
        protected CubemapConvolutionTransform convolution;
        protected FXAAPostProcess fxaa;
        #endregion

        protected LookUpTable2D lut;
        protected Prefilter prefilter;
        protected GBuffer renderGBuffer;

    }
}
