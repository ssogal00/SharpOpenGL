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
                TestParsedFbxMesh = SdkWrapper.ImportFBXMesh("FBXSample.FBX");
                TestAnimStack = SdkWrapper.ImportFBXAnimation("ActiveSkill01.FBX");
                TestAnimation = new ParsedFBXAnimation(TestParsedFbxMesh, TestAnimStack);

                MyFBXMesh = new FBXMesh();                
            }
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

            for (int i = 0; i < 100; i++)
            {
                AnimList.Add(new FBXMeshAnimation(TestAnimation, i));
            }

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

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //Camera.UpdateCameraDistance();
            Camera.UpdateViewMatrix();
            Camera.UpdateProjMatrix();
            
            ModelTransform.View = Camera.View;
            ModelTransform.Proj = Camera.Proj;

            LineTransform.View = Camera.View;
            LineTransform.Proj = Camera.Proj;
            LineTransform.Model = ModelTransform.Model;

            TestMaterial.Use();
            TestMaterial.SetUniformBufferValue<SharpOpenGL.BasicMaterial.Transform>("Transform", ref ModelTransform);

            if(MyFBXMesh != null)
            {                
               MyFBXMesh.Draw();
            }

             Simple.Use();
             Simple.SetUniformBufferValue<SharpOpenGL.SimpleMaterial.Transform>("Transform", ref LineTransform);
             if(MyFBXMesh != null)
             {
                // MyFBXMesh.DrawBoneHierarchy();
             }

            if(AnimList != null)
            {
                AnimList[nCurrentAnimIndex].DrawBoneHierarchy();
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
            Camera.EyeLocation = new Vector3(0, 0, -10);
            Camera.LookAtLocation = new Vector3(0, 0, 0);

            /*Camera.UpdateCameraDistance();*/
            Camera.UpdateViewMatrix();
            Camera.UpdateProjMatrix();

            ModelTransform.Proj = Matrix4.CreatePerspectiveFieldOfView(Camera.FOV, fAspectRatio, Camera.Near, Camera.Far);
            ModelTransform.Model = Matrix4.CreateFromAxisAngle(Vector3.UnitX, -OpenTK.MathHelper.PiOver2) * Matrix4.CreateScale(0.015f) ;               
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

        protected FBXMesh MyFBXMesh = null;
        protected FBXMeshAnimation MyFBXAnim = null;

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