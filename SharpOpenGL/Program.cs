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

namespace SharpOpenGL
{
    public class MainWindow : GameWindow
    {
        protected Matrix4 ModelView = new Matrix4();
        protected Matrix4 Projection = new Matrix4();

        protected CameraBase CurrentCam = null;
        protected FreeCamera  FreeCam = new FreeCamera();
        protected OrbitCamera OribitCam = new OrbitCamera();

        protected GBufferDraw.Transform Transform = new GBufferDraw.Transform();

        protected ShaderProgram ProgramObject = null;

        protected RenderTarget testRenderTarget = new RenderTarget(1024, 768, 1);
        protected Core.MaterialBase.MaterialBase BaseTest = null;
        protected Core.MaterialBase.MaterialBase DefaultMaterial = null;
        protected PostProcess.BlurPostProcess Blur = new SharpOpenGL.PostProcess.BlurPostProcess();
        protected PostProcess.DeferredLight LightPostProcess = new DeferredLight();
        protected PostProcess.Skybox SkyboxPostProcess = new Skybox();

        protected StaticMeshAsset Mesh = null;
        protected Task<StaticMeshAsset> MeshLoadTask = null;
        protected Task<StaticMeshAsset> MeshLoadTask2 = null;
        protected GBuffer MyGBuffer = new GBuffer(1024, 768);

        public event EventHandler<EventArgs> OnResourceCreate;
        public event EventHandler<ScreenResizeEventArgs> OnWindowResize;

        public event EventHandler<OpenTK.Input.KeyboardKeyEventArgs> OnKeyEvent;

        protected BlitToScreen ScreenBlit = new BlitToScreen();

        protected Texture2D TestTexture = null;

        protected int mainThreadId;

        public int MainThreadId { get { return mainThreadId; } }

        protected override void OnLoad(EventArgs e)
        {
            mainThreadId = Thread.CurrentThread.ManagedThreadId;

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

            GL.ClearColor(System.Drawing.Color.DarkGray);            

            // register resource create event handler            
            OnResourceCreate += this.ResourceCreate;            
            OnResourceCreate += Sampler.OnResourceCreate;

            // resigter window resize event handler
            OnWindowResize += FreeCam.OnWindowResized;

            OnResourceCreate += RenderResource.OnOpenGLContextCreated;
            OnWindowResize += RenderResource.OnWindowResized;

            OnResourceCreate(this, e);

            ScreenBlit.SetGridSize(2, 2);

            OnKeyEvent += FreeCam.OnKeyDown;

            AssetManager.Get().DiscoverShader();

            Mesh = AssetManager.LoadAssetSync<StaticMeshAsset>("./Resources/Imported/StaticMesh/sherry3.staticmesh");
            MeshLoadTask = AssetManager.LoadAssetAsync<StaticMeshAsset>("./Resources/Imported/StaticMesh/sponza2.staticmesh");
            MeshLoadTask2 = AssetManager.LoadAssetAsync<StaticMeshAsset>("./Resources/Imported/StaticMesh/bunny.staticmesh");
        }

        protected void ResourceCreate(object sender, EventArgs e)
        {
            BaseTest = new SharpOpenGL.GBufferDraw.GBufferDraw();
            BaseTest.Setup();
            DefaultMaterial = new GBufferWithoutTexture.GBufferWithoutTexture();
        }       

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            //
            MainThreadQueue.Get().Execute();

            TickableObjectManager.Tick(e.Time);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Transform.View = FreeCam.View;
            Transform.Proj = FreeCam.Proj;

            // draw cubemap first
            SkyboxPostProcess.ViewMatrix = FreeCam.View;
            SkyboxPostProcess.Render();

            // 
            MyGBuffer.BindAndExecute(
            () =>
            {
                MyGBuffer.Clear();
            });

            SkyboxPostProcess.GetOutputTextureObject().Copy(MyGBuffer.GetColorAttachement);

            MyGBuffer.BindAndExecute(BaseTest, () =>
            {
                BaseTest.SetUniformBufferValue<SharpOpenGL.GBufferDraw.Transform>("Transform", ref Transform);
                Mesh.Draw(BaseTest);
            });

            LightPostProcess.Render(MyGBuffer.GetPositionAttachment, MyGBuffer.GetColorAttachement, MyGBuffer.GetNormalAttachment);            
            ScreenBlit.Blit(LightPostProcess.GetOutputTextureObject().GetColorAttachment0TextureObject(), 0, 0, 2, 2);

            SwapBuffers();
        }

        protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            OnKeyEvent(this, e);
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

            ScreenResizeEventArgs eventArgs = new ScreenResizeEventArgs();
            eventArgs.Width = Width;
            eventArgs.Height = Height;

            float fAspectRatio = Width / (float) Height;

            OnWindowResize(this, eventArgs);

            Transform.Proj = Matrix4.CreatePerspectiveFieldOfView(FreeCam.FOV, fAspectRatio, FreeCam.Near, FreeCam.Far);
            Transform.Model = Matrix4.CreateScale(1.0f);
            FreeCam.EyeLocation = Mesh.CenterVertex + new Vector3(Mesh.XExtent, 0, 0);
            Transform.View = Matrix4.LookAt(Mesh.MaxVertex, Mesh.CenterVertex, Vector3.UnitY);
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
