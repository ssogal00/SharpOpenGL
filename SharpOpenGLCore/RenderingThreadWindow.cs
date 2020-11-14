using Core;
using Core.Asset;
using Core.Buffer;
using Core.CustomEvent;
using Core.Primitive;
using Core.Texture;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SharpOpenGL.PostProcess;
using SharpOpenGL.Transform;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using MouseButtonEventArgs = OpenTK.Windowing.Common.MouseButtonEventArgs;

namespace SharpOpenGL
{
    public class RenderingThreadWindow : GameWindow
    {
        protected Subject<Unit> GLContextCreated = new Subject<Unit>();
        protected Subject<Tuple<int,int>> WindowResized = new Subject<Tuple<int, int>>();

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

        public event EventHandler<KeyboardKeyEventArgs> OnKeyDownEvent;
        public event EventHandler<KeyboardKeyEventArgs> OnKeyUpEvent;

        public AutoResetEvent RenderingDone = new AutoResetEvent(false);

#region @Mouse Info
        private Vector2 LastMouseBtnDownPosition = new Vector2(0);
        private Vector2 LastMouseBtnMovePosition = new Vector2(0);
        private bool RightMouseBtnDown = false;
#endregion

        public RenderingThreadWindow(int width, int height)
        :base (new GameWindowSettings{IsMultiThreaded = false, UpdateFrequency = 500, RenderFrequency = 500}, NativeWindowSettings.Default)
        {
            GLContextCreated.Subscribe(_ =>
            {
                Sampler.OnGLContextCreated();
            });

            WindowResized.Subscribe((x) =>
            {
                CameraManager.Get().OnWindowResized(x.Item1, x.Item2);
            });
        }

        public int Width
        {
            get => this.ClientSize.X;
        }

        public int Height
        {
            get => this.ClientSize.Y;
        }

        protected override void OnLoad()
        {
            this.Title = "MyEngine";

            OnGLContextCreated += RenderResource.OnOpenGLContextCreated;

            GLContextCreated.OnNext(Unit.Default);

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

            OnGLContextCreated(this, new EventArgs());
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

            
            bInitialized = true;
        }


        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            OnKeyDownEvent?.Invoke(this, e);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (OnKeyUpEvent != null)
            {
                OnKeyUpEvent(this, e);
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            
        }

        protected override void OnMouseEnter()
        {
            base.OnMouseEnter();
        }

        protected override void OnMouseLeave()
        {
            base.OnMouseLeave();
        }
        //
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (RightMouseBtnDown)
            {
                if (Math.Abs(e.DeltaX) > 0)
                {
                    CameraManager.Get().CurrentCamera.RotateYaw(e.DeltaX * 0.01f);
                }

                if (Math.Abs(e.DeltaY) > 0)
                {
                    CameraManager.Get().CurrentCamera.RotatePitch(e.DeltaY * 0.01f);
                }
            }
        }
        //
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            
        }


        public void HandleKeyDownEvent(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Keys.F1)
            {
                CameraManager.Get().SwitchCamera();
            }
            else if (e.Key == Keys.F2)
            {
                CameraManager.Get().CurrentCamera.FOV += OpenTK.Mathematics.MathHelper.DegreesToRadians(1.0f);
            }
            else if (e.Key == Keys.F3)
            {
                CameraManager.Get().CurrentCamera.FOV -= OpenTK.Mathematics.MathHelper.DegreesToRadians(1.0f);
            }
            else if (e.Key == Keys.F6)
            {   
                
            }
            else if (e.Key == Keys.F4)
            {
                DebugDrawer.Get().IsGBufferDump = !DebugDrawer.Get().IsGBufferDump;
            }
            else if (e.Key == Keys.F5)
            {
                gbufferVisualize.ChangeVisualizeMode();
            }
        }

        protected override void OnUnload()
        {
            Engine.Get().RequestExit();
        }
        
        protected override void OnResize(ResizeEventArgs e)
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

            var newSize = new Tuple<int, int>(Width, Height);

            WindowResized.OnNext(newSize);

            ResizableManager.Get().ResizeEventHandler.OnNext(newSize);
            
            this.Title = string.Format("MyEngine({0}x{1})", Width, Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            RenderingThread.Get().ExecuteTimeSlice();

        }


        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            SharpOpenGL.Engine.Get().RequestExit();
        }


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Engine.Get().WaitForGameThread();

            this.MakeCurrent();
            GL.ClearColor(Color.BlueViolet);
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
                }
            );

            if (true)
            {
                gbufferVisualize.Render(renderGBuffer.GetColorAttachement, renderGBuffer.GetNormalAttachment, renderGBuffer.GetPositionAttachment, renderGBuffer.GetMotionAttachment);
                ScreenBlit.Blit(gbufferVisualize.OutputColorTex0, 0, 0, 2, 2);
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
            }

            SwapBuffers();
        }
    }
}
