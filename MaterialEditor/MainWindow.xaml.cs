using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using Core;
using Core.OpenGLShader;
using Core.Buffer;
using Core.Camera;
using System.IO;
using System.Runtime.InteropServices;
using SharpOpenGL.StaticMesh;

namespace MaterialEditor
{
    [StructLayout(LayoutKind.Explicit, Size = 192)]
    public struct Transform
    {
        [FieldOffset(0)]
        public OpenTK.Matrix4 Model;
        [FieldOffset(64)]
        public OpenTK.Matrix4 View;
        [FieldOffset(128)]
        public OpenTK.Matrix4 Proj;
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected ShaderProgram ProgramObject;

        protected OrbitCamera Camera = new OrbitCamera();
        protected DynamicUniformBuffer TransformBuffer = null;
        protected DynamicUniformBuffer ColorBuffer = null;
        protected Transform TransformData = new Transform();


        protected ObjMesh Mesh = new ObjMesh();

        private void GLControlLoad(object sender, EventArgs e)
        {
            GL.ClearColor(System.Drawing.Color.Aqua);            

            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.Enable(EnableCap.DepthTest);           

            VertexShader vs = new VertexShader();

            var dir = Directory.GetCurrentDirectory();

            var content = File.ReadAllText("..\\..\\Shader\\TestShader.vs");

            vs.CompileShader(content);

            FragmentShader fs = new FragmentShader();

            var fscontent = File.ReadAllText("..\\..\\Shader\\TestShader.fs");

            fs.CompileShader(fscontent);

            ProgramObject = new ShaderProgram();

            ProgramObject.AttachShader(vs);
            ProgramObject.AttachShader(fs);

            String result;
            if (ProgramObject.LinkProgram(out result))
            {
                ProgramObject.UseProgram();


                //Mesh.Load("..\\..\\ObjMesh\\sponza2.obj", "..\\..\\ObjMesh\\sponzaPBR.mtl");
                Mesh.Load("../../ObjMesh/pop.obj", "../../ObjMesh/pop.mtl");
            }

        }

        private void GLControlPaint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.Enable(EnableCap.DepthTest);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);            

           // TransformData.View = Camera.View;
            TransformData.Proj = Camera.Proj;
            /* Transform.Model = Matrix4.Identity * Matrix4.CreateScale(0.3f);*/

            TransformBuffer.BindBufferBase(0);
            TransformBuffer.BufferData<Transform>(ref TransformData);

            Mesh.Draw(ProgramObject);            

            mGlControl.SwapBuffers();
        }

        private void GLControlResize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, mGlControl.Size.Width, mGlControl.Size.Height);


            // init uniform buffer
            TransformBuffer = new DynamicUniformBuffer();
            ColorBuffer = new DynamicUniformBuffer();

            float fAspectRatio = mGlControl.Size.Width / (float)mGlControl.Size.Height;

            Camera.AspectRatio = fAspectRatio;
            Camera.FOV = MathHelper.PiOver6;
            Camera.Near = 1;
            Camera.Far = 10000;
            Camera.EyeLocation = new Vector3(5, 5, 5);
            Camera.DestLocation = new Vector3(5, 5, 5);
            Camera.LookAtLocation = new Vector3(0, 0, 0);

            Camera.UpdateCameraDistance();
            Camera.UpdateProjMatrix();

            TransformData.Proj = Matrix4.CreatePerspectiveFieldOfView(Camera.FOV, fAspectRatio, Camera.Near, Camera.Far);
            TransformData.Model = Matrix4.CreateScale(0.03f);
            TransformData.View = Matrix4.LookAt(new Vector3(10, 0, 0), new Vector3(0, 0, 0), Vector3.UnitY);

            TransformBuffer.Bind();
            TransformBuffer.BufferData<Transform>(ref TransformData);

            ColorBuffer.Bind();
            var greenColor = new Vector3(0, 1, 0);
            ColorBuffer.BufferData<Vector3>(ref greenColor);
        }
    }


    
}
