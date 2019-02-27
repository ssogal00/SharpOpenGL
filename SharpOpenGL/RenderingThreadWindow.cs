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
        protected BlurPostProcess blurPostProcess = new BlurPostProcess();
        protected DeferredLight lightPostProcess = new DeferredLight();
        protected GBufferVisualize gbufferVisualize = new GBufferVisualize();
        protected Bloom bloom = new Bloom();
        protected GBuffer renderGBuffer = new GBuffer(1024, 768);
        protected StaticMeshObject sponzamesh = null;
        protected bool bInitialized = false;

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
            // srgb
            //GL.Enable(EnableCap.FramebufferSrgb);

            ShaderManager.Get().CompileShaders();
            AssetManager.Get().ImportStaticMeshes();
            TextureManager.Get().ImportTextures();

            OnGLContextCreated(this, e);
            ScreenBlit.SetGridSize(2, 2);

            sponzamesh = new StaticMeshObject("sponza2.staticmesh");
            sponzamesh.IsMetallicOverride = sponzamesh.IsRoughnessOverride = true;
            sponzamesh.Metallic = 0.6f;
            sponzamesh.Roughness = 0.3f;

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
            else if (e.Key == Key.Tilde)
            {
                var testSceneObject = SceneObjectManager.Get().GetAnySceneObjectOf<StaticMeshObject>();
                if (testSceneObject != null)
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
          
            skyboxPostProcess.Render();

            renderGBuffer.BindAndExecute(
                () =>
                {
                    renderGBuffer.Clear();
                });

            skyboxPostProcess.GetOutputRenderTarget().Copy(renderGBuffer.GetColorAttachement);

            renderGBuffer.BindAndExecute
            (
                () =>
                {   
                    SceneObjectManager.Get().Draw();
                    DebugDrawer.Get().DebugDraw();
                }
            );


            if (DebugDrawer.Get().IsGBufferDump)
            {
                gbufferVisualize.Render(renderGBuffer.GetColorAttachement, renderGBuffer.GetNormalAttachment, renderGBuffer.GetPositionAttachment);
                ScreenBlit.Blit(gbufferVisualize.OutputRenderTarget.ColorAttachment0, 0, 0, 2, 2);
            }
            else
            {
                lightPostProcess.Render(renderGBuffer.GetColorAttachement, renderGBuffer.GetNormalAttachment, renderGBuffer.GetPositionAttachment);
                ScreenBlit.Blit(lightPostProcess.OutputRenderTarget.ColorAttachment0, 0, 0, 2, 2);
            }

            SwapBuffers();
        }
    }
}
