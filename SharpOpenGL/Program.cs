using Core.Buffer;
using Core.Tickable;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using SharpOpenGL.StaticMesh;
using System;
using System.Windows.Forms;
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
using OpenTK.Graphics;
using SharpOpenGL.Font;
using NativeWindow = OpenTK.NativeWindow;

namespace SharpOpenGL
{
    public class MainWindow : GameWindow
    {
        public MainWindow()
        :base (512,384)
        {
        }
        public MainWindow(int width, int height, GraphicsMode mode, string title, GameWindowFlags options,
            DisplayDevice device)
        : base(width, height,mode,title,options,device)
        {
        }

        public MainWindow(int width, int height, GraphicsMode mode, string title, GameWindowFlags options,
            DisplayDevice device, int major, int minor, GraphicsContextFlags flags, IGraphicsContext sharedContext,
            bool isSingleThreaded)
            : base(width, height, mode, title, options, device, major, minor, flags, sharedContext, isSingleThreaded)
        {
        }

        protected GBufferDraw.ModelTransform ModelMatrix = new GBufferDraw.ModelTransform();
        protected GBufferDraw.CameraTransform Transform = new GBufferDraw.CameraTransform();
        
        protected Core.MaterialBase.MaterialBase GBufferMaterial = null;
        protected Core.MaterialBase.MaterialBase DefaultMaterial = null;
        protected Core.MaterialBase.MaterialBase GBufferPNCMaterial = null;
        protected Core.MaterialBase.MaterialBase GridMaterial = null;
        protected Core.MaterialBase.MaterialBase ThreeDTextMaterial = null;

        //protected RenderTarget TestRenderTarget= new RenderTarget(1024, 768, 1,PixelInternalFormat.LuminanceAlpha, false);
        
        
        protected PostProcess.Skybox SkyboxPostProcess = new Skybox();
        
        protected Cylinder TestCyliner = new Cylinder(10, 10, 24);
        protected Cone TestCone = new Cone(10, 20, 12);
        protected Arrow TestArrow = new Arrow(10);
        protected ThreeAxis TestAxis = new ThreeAxis();
        protected ThreeDText TestText = null;
        
        protected StaticMeshAsset Mesh = null;
        protected StaticMeshAsset Sphere = null;
        protected Task<StaticMeshAsset> MeshLoadTask = null;
        protected Task<StaticMeshAsset> MeshLoadTask2 = null;
        protected GBuffer MyGBuffer = new GBuffer(1024,768);

        public event EventHandler<EventArgs> OnResourceCreate;
        public event EventHandler<ScreenResizeEventArgs> OnWindowResize;

        public event EventHandler<OpenTK.Input.KeyboardKeyEventArgs> OnKeyDownEvent;
        public event EventHandler<OpenTK.Input.KeyboardKeyEventArgs> OnKeyUpEvent;

        protected BlitToScreen ScreenBlit = new BlitToScreen();

        protected Texture2D TestTexture = null;

        protected int mainThreadId;

        public int MainThreadId { get { return mainThreadId; } }

        RenderingThread renderingThread = new RenderingThread();
        Thread renderThread = null;

        protected override void OnUnload(EventArgs e)
        {
            //renderingThread.RequestExit();
            //renderThread.Join();
        }

        protected override void OnLoad(EventArgs e)
        {
            /*renderThread = new Thread(renderingThread.Run);
            renderThread.Priority = ThreadPriority.AboveNormal;
            renderThread.Name = "RenderingThread";
            renderThread.Start();*/

            mainThreadId = Thread.CurrentThread.ManagedThreadId;

            OpenGLContext.Get().SetRenderingWindow(this);
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
            OnWindowResize += CameraManager.Get().OnWindowResized;

            OnResourceCreate += RenderResource.OnOpenGLContextCreated;
            OnWindowResize += ResizableManager.Get().ResizeEventHandler;

            AssetManager.Get().ImportStaticMeshes();

            OnResourceCreate(this, e);

            ScreenBlit.SetGridSize(2, 2);

            OnKeyDownEvent += CameraManager.Get().OnKeyDown;
            OnKeyUpEvent += CameraManager.Get().OnKeyUp;

            OnKeyDownEvent += this.HandleKeyDownEvent;

            Mesh = AssetManager.LoadAssetSync<StaticMeshAsset>("./Resources/Imported/StaticMesh/sponza2.staticmesh");
            Sphere = AssetManager.LoadAssetSync<StaticMeshAsset>("./Resources/Imported/StaticMesh/sphere3.staticmesh");
            GBufferMaterial = ShaderManager.Get().GetMaterial("GBufferDraw");
            DefaultMaterial = ShaderManager.Get().GetMaterial("GBufferWithoutTexture");
            GBufferPNCMaterial = ShaderManager.Get().GetMaterial("GBufferPNC");
            GridMaterial = ShaderManager.Get().GetMaterial("GridRenderMaterial");
            ThreeDTextMaterial = ShaderManager.Get().GetMaterial("ThreeDTextRenderMaterial");

            FontManager.Get().Initialize();

            TestText = new ThreeDText("Hello World");
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

        protected void CaptureStaticMesh()
        {
            StaticMeshCapture.Get().Capture("./Resources/Imported/StaticMesh/pop.staticmesh");
        }

        protected void SwitchCameraMode()
        {
            // 
            CameraManager.Get().SwitchCamera();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            //base.OnUpdateFrame(e);

            if(this.WindowState == OpenTK.WindowState.Minimized)
            {
                return;
            }

            //
            MainThreadQueue.Get().Execute();

            TickableObjectManager.Tick(e.Time);

            GL.ClearColor (Color.White);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            Transform.View = CameraManager.Get().CurrentCameraView;
            Transform.Proj = CameraManager.Get().CurrentCameraProj;

            // draw cubemap first
            SkyboxPostProcess.Render();

            // 
            MyGBuffer.BindAndExecute(
            () =>
            {
                MyGBuffer.Clear();
            });

            SkyboxPostProcess.GetOutputRenderTarget().Copy(MyGBuffer.GetColorAttachement);
            
            ScreenBlit.Blit(MyGBuffer.GetColorAttachement, 0, 0, 2, 2);
            //LightPostProcess.Render(MyGBuffer.GetPositionAttachment, MyGBuffer.GetColorAttachement, MyGBuffer.GetNormalAttachment);
            //ScreenBlit.Blit(LightPostProcess.GetOutputRenderTarget().GetColorAttachment0TextureObject(), 0, 0, 2, 2);

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
                if (ConsoleCommandManager.Get().IsActive == false)
                {
                    CameraManager.Get().CurrentCamera.ToggleLock();
                }
                return;
            }

            if(e.Key == Key.F1)
            {
                CameraManager.Get().SwitchCamera();
            }
            else if(e.Key == Key.F2)
            {
                CameraManager.Get().CurrentCamera.FOV += OpenTK.MathHelper.DegreesToRadians(1.0f);
            }
            else if(e.Key == Key.F3)
            {
                CameraManager.Get().CurrentCamera.FOV -= OpenTK.MathHelper.DegreesToRadians(1.0f);
            }
            else if(e.Key == Key.F5)
            {
                CaptureStaticMesh();
            }
            else if (e.Key == Key.Tilde)
            {
                ConsoleCommandManager.Get().ToggleActive();
                CameraManager.Get().CurrentCamera.ToggleLock();
            }
        }

        protected override void OnKeyUp(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (OnKeyUpEvent != null) OnKeyUpEvent(this, e);
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

            Transform.Proj = CameraManager.Get().CurrentCamera.Proj;
            Transform.View = CameraManager.Get().CurrentCamera.View;

            ModelMatrix.Model = Matrix4.CreateScale(1.500f);
            this.Title = string.Format("MyEngine({0}x{1})", Width, Height);
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
            var renderThread = new Thread(RenderingThread.Get().Run);
            renderThread.Priority = ThreadPriority.AboveNormal;
            renderThread.Name = "RenderingThread";
            renderThread.Start();

            var uiThread = new Thread(UIThread.Get().Run);
            uiThread.Priority = ThreadPriority.BelowNormal;
            uiThread.Name = "UIThread";
            uiThread.SetApartmentState(ApartmentState.STA);
            uiThread.Start();

            Engine.Get().Initialize();

            while (true)
            {
                if (Engine.Get().IsRequestExit)
                {
                    RenderingThread.Get().RequestExit();
                    UIThread.Get().RequestExit();
                    break;
                }
                Engine.Get().Tick();
                Thread.Sleep(1000/60);
            }

            renderThread.Join();
            uiThread.Join();
        }
    }
}
