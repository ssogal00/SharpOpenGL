using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Buffer;
using Core.CustomEvent;
using Core.MaterialBase;
using Core.Primitive;
using Core.Texture;
using Core.Tickable;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using SharpOpenGL.Asset;
using SharpOpenGL.GBufferDraw;
using SharpOpenGL.PostProcess;
using SharpOpenGL.StaticMesh;

namespace SharpOpenGL
{
    public class RenderingThreadWindow : GameWindow
    {
        protected event EventHandler<EventArgs> OnGLContextCreated;
        protected event EventHandler<ScreenResizeEventArgs> OnWindowResize;
        protected BlitToScreen ScreenBlit = new BlitToScreen();

        protected Skybox skyboxPostProcess = new Skybox();
        protected GBuffer renderGBuffer = new GBuffer(1024, 768);
        protected StaticMeshAsset sponzamesh = null;
        protected bool bInitialized = false;

        protected GBufferDraw.ModelTransform ModelMatrix = new GBufferDraw.ModelTransform();
        protected GBufferDraw.CameraTransform Transform = new GBufferDraw.CameraTransform();

        protected MaterialBase GBufferMaterial = null;
        protected MaterialBase GridMaterial = null;
        protected MaterialBase WireframeMaterial = null;


        public event EventHandler<OpenTK.Input.KeyboardKeyEventArgs> OnKeyDownEvent;
        public event EventHandler<OpenTK.Input.KeyboardKeyEventArgs> OnKeyUpEvent;

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
            OnWindowResize += ResizableManager.Get().ResizeEventHandler;

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

            OnGLContextCreated(this, e);
            ScreenBlit.SetGridSize(2, 2);

            sponzamesh = AssetManager.LoadAssetSync<StaticMeshAsset>("sponza2.staticmesh");
            GBufferMaterial = ShaderManager.Get().GetMaterial("GBufferDraw");
            GridMaterial = ShaderManager.Get().GetMaterial("GridRenderMaterial");
            WireframeMaterial = ShaderManager.Get().GetMaterial("GBufferPNC");
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
        }

        protected override void OnUnload(EventArgs e)
        {
            Engine.Get().RequestExit();
        }
        
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);

            ScreenResizeEventArgs eventArgs = new ScreenResizeEventArgs
            {
                Width = Width,
                Height = Height
            };

            float fAspectRatio = Width / (float)Height;

            OnWindowResize(this, eventArgs);

            Transform.Proj = CameraManager.Get().CurrentCamera.Proj;
            Transform.View = CameraManager.Get().CurrentCamera.View;

            ModelMatrix.Model = Matrix4.CreateScale(1.500f);

            this.Title = string.Format("MyEngine({0}x{1})", Width, Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            RenderingThread.Get().ExecuteTimeSlice();
        }
        
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(Color.Brown);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

            if (bInitialized == false || Engine.Get().IsInitialized == false)
            {
                SwapBuffers();
                return;
            }

            Transform.View = CameraManager.Get().CurrentCameraView;
            Transform.Proj = CameraManager.Get().CurrentCameraProj;

            skyboxPostProcess.ModelMatrix = OpenTK.Matrix4.CreateScale(10.0f) * OpenTK.Matrix4.CreateTranslation(CameraManager.Get().CurrentCameraEye);
            skyboxPostProcess.ViewMatrix = CameraManager.Get().CurrentCameraView;
            skyboxPostProcess.ProjMatrix = CameraManager.Get().CurrentCameraProj;
            skyboxPostProcess.Render();

            renderGBuffer.BindAndExecute(
                () =>
                {
                    renderGBuffer.Clear();
                });

            skyboxPostProcess.GetOutputRenderTarget().Copy(renderGBuffer.GetColorAttachement);

            renderGBuffer.BindAndExecute
            (GBufferMaterial, () =>
            {
                GBufferMaterial.SetUniformBufferValue<ModelTransform>("ModelTransform", ref ModelMatrix);
                GBufferMaterial.SetUniformBufferValue<CameraTransform>("CameraTransform", ref Transform);
                sponzamesh.Draw(GBufferMaterial);

                GridDrawer.Get().Draw(GridMaterial);

                using (var dummy = new WireFrameMode())
                {
                    WireframeMaterial.BindAndExecute
                    (() =>
                        {   
                            WireframeMaterial.SetUniformBufferValue<SharpOpenGL.GBufferDraw.CameraTransform>("CameraTransform", ref Transform);
                            SceneObjectManager.Get().Draw(WireframeMaterial);
                        }
                    );
                }
            });

            ScreenBlit.Blit(renderGBuffer.GetColorAttachement, 0, 0, 2, 2);

            SwapBuffers();
        }
    }
}
