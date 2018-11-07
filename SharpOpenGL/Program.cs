using Core.Buffer;
using Core.Camera;
using Core.OpenGLShader;
using Core.Tickable;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using SharpOpenGL.StaticMesh;
using System;
using Core.CustomEvent;
using Core.Texture;
using Core;
using ZeroFormatter.Formatters;
using Core.CustomSerialize;
using SharpOpenGL.Asset;
using SharpOpenGL.PostProcess;
using System.Threading;
using System.Threading.Tasks;
using Core.MaterialBase;
using SharpOpenGL.GBufferDraw;
using System.Drawing;
using Core.Primitive;
using SharpOpenGL.Font;

namespace SharpOpenGL
{
    public class MainWindow : GameWindow
    {
        protected CameraBase CurrentCam = null;
        protected FreeCamera  FreeCam = new FreeCamera();
        protected OrbitCamera OrbitCam = new OrbitCamera();

        protected GBufferDraw.ModelTransform ModelMatrix = new GBufferDraw.ModelTransform();
        protected GBufferDraw.CameraTransform Transform = new GBufferDraw.CameraTransform();

        protected ShaderProgram ProgramObject = null;

        protected RenderTarget testRenderTarget = new RenderTarget(1024, 768, 1);
        protected Core.MaterialBase.MaterialBase GBufferMaterial = null;
        protected Core.MaterialBase.MaterialBase DefaultMaterial = null;
        protected Core.MaterialBase.MaterialBase GBufferPNCMaterial = null;
        protected PostProcess.BlurPostProcess Blur = new SharpOpenGL.PostProcess.BlurPostProcess();
        protected PostProcess.DeferredLight LightPostProcess = new DeferredLight();
        protected DepthVisualize DepthVisualizePostProcess = new DepthVisualize();
        protected PostProcess.Skybox SkyboxPostProcess = new Skybox();

        protected string consoleCommandString = ">";
        private bool consoleCommandInputMode = false;

        protected Cylinder TestCyliner = new Cylinder(10, 10, 24);
        protected Cone TestCone = new Cone(10, 20, 12);
        protected Sphere TestSphere = new Core.Primitive.Sphere(10, 20, 20);
        protected Torus TestTorus = new Core.Primitive.Torus(10, 2, 15);
        protected Arrow TestArrow = new Arrow(10);
        protected ThreeAxis TestAxis = new ThreeAxis();
        

        protected StaticMeshAsset Mesh = null;
        protected StaticMeshAsset Sphere = null;
        protected Task<StaticMeshAsset> MeshLoadTask = null;
        protected Task<StaticMeshAsset> MeshLoadTask2 = null;
        protected GBuffer MyGBuffer = new GBuffer(1024,768);
        protected MultisampleGBuffer multisampleGBuffer = new MultisampleGBuffer(1024,768);

        public event EventHandler<EventArgs> OnResourceCreate;
        public event EventHandler<ScreenResizeEventArgs> OnWindowResize;

        public event EventHandler<OpenTK.Input.KeyboardKeyEventArgs> OnKeyDownEvent;
        public event EventHandler<OpenTK.Input.KeyboardKeyEventArgs> OnKeyUpEvent;

        protected BlitToScreen ScreenBlit = new BlitToScreen();

        protected Texture2D TestTexture = null;

        protected int mainThreadId;

        public int MainThreadId { get { return mainThreadId; } }

        protected override void OnLoad(EventArgs e)
        {
            mainThreadId = Thread.CurrentThread.ManagedThreadId;

            CurrentCam = FreeCam;

            OpenGLContext.Get().SetGameWindow(this);
            OpenGLContext.Get().SetMainThreadId(MainThreadId);

            Formatter<DefaultResolver, OpenTK.Vector3>.Register(new Vector3Formatter<DefaultResolver>());
            Formatter<DefaultResolver, OpenTK.Vector2>.Register(new Vector2Formatter<DefaultResolver>());
            Formatter<DefaultResolver, OpenTK.Vector4>.Register(new Vector4Formatter<DefaultResolver>());

            VSync = VSyncMode.Off;
            
            GL.CullFace(CullFaceMode.Back);            
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.TextureCubeMap);
            GL.Enable(EnableCap.TextureCubeMapSeamless);

            GL.ClearColor(System.Drawing.Color.DarkGray);            

            // register resource create event handler            
            OnResourceCreate += this.ResourceCreate;            
            OnResourceCreate += Sampler.OnResourceCreate;

            // resigter window resize event handler
            OnWindowResize += FreeCam.OnWindowResized;

            OnResourceCreate += RenderResource.OnOpenGLContextCreated;
            OnWindowResize += ResizableManager.Get().ResizeEventHandler;

            AssetManager.Get().DiscoverShader();

            OnResourceCreate(this, e);

            ScreenBlit.SetGridSize(2, 2);

            OnKeyDownEvent += FreeCam.OnKeyDown;
            OnKeyDownEvent += this.HandleKeyDownEvent;
            
            OnKeyUpEvent += FreeCam.OnKeyUp;            

            Mesh = AssetManager.LoadAssetSync<StaticMeshAsset>("./Resources/Imported/StaticMesh/sponza2.staticmesh");
            Sphere = AssetManager.LoadAssetSync<StaticMeshAsset>("./Resources/Imported/StaticMesh/sphere3.staticmesh");
            GBufferMaterial = AssetManager.LoadAssetSync<MaterialBase>("GBufferDraw");
            DefaultMaterial = AssetManager.LoadAssetSync<MaterialBase>("GBufferWithoutTexture");
            GBufferPNCMaterial = AssetManager.LoadAssetSync<MaterialBase>("GBufferPNC");

            FontManager.Get().Initialize();
        }

        protected void ResourceCreate(object sender, EventArgs e)
        {
        }

        protected void ScreenCaptureGBuffer()
        {
            var colorData = MyGBuffer.GetColorAttachement.GetTexImage();
            var width = MyGBuffer.GetColorAttachement.Width;
            var height = MyGBuffer.GetColorAttachement.Height;
            FreeImageHelper.SaveAsBmp(ref colorData, width, height, "ColorBuffer.bmp");

            var normalData = MyGBuffer.GetNormalAttachment.GetTexImage();
            FreeImageHelper.SaveAsBmp(ref normalData, width, height, "NormalBuffer.bmp");
        }

        protected void SwitchCameraMode()
        {
            // 
            if(CurrentCam == FreeCam)
            {
                OrbitCam.DestLocation = OrbitCam.EyeLocation = FreeCam.EyeLocation;
                OrbitCam.LookAtLocation = FreeCam.EyeLocation + FreeCam.GetLookAtDir() * 50.0f;
                OrbitCam.SetDistanceToLookAt(50.0f);
                OrbitCam.AspectRatio = FreeCam.AspectRatio;
                OrbitCam.FOV = FreeCam.FOV;
                CurrentCam = OrbitCam;

                OnKeyDownEvent -= FreeCam.OnKeyDown;
                OnKeyDownEvent += OrbitCam.OnKeyDown;
            }
            else
            {
                CurrentCam = FreeCam;

                OnKeyDownEvent -= OrbitCam.OnKeyDown;
                OnKeyDownEvent += FreeCam.OnKeyDown;
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if(this.WindowState == OpenTK.WindowState.Minimized)
            {
                return;
            }

            //
            MainThreadQueue.Get().Execute();

            TickableObjectManager.Tick(e.Time);

            GL.ClearColor (Color.White);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            Transform.View = CurrentCam.View;
            Transform.Proj = CurrentCam.Proj;

            // draw cubemap first
            SkyboxPostProcess.ModelMatrix = OpenTK.Matrix4.CreateScale(10.0f) * OpenTK.Matrix4.CreateTranslation(CurrentCam.EyeLocation);
            SkyboxPostProcess.ViewMatrix = CurrentCam.View;
            SkyboxPostProcess.ProjMatrix = CurrentCam.Proj;
            SkyboxPostProcess.Render();

            // 
            MyGBuffer.BindAndExecute(
            () =>
            {
                MyGBuffer.Clear();
            });

            SkyboxPostProcess.GetOutputRenderTarget().Copy(MyGBuffer.GetColorAttachement);

            MyGBuffer.BindAndExecute(GBufferMaterial, () =>
            {
                GBufferMaterial.SetUniformBufferValue<ModelTransform>("ModelTransform", ref ModelMatrix);
                GBufferMaterial.SetUniformBufferValue<SharpOpenGL.GBufferDraw.CameraTransform>("CameraTransform", ref Transform);
                Mesh.Draw(GBufferMaterial);
                
                if (ConsoleCommandManager.Get().IsActive)
                {
                    FontManager.Get().RenderText(10, 100, ConsoleCommandManager.Get().ConsoleCommandString);
                }

                if (CurrentCam == OrbitCam)
                {
                    using (var dummy = new WireFrameMode())
                    {
                        GBufferPNCMaterial.BindAndExecute
                        (() =>
                        {
                            TestAxis.ParentMatrix = Matrix4.CreateTranslation(CurrentCam.LookAtLocation);
                            TestAxis.Scale = 0.4f;
                            GBufferPNCMaterial.SetUniformBufferValue<SharpOpenGL.GBufferDraw.CameraTransform>("CameraTransform", ref Transform);
                            TestAxis.Draw(GBufferPNCMaterial);
                        }
                        );
                    }
                }
            });

            DepthVisualizePostProcess.Render(MyGBuffer.GetDepthAttachment);
            LightPostProcess.Render(MyGBuffer.GetPositionAttachment, MyGBuffer.GetColorAttachement, MyGBuffer.GetNormalAttachment);
            ScreenBlit.Blit(LightPostProcess.GetOutputRenderTarget().GetColorAttachment0TextureObject(), 0, 0, 2, 2);
            

            SwapBuffers();
        }

        protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (OnKeyDownEvent != null) OnKeyDownEvent(this, e);
        }

        public void HandleKeyDownEvent(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (ConsoleCommandManager.Get().IsActive)
            {
                ConsoleCommandManager.Get().OnKeyDown(e);
                consoleCommandString = ConsoleCommandManager.Get().ConsoleCommandString;
                if (ConsoleCommandManager.Get().IsActive == false)
                {
                    CurrentCam.ToggleLock();
                }
                return;
            }

            if(e.Key == Key.F1)
            {
                SwitchCameraMode();
            }
            else if(e.Key == Key.F2)
            {
                CurrentCam.FOV += OpenTK.MathHelper.DegreesToRadians(1.0f);
            }
            else if(e.Key == Key.F3)
            {
                CurrentCam.FOV -= OpenTK.MathHelper.DegreesToRadians(1.0f);
            }
            else if(e.Key == Key.F5)
            {
                ScreenCaptureGBuffer();
            }
            else if (e.Key == Key.Tilde)
            {
                ConsoleCommandManager.Get().ToggleActive();

                if (CurrentCam != null)
                {
                    CurrentCam.ToggleLock();
                }
            }
        }

        protected override void OnKeyUp(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (OnKeyUpEvent != null) OnKeyUpEvent(this, e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
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

            float fAspectRatio = Width / (float) Height;

            OnWindowResize(this, eventArgs);

            Transform.Proj = Matrix4.CreatePerspectiveFieldOfView(FreeCam.FOV, fAspectRatio, FreeCam.Near, FreeCam.Far);
            Transform.View = Matrix4.LookAt(Mesh.MaxVertex, Mesh.CenterVertex, Vector3.UnitY);

            ModelMatrix.Model = Matrix4.CreateScale(1.500f);
            FreeCam.Destination = FreeCam.EyeLocation = Mesh.CenterVertex + new Vector3(Mesh.XExtent, 0, 0);
        }
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (MainWindow example = new MainWindow())            
            {   
                example.Run(200);
            }
        }
    }
}
