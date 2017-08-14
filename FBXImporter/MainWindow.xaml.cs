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
using Core.Primitive;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SharpOpenGL.BasicMaterial;
using SharpOpenGL.SimpleMaterial;

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
                MyLine = new Core.Primitive.Line(new OpenTK.Vector3(-10,-10,-10), new OpenTK.Vector3(10,10,10));
                MyLineDrawer = new LineDrawer();                
                MyLineDrawer.AddLine(MyLine);                
            }
        }
        private void GLControlLoad(object sender, EventArgs e)
        {
            GL.ClearColor(System.Drawing.Color.Aqua);

            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.Enable(EnableCap.DepthTest);

            TestMaterial = new SharpOpenGL.BasicMaterial.BasicMaterial();
            Simple = new SharpOpenGL.SimpleMaterial.SimpleMaterial();
            TestMaterial.Use();

            if (MyFBXMesh != null)
            {
                MyFBXMesh.PrepareToDraw();
            }

            if(MyLineDrawer != null)
            {
                MyLineDrawer.Setup();
            }
        }


        private void GLControlPaint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.Enable(EnableCap.DepthTest);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Camera.UpdateCameraDistance();
            Camera.UpdateViewMatrix();
            Camera.UpdateProjMatrix();
            
            ModelTransform.View = Camera.View;
            ModelTransform.Proj = Camera.Proj;

            LineTransform.View = Camera.View;
            LineTransform.Proj = Camera.Proj;
            LineTransform.Model = Matrix4.CreateScale(0.014f);

            TestMaterial.Use();
            TestMaterial.SetTransformBlockData(ref ModelTransform);

            if(MyFBXMesh != null)
            {                
                //MyFBXMesh.Draw();
            }

            Simple.Use();
            Simple.SetTransformBlockData(ref LineTransform);
            MyLineDrawer.Draw();
            
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
            Camera.EyeLocation = new Vector3(-10, 0, 0);
            Camera.DestLocation = new Vector3(10, 0, 0);
            Camera.LookAtLocation = new Vector3(0, 0, 0);

            Camera.UpdateCameraDistance();
            Camera.UpdateViewMatrix();
            Camera.UpdateProjMatrix();

            ModelTransform.Proj = Matrix4.CreatePerspectiveFieldOfView(Camera.FOV, fAspectRatio, Camera.Near, Camera.Far);
            ModelTransform.Model = Matrix4.CreateScale(0.015f) ;            
        }

        protected SharpOpenGL.BasicMaterial.BasicMaterial TestMaterial = null;
        protected SharpOpenGL.SimpleMaterial.SimpleMaterial Simple = null;
        
        
        protected Matrix4 ModelView = new Matrix4();
        protected Matrix4 Projection = new Matrix4();
        protected OrbitCamera Camera = new OrbitCamera();

        protected SharpOpenGL.BasicMaterial.Transform ModelTransform = new SharpOpenGL.BasicMaterial.Transform();
        protected SharpOpenGL.SimpleMaterial.Transform LineTransform = new SharpOpenGL.SimpleMaterial.Transform();

        protected ParsedFBXMesh TestParsedFbxMesh = null;
        protected FBXMesh MyFBXMesh = null;
        protected Core.Primitive.Line MyLine = null;
        protected Core.Primitive.LineDrawer MyLineDrawer = null;
    }    
}