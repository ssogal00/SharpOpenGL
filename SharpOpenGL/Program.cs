using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Core.Buffer;
using Core.OpenGLShader;
using Core.Camera;
using System.Drawing;
using SharpOpenGL.StaticMesh;
using Core.Texture;
using Core.Tickable;
using System.Runtime.Serialization.Formatters.Binary;

using TestShaderVertexAttributes = SharpOpenGL.BasicMaterial.VertexAttribute;
using TestShaderVS = SharpOpenGL.BasicMaterial;

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
        

        protected TestShaderVS.Transform Transform = new TestShaderVS.Transform();
        protected ShaderProgram ProgramObject = null;

        protected SharpOpenGL.BasicMaterial.BasicMaterial TestMaterial = null;

        protected ObjMesh Mesh = new ObjMesh();

        protected override void OnLoad(EventArgs e)
        {
            VSync = VSyncMode.Off;

            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.Enable(EnableCap.DepthTest);

            GL.ClearColor(System.Drawing.Color.White);            

            TestMaterial = new SharpOpenGL.BasicMaterial.BasicMaterial();            

            TestMaterial.Use();
            
            Mesh.Load("./ObjMesh/sponza2.obj", "./ObjMesh/sponzaPBR.mtl");
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.Enable(EnableCap.DepthTest);            

            TickableObjectManager.Tick(e.Time);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Transform.View = Camera.View;
            Transform.Proj = Camera.Proj;
            Transform.Model = Matrix4.Scale((float)0.1);
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

        

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);

            float fAspectRatio = Width / (float)Height;

            Camera.AspectRatio = fAspectRatio;
            Camera.FOV = MathHelper.PiOver6 ;
            Camera.Near = 1;
            Camera.Far = 10000;
            Camera.EyeLocation = new Vector3(5, 5, 5);
            Camera.DestLocation = new Vector3(5, 5, 5);
            Camera.LookAtLocation = new Vector3(0, 0, 0);                        

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
