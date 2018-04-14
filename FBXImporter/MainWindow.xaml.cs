using System;
using System.Collections.Generic;

using System.Windows;
using System.Windows.Threading;
using FBXWrapper;

using SharpOpenGL;
using Core.Buffer;
using Core.Camera;
using Core.Primitive;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SharpOpenGL.BasicMaterial;
using SharpOpenGL.SimpleMaterial;
using SharpOpenGL.StaticMesh;

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
                TestParsedFbxMesh = SdkWrapper.ImportFBXMesh("FBXSample.FBX");
                TestAnimStack = SdkWrapper.ImportFBXAnimation("ActiveSkill01.FBX");
                TestAnimation = new ParsedFBXAnimation(TestParsedFbxMesh, TestAnimStack);                

                MyFBXMesh = new FBXMesh();                
            }

            timer.Tick += new EventHandler(Tick);
            timer.Interval = TimeSpan.FromMilliseconds(1);
        }

        private void Tick(object sender, EventArgs e)
        {
            nCurrentAnimIndex++;
            nCurrentAnimIndex = nCurrentAnimIndex % AnimList.Count;
            GlControl.Invalidate();            
        }

    private void GLControlLoad(object sender, EventArgs e)
        {
            GL.ClearColor(System.Drawing.Color.LightGray);

            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.Enable(EnableCap.DepthTest);

            TestMaterial = new SharpOpenGL.BasicMaterial.BasicMaterial();
            Simple = new SharpOpenGL.SimpleMaterial.SimpleMaterial();
            TestMaterial.Use();
            if (MyFBXMesh != null)
            {
                MyFBXMesh.SetFBXMeshInfo(TestParsedFbxMesh);
            }

            cube = new Core.Cube();
            
            for (int i = 0; i < 100; i++)
            {
                AnimList.Add(new FBXMeshAnimation(TestAnimation, i));
            }

            timer.Start();

            Simple.Use();
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

            GL.ClearColor(1, 1, 1, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);            
            
            Camera.UpdateViewMatrix();
            Camera.UpdateProjMatrix();
            
            ModelTransform.View = Camera.View;
            ModelTransform.Proj = Camera.Proj;

            LineTransform.View = Camera.View;
            LineTransform.Proj = Camera.Proj;
            LineTransform.Model = ModelTransform.Model;

            TestMaterial.Setup();
            TestMaterial.SetUniformBufferValue<SharpOpenGL.BasicMaterial.Transform>("Transform", ref ModelTransform);

            if(MyFBXMesh != null)
            {                
               //MyFBXMesh.Draw();
            }

            Simple.Setup();
            Simple.SetUniformBufferValue<SharpOpenGL.SimpleMaterial.Transform>("Transform", ref LineTransform);
            if(MyFBXMesh != null)
            {
                //MyFBXMesh.DrawBoneHierarchy();
            }

            if(AnimList != null)
            {
                AnimList[nCurrentAnimIndex].DrawBoneHierarchy();
            }
            
            GlControl.SwapBuffers();
        }

        protected void OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {            
            if (Camera != null)
            {
                Camera.OnKeyDown(sender, e);
                GlControl.Invalidate();
            }
        }

        private void GLControlResize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, GlControl.Size.Width, GlControl.Size.Height);

            float fAspectRatio = GlControl.Size.Width / (float) GlControl.Size.Height;

            var screenResizeEvent = new Core.CustomEvent.ScreenResizeEventArgs();
            screenResizeEvent.Width = GlControl.Size.Width;
            screenResizeEvent.Height = GlControl.Size.Height;
            
            Camera.OnWindowResized(this, screenResizeEvent);

            Camera.AspectRatio = fAspectRatio;
            Camera.FOV = MathHelper.PiOver6;
            Camera.Near = 1;
            Camera.Far = 10000;
            Camera.EyeLocation = new Vector3(10, 10, 10);
            Camera.DestLocation = new Vector3(10, 10, 10);
            Camera.LookAtLocation = new Vector3(0, 0, 0);

            Camera.UpdateViewMatrix();
            Camera.UpdateProjMatrix();


            ModelTransform.Proj = Camera.Proj;
            ModelTransform.Model = Matrix4.CreateScale(0.01f);
            ModelTransform.View = Camera.View;
        }

        protected SharpOpenGL.BasicMaterial.BasicMaterial TestMaterial = null;
        protected SharpOpenGL.SimpleMaterial.SimpleMaterial Simple = null;
        
        
        protected Matrix4 ModelView = new Matrix4();
        protected Matrix4 Projection = new Matrix4();
        protected OrbitCamera Camera = new OrbitCamera();        

        protected SharpOpenGL.BasicMaterial.Transform ModelTransform = new SharpOpenGL.BasicMaterial.Transform();
        protected SharpOpenGL.SimpleMaterial.Transform LineTransform = new SharpOpenGL.SimpleMaterial.Transform();

        protected ParsedFBXMesh TestParsedFbxMesh = null;
        protected ParsedFBXAnimStack TestAnimStack = null;
        protected ParsedFBXAnimation TestAnimation = null;

        protected DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer(DispatcherPriority.Normal);

        protected FBXMesh MyFBXMesh = null;
        protected FBXMeshAnimation MyFBXAnim = null;

        protected Core.Cube cube = null;

        List<FBXMeshAnimation> AnimList = new List<FBXMeshAnimation>();

        protected Core.Primitive.Line MyLine = null;
        protected Core.Primitive.LineDrawer MyLineDrawer = null;

        protected int nCurrentAnimIndex = 0;

        private void AnimIndex_Click(object sender, RoutedEventArgs e)
        {
            nCurrentAnimIndex++;
            nCurrentAnimIndex = nCurrentAnimIndex % AnimList.Count;
            GlControl.Invalidate();
        }
    }    
}