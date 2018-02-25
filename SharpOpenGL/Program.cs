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
using TestShaderVertexAttributes = SharpOpenGL.BasicMaterial.VertexAttribute;
using TestShaderVS = SharpOpenGL.BasicMaterial;
using Core.CustomEvent;

namespace SharpOpenGL
{
    public class MainWindow : GameWindow
    {
        TestShaderVertexAttributes[] Vertices = new TestShaderVertexAttributes[]
        {
            new TestShaderVertexAttributes{ VertexPosition = new Vector3(-1.0f,-1.0f, 1.0f)},
            new TestShaderVertexAttributes{ VertexPosition = new Vector3( 1.0f,-1.0f, 1.0f)},
            new TestShaderVertexAttributes{ VertexPosition = new Vector3( 1.0f, 1.0f, 1.0f)},
            new TestShaderVertexAttributes{ VertexPosition = new Vector3(-1.0f, 1.0f, 1.0f)},
            new TestShaderVertexAttributes{ VertexPosition = new Vector3(-1.0f,-1.0f,-1.0f)},
            new TestShaderVertexAttributes{ VertexPosition = new Vector3( 1.0f,-1.0f,-1.0f)},
            new TestShaderVertexAttributes{ VertexPosition = new Vector3( 1.0f, 1.0f,-1.0f)},
            new TestShaderVertexAttributes{ VertexPosition = new Vector3(-1.0f, 1.0f,-1.0f)},
        };

        ushort[] CubeElements = new ushort[]
        {
            0, 1, 2, 2, 3, 0, // front face
            3, 2, 6, 6, 7, 3, // top face
            7, 6, 5, 5, 4, 7, // back face
            4, 0, 3, 3, 7, 4, // left face
            0, 1, 5, 5, 4, 0, // bottom face
            1, 5, 6, 6, 2, 1, // right face
        };


        protected Matrix4 ModelView = new Matrix4();
        protected Matrix4 Projection = new Matrix4();

        protected OrbitCamera Camera = new OrbitCamera();
        protected FreeCamera ActiveCamera = new FreeCamera();
        protected DynamicUniformBuffer TransformBuffer = null;
        protected DynamicUniformBuffer ColorBuffer = null;

        protected TestShaderVS.Transform Transform = new TestShaderVS.Transform();
        protected ShaderProgram ProgramObject = null;

        protected SharpOpenGL.BasicMaterial.BasicMaterial TestMaterial = null;

        protected ObjMesh Mesh = new ObjMesh();

        protected GBuffer MyGBuffer = null;

        private Task<ObjMesh> MeshLoadTask = null;

        public event EventHandler<EventArgs> OnResourceCreate;
        public event EventHandler<ScreenResizeEventArgs> OnWindowResize;

        protected BlitToScreen ScreenBlit = new BlitToScreen();

        protected override void OnLoad(EventArgs e)
        {
            VSync = VSyncMode.Off;

            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.Enable(EnableCap.DepthTest);

            GL.ClearColor(System.Drawing.Color.White);            

            TestMaterial = new SharpOpenGL.BasicMaterial.BasicMaterial();

            TestMaterial.Use();

            OnResourceCreate += ScreenBlit.OnResourceCreate;
            OnWindowResize += Camera.OnWindowResized;
            
            MeshLoadTask = ObjMesh.LoadMeshAsync("./Resources/ObjMesh/sponza2.obj", "./Resources/ObjMesh/sponzaPBR.mtl");
        }



        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);            

            if(MeshLoadTask != null)
            {
                if(MeshLoadTask.IsCompleted)
                {
                    Mesh = MeshLoadTask.Result;
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

            Transform.View = Camera.View;
            Transform.Proj = Camera.Proj;                  
            TestMaterial.SetTransformBlockData(ref Transform);

            Mesh.Draw(TestMaterial);

            SwapBuffers();
        }

        protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            if(e.Key == OpenTK.Input.Key.W)
            {
                Camera.MoveForward(3);
            }
            else if(e.Key == OpenTK.Input.Key.S)
            {
                Camera.MoveForward(-3);
            }
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

            Transform.Proj = Matrix4.CreatePerspectiveFieldOfView(Camera.FOV, fAspectRatio, Camera.Near, Camera.Far);
            Transform.Model = Matrix4.CreateScale(0.03f);
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
