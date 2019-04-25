using Core;
using Core.Buffer;
using Core.CustomEvent;
using Core.Primitive;
using Core.Texture;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Core.Asset;
using SharpOpenGL.PostProcess;
using SharpOpenGL.Transform;
using System;
using System.Drawing;

namespace SharpOpenGL
{
    public class RenderingThreadWindow : GameWindow
    {
        protected event EventHandler<EventArgs> OnGLContextCreated;
        protected event EventHandler<ScreenResizeEventArgs> OnWindowResize;
        protected BlitToScreen ScreenBlit = new BlitToScreen();

        #region @PostProcess Start
        protected Skybox skyboxPostProcess = new Skybox();
        protected DeferredLight lightPostProcess = new DeferredLight();
        protected GBufferVisualize gbufferVisualize = new GBufferVisualize();
        protected DepthVisualize depthVisualize = new DepthVisualize();
        protected ResolvePostProcess resolvePostProcess = new ResolvePostProcess();
        protected EquirectangleToCubemap equirectToCube = new EquirectangleToCubemap();
        protected CubemapConvolutionTransform convolution = new CubemapConvolutionTransform();
        protected SSAO ssaoPostProcess = new SSAO();
        protected HDAO hdaoPostProcess = new HDAO();
        protected FXAAPostProcess fxaa = new FXAAPostProcess();
        #endregion

        protected LookUpTable2D lut = new LookUpTable2D();
        protected Prefilter prefilter = new Prefilter();

        protected GBuffer renderGBuffer = new GBuffer(1024, 768);
        protected StaticMeshObject sponzamesh = null;
        protected StaticMeshObject pistol = null;
        protected bool bInitialized = false;

        public event EventHandler<OpenTK.Input.KeyboardKeyEventArgs> OnKeyDownEvent;
        public event EventHandler<OpenTK.Input.KeyboardKeyEventArgs> OnKeyUpEvent;

#region @Mouse Info
        private OpenTK.Vector2 LastMouseBtnDownPosition = new Vector2(0);
        private OpenTK.Vector2 LastMouseBtnMovePosition = new Vector2(0);
        private bool RightMouseBtnDown = false;
#endregion

        public RenderingThreadWindow(int width, int height)
        :base (width, height)
        {
            
        }

        protected override void OnLoad(EventArgs e)
        {
            this.Title = "MyEngine";

            OnGLContextCreated += Sampler.OnResourceCreate;
            OnGLContextCreated += RenderResource.OnOpenGLContextCreated;

            OnWindowResize += CameraManager.Get().OnWindowResized;

            OnKeyDownEvent += CameraManager.Get().OnKeyDown;
            OnKeyUpEvent += CameraManager.Get().OnKeyUp;

            OnKeyDownEvent += this.HandleKeyDownEvent;

            VSync = VSyncMode.Off;

            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.TextureCubeMap);
            GL.Enable(EnableCap.TextureCubeMapSeamless);
            

            ShaderManager.Get().CompileShaders();
            AssetManager.Get().ImportStaticMeshes();
            TextureManager.Get().ImportTextures();

            OnGLContextCreated(this, e);
            ScreenBlit.SetGridSize(2, 2);

            sponzamesh = new StaticMeshObject("sponza2.staticmesh");
            sponzamesh.IsMetallicOverride = sponzamesh.IsRoughnessOverride = true;
            sponzamesh.Metallic = 0.6f;
            sponzamesh.Roughness = 0.3f;

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

            // add ui editable objects
            UIThread.Get().Enqueue
            (
                () =>
                {
                    UIThread.EditorWindow.SetSceneObjectList(SceneObjectManager.Get().GetEditableSceneObjectList());
                    UIThread.EditorWindow.AddObjectToWatch(DebugDrawer.Get());
                }
            );

            bInitialized = true;
        }


        protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            OnKeyDownEvent?.Invoke(this, e);
        }

        protected override void OnKeyUp(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (OnKeyUpEvent != null)
            {
                OnKeyUpEvent(this, e);
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Right)
            {
                LastMouseBtnDownPosition = new Vector2(e.X, e.Y);
                RightMouseBtnDown = true;
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
        }
        //
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (RightMouseBtnDown)
            {
                if (Math.Abs(e.XDelta) > 0)
                {
                    CameraManager.Get().CurrentCamera.RotateYaw(e.XDelta * 0.01f);
                }

                if (Math.Abs(e.YDelta) > 0)
                {
                    CameraManager.Get().CurrentCamera.RotatePitch(e.YDelta * 0.01f);
                }
            }
        }
        //
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Right)
            {
                RightMouseBtnDown = false;
            }
        }


        public void HandleKeyDownEvent(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                CameraManager.Get().SwitchCamera();
            }
            else if (e.Key == Key.F2)
            {
                CameraManager.Get().CurrentCamera.FOV += OpenTK.MathHelper.DegreesToRadians(1.0f);
            }
            else if (e.Key == Key.F3)
            {
                CameraManager.Get().CurrentCamera.FOV -= OpenTK.MathHelper.DegreesToRadians(1.0f);
            }
            else if (e.Key == Key.Tilde)
            {   
                UIThread.Get().Enqueue
                (
                    () =>
                    {
                        UIThread.EditorWindow.SetSceneObjectList(SceneObjectManager.Get().GetEditableSceneObjectList());
                        UIThread.EditorWindow.AddObjectToWatch(DebugDrawer.Get());
                    }
                );
            }
            else if (e.Key == Key.F4)
            {
                DebugDrawer.Get().IsGBufferDump = !DebugDrawer.Get().IsGBufferDump;
            }
            else if (e.Key == Key.F5)
            {
                gbufferVisualize.ChangeVisualizeMode();
            }
        }

        protected override void OnUnload(EventArgs e)
        {
            Engine.Get().RequestExit();
        }
        
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);

            if (this.Width == 0 || this.Height == 0)
            {
                return;
            }

            ScreenResizeEventArgs eventArgs = new ScreenResizeEventArgs
            {
                Width = Width,
                Height = Height
            };

            float fAspectRatio = Width / (float)Height;

            OnWindowResize(this, eventArgs);
            ResizableManager.Get().ResizeEventHandler(this, eventArgs);
            
            this.Title = string.Format("MyEngine({0}x{1})", Width, Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            RenderingThread.Get().ExecuteTimeSlice();
        }

        private void RenderSkybox()
        {
            skyboxPostProcess.Render();
        }

        private void ClearGBuffer()
        {
            renderGBuffer.BindAndExecute(
                () =>
                {
                    renderGBuffer.Clear();
                });
        }

        private void DrawSceneObjects()
        {

        }
        
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(Color.Aqua);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

            if (bInitialized == false || Engine.Get().IsInitialized == false)
            {
                SwapBuffers();
                return;
            }

            renderGBuffer.BindAndExecute(
                () =>
                {
                    renderGBuffer.Clear();
                });

            renderGBuffer.BindAndExecute
            (
                () =>
                {
                    skyboxPostProcess.Render();
                    SceneObjectManager.Get().Draw();
                    DebugDrawer.Get().DebugDraw();
                }
            );

            if (DebugDrawer.Get().IsGBufferDump)
            {
                gbufferVisualize.Render(renderGBuffer.GetColorAttachement, renderGBuffer.GetNormalAttachment, renderGBuffer.GetPositionAttachment, renderGBuffer.GetMotionAttachment);
                ScreenBlit.Blit(gbufferVisualize.OutputColorTex0, 0,0,2,2);
            }
            else if (DebugDrawer.Get().IsDepthDump)
            {
                depthVisualize.Render(renderGBuffer.GetDepthAttachment);
                ScreenBlit.Blit(depthVisualize.OutputColorTex0, 0, 0, 2, 2);
            }
            else
            {
                lightPostProcess.Render(renderGBuffer.GetColorAttachement, renderGBuffer.GetNormalAttachment, renderGBuffer.GetPositionAttachment ,convolution.ResultCubemap, lut.GetOutputRenderTarget().ColorAttachment0, prefilter.ResultCubemap);

                GL.Viewport(0,0, Width, Height);

                if (DebugDrawer.Get().IsFXAAEnabled)
                {
                    fxaa.Render(lightPostProcess.OutputColorTex0);
                    ScreenBlit.Blit(fxaa.OutputColorTex0, 0, 0, 2, 2);
                }
                else
                {
                    ScreenBlit.Blit(lightPostProcess.OutputColorTex0, 0, 0, 2, 2);
                }
            }

            SwapBuffers();
        }
    }
}
