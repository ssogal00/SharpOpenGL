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
using FBXWrapper;

using SharpOpenGL;
using Core.Buffer;
using Core.Camera;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SharpOpenGL.BasicMaterial;

namespace FBXImporter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var SdkWrapper = new FBXWrapper.FBXSDKWrapper();

            if (SdkWrapper.InitializeSDK())
            {
                TestParsedFbxMesh = SdkWrapper.ImportFBXMesh("Sample.FBX");
                MyFBXMesh = new FBXMesh(TestParsedFbxMesh);
            }
        }
        private void GLControlLoad(object sender, EventArgs e)
        {
            GL.ClearColor(System.Drawing.Color.Aqua);

            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.Enable(EnableCap.DepthTest);

            TestMaterial = new SharpOpenGL.BasicMaterial.BasicMaterial();
            TestMaterial.Use();

            // init uniform buffer
            TransformBuffer = new DynamicUniformBuffer();
            ColorBuffer = new DynamicUniformBuffer();
            if (MyFBXMesh != null)
            {
                MyFBXMesh.PrepareToDraw();
            }            
        }


        private void GLControlPaint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.Enable(EnableCap.DepthTest);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //ModelTransform.View = Camera.View;
            //ModelTransform.Proj = Camera.Proj;
            TestMaterial.SetTransformBlockData(ref ModelTransform);

            if(MyFBXMesh != null)
            {                
                MyFBXMesh.Draw();
            }
            
            GlControl.SwapBuffers();
        }

        private void GLControlResize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, GlControl.Size.Width, GlControl.Size.Height);

            float fAspectRatio = GlControl.Size.Width / (float) GlControl.Size.Height;

            Camera.AspectRatio = fAspectRatio;
            Camera.FOV = MathHelper.PiOver6;
            Camera.Near = 1;
            Camera.Far = 10000;
            Camera.EyeLocation = new Vector3(5, 5, 5);
            Camera.DestLocation = new Vector3(5, 5, 5);
            Camera.LookAtLocation = new Vector3(0, 0, 0);

            Camera.UpdateCameraDistance();
            Camera.UpdateProjMatrix();

            ModelTransform.Proj = Matrix4.CreatePerspectiveFieldOfView(Camera.FOV, fAspectRatio, Camera.Near, Camera.Far);
            ModelTransform.Model = Matrix4.CreateScale(0.03f);
            ModelTransform.View = Matrix4.LookAt(new Vector3(10, 0, 0), new Vector3(0, 0, 0), Vector3.UnitY);
                        
        }

        protected SharpOpenGL.BasicMaterial.BasicMaterial TestMaterial = null;
        protected DynamicUniformBuffer TransformBuffer = null;
        protected DynamicUniformBuffer ColorBuffer = null;
        protected Matrix4 ModelView = new Matrix4();
        protected Matrix4 Projection = new Matrix4();
        protected OrbitCamera Camera = new OrbitCamera();
        protected SharpOpenGL.BasicMaterial.Transform ModelTransform = new SharpOpenGL.BasicMaterial.Transform();

        protected ParsedFBXMesh TestParsedFbxMesh = null;
        protected FBXMesh MyFBXMesh = null;
    }    
}