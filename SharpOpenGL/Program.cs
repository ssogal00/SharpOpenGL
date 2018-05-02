using Core.Buffer;
using Core.Camera;
using Core.OpenGLShader;
using Core.Tickable;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using SharpOpenGL.StaticMesh;
using System;
using System.Threading.Tasks;
using Core.CustomEvent;
using Core.Texture;
using Core;
using SharpOpenGL.GBufferDraw;
using ZeroFormatter.Formatters;
using Core.CustomSerialize;



namespace SharpOpenGL
{
    public class MainWindow : GameWindow
    {
        protected Matrix4 ModelView = new Matrix4();
        protected Matrix4 Projection = new Matrix4();
        
        protected FreeCamera FreeCam = new FreeCamera();        
        protected SharpOpenGL.GBufferDraw.Transform Transform = new SharpOpenGL.GBufferDraw.Transform();

        protected ShaderProgram ProgramObject = null;

        protected SharpOpenGL.BasicMaterial.BasicMaterial TestMaterial = null;
        protected SharpOpenGL.GBufferDraw.GBufferDraw GBufferMaterial = null;
        protected Core.MaterialBase.MaterialBase BaseTest = null;
        protected SharpOpenGL.PostProcess.BlurPostProcess Blur = null;
        protected SharpOpenGL.PostProcess.DeferredLight LightPostProcess = null;

        protected ObjMesh Mesh = new ObjMesh();
        protected GBuffer MyGBuffer = new GBuffer(1024, 768);        


        private Task<ObjMesh> MeshLoadTask = null;

        public event EventHandler<EventArgs> OnResourceCreate;
        public event EventHandler<ScreenResizeEventArgs> OnWindowResize;

        public event EventHandler<OpenTK.Input.KeyboardKeyEventArgs> OnKeyEvent;

        protected BlitToScreen ScreenBlit = new BlitToScreen();

        protected Texture2D TestTexture = null;

        protected OpenGLContext openglContext = null;

        protected override void OnLoad(EventArgs e)
        {
            openglContext = new OpenGLContext(this);

            Formatter<DefaultResolver, OpenTK.Vector3>.Register(new Vector3Formatter<DefaultResolver>());
            Formatter<DefaultResolver, OpenTK.Vector2>.Register(new Vector2Formatter<DefaultResolver>());
            Formatter<DefaultResolver, OpenTK.Vector4>.Register(new Vector4Formatter<DefaultResolver>());

            VSync = VSyncMode.Off;

            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.Enable(EnableCap.DepthTest);

            GL.ClearColor(System.Drawing.Color.White);            

            // register resource create event handler            
            OnResourceCreate += this.ResourceCreate;
            OnResourceCreate += MyGBuffer.OnResourceCreate;
            OnResourceCreate += Sampler.OnResourceCreate;

            // resigter window resize event handler
            //OnWindowResize += Camera.OnWindowResized;
            OnWindowResize += FreeCam.OnWindowResized;
            OnWindowResize += MyGBuffer.OnWindowResized;
            
            OnResourceCreate(this, e);

            // OnKeyEvent += Camera.OnKeyDown;
            OnKeyEvent += FreeCam.OnKeyDown;

            OnWindowResize += Blur.OnWindowResized;
            OnWindowResize += LightPostProcess.OnWindowResized;

            //MeshLoadTask = ObjMesh.ImportMeshAsync("./Resources/ObjMesh/sponza2.obj", "./Resources/ObjMesh/sponzaPBR.mtl");
            MeshLoadTask = ObjMesh.LoadSerializedAsync("sponza.serialized");

            var a = openglContext.GetMaxElementsVertices();
            var b =  openglContext.GetMaxElementsIndices();
        }

        protected void ResourceCreate(object sender, EventArgs e)
        {
            // init screen blit
            ScreenBlit.OnResourceCreate(sender, e);
            ScreenBlit.SetGridSize(3, 3);            

            Blur = new SharpOpenGL.PostProcess.BlurPostProcess();
            Blur.OnResourceCreate(this, e);

            LightPostProcess = new SharpOpenGL.PostProcess.DeferredLight();
            LightPostProcess.OnResourceCreate(this, e);

            //            
            BaseTest = new SharpOpenGL.GBufferDraw.GBufferDraw();
            BaseTest.Setup();
        }       

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if(MeshLoadTask != null)
            {
                if(MeshLoadTask.IsCompleted)
                {
                    Mesh = MeshLoadTask.Result;
                    //Mesh.SaveSerialized("sponza.serialized");
                    Mesh.PrepareToDraw();
                    Mesh.LoadTextures();
                    MeshLoadTask = null;
                }
                return;
            }

            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.Enable(EnableCap.DepthTest);            

            TickableObjectManager.Tick(e.Time);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Transform.View = FreeCam.View;
            Transform.Proj = FreeCam.Proj;
            
            MyGBuffer.Bind();
            MyGBuffer.Clear();
            MyGBuffer.PrepareToDraw();            
            BaseTest.Setup();
            BaseTest.SetUniformBufferValue<SharpOpenGL.GBufferDraw.Transform>("Transform", ref Transform);

            

            Mesh.Draw(BaseTest);
            MyGBuffer.Unbind();

            LightPostProcess.Render(MyGBuffer.GetPositionAttachment, MyGBuffer.GetColorAttachement, MyGBuffer.GetNormalAttachment);

            //Blur.Render(MyGBuffer.GetColorAttachement);
            ScreenBlit.Blit(MyGBuffer.NormalBufferObject, 2, 2, 1, 1);
            ScreenBlit.Blit(LightPostProcess.GetOutputTextureObject().GetColorAttachment0TextureObject(), 0,0,3,3);
            //ScreenBlit.Blit(MyGBuffer.ColorBufferObject, 0, 0, 3, 3);

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
            Transform.Model = Matrix4.CreateScale(0.06f);
            Transform.View = Matrix4.LookAt(new Vector3(10, 0, 0), new Vector3(0, 0, 0), Vector3.UnitY);

            
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
                example.Run(120);
            }
        }
    }
}
