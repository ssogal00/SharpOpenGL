using Core.Buffer;
using Core.Camera;
using Core.OpenGLShader;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SharpOpenGL.StaticMesh;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;

using Core.MaterialBase;
using System.Windows.Input;
using SharpOpenGL;
using Core.CustomEvent;
using SharpOpenGL.GBufferDraw;
using Core.Texture;

namespace MaterialEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindowViewModel ViewModel
        {
            get
            {
                return (MainWindowViewModel)DataContext;
            }
        }

        protected ShaderProgram ProgramObject;

        protected OrbitCamera Camera = new OrbitCamera();
        protected DynamicUniformBuffer TransformBuffer = null;
        protected DynamicUniformBuffer ColorBuffer = null;
        
        protected SharpOpenGL.GBufferDraw.Transform Transform = new SharpOpenGL.GBufferDraw.Transform();

        protected Texture2D test = null;
        protected GBuffer MyGbuffer = new GBuffer(100,100);
        protected BlitToScreen ScreenBlit = new BlitToScreen();

        protected MaterialBase DeferredMaterial = null;

        protected EventHandler<EventArgs> WindowCreateEvent;
        protected EventHandler<Core.CustomEvent.ScreenResizeEventArgs> WindowResizeEvent;

        protected ObjMesh Mesh = new ObjMesh();

        private void GLControlLoad(object sender, EventArgs e)
        {
            GL.ClearColor(System.Drawing.Color.LightGray);

            DeferredMaterial = new SharpOpenGL.GBufferDraw.GBufferDraw();            

            WindowCreateEvent += MyGbuffer.OnResourceCreate;
            WindowCreateEvent += Sampler.OnResourceCreate;

            WindowResizeEvent += MyGbuffer.OnWindowResized;            

            ScreenBlit.OnResourceCreate(sender, e);
            ScreenBlit.SetGridSize(1, 1);

            MyGbuffer.OnResourceCreate(this, e);

            WindowCreateEvent(this, e);

            test = new Texture2D();
            test.Load("./Resources/SponzaTexture/Sponza_Bricks_a_Albedo.tga");

            Mesh.Load("./Resources/ObjMesh/myteapot.obj", "./Resources/ObjMesh/myteapot.mtl");
            Mesh.PrepareToDraw();
            Mesh.LoadTextures();
        }

        private void GLControlPaint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.Enable(EnableCap.DepthTest);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            MyGbuffer.Bind();
            MyGbuffer.Clear(System.Drawing.Color.Gray);
            MyGbuffer.PrepareToDraw();

            DeferredMaterial.Setup();
            DeferredMaterial.SetUniformBufferValue<Transform>("Transform", ref Transform);

            Mesh.Draw(DeferredMaterial);

            MyGbuffer.Unbind();

            ScreenBlit.Blit(MyGbuffer.NormalBufferObject, 0, 0, 1, 1);            
            
            mGlControl.SwapBuffers();
        }

        private void GLControlResize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, mGlControl.Size.Width, mGlControl.Size.Height);

            if (WindowResizeEvent != null)
            {
                ScreenResizeEventArgs eventArgs = new ScreenResizeEventArgs();
                eventArgs.Width = mGlControl.Size.Width;
                eventArgs.Height = mGlControl.Size.Height;

                WindowResizeEvent(this, eventArgs);
            }

            float fAspectRatio = mGlControl.Size.Width / (float)mGlControl.Size.Height;

            Camera.AspectRatio = fAspectRatio;
            Camera.FOV = MathHelper.PiOver6;
            Camera.Near = 1;
            Camera.Far = 10000;
            Camera.EyeLocation = new Vector3(10, 10, 10);
            Camera.DestLocation = new Vector3(10, 10, 10);
            Camera.LookAtLocation = new Vector3(0, 0, 0);

            Camera.UpdateViewMatrix();
            Camera.UpdateProjMatrix();

            Transform.Proj = Camera.Proj;
            Transform.Model = Matrix4.CreateScale(0.1f);
            Transform.View = Camera.View;
        }

        /// <summary>
        /// Event raised when the user has started to drag out a connection.
        /// </summary>
        private void networkControl_ConnectionDragStarted(object sender, ConnectionDragStartedEventArgs e)
        {
            var draggedOutConnector = (ConnectorViewModel)e.ConnectorDraggedOut;
            var curDragPoint = Mouse.GetPosition(networkControl);

            //
            // Delegate the real work to the view model.
            //
            var connection = this.ViewModel.ConnectionDragStarted(draggedOutConnector, curDragPoint);

            //
            // Must return the view-model object that represents the connection via the event args.
            // This is so that NetworkView can keep track of the object while it is being dragged.
            //
            e.Connection = connection;
        }

        /// <summary>
        /// Event raised while the user is dragging a connection.
        /// </summary>
        private void networkControl_ConnectionDragging(object sender, ConnectionDraggingEventArgs e)
        {
            Point curDragPoint = Mouse.GetPosition(networkControl);
            var connection = (ConnectionViewModel)e.Connection;
            this.ViewModel.ConnectionDragging(curDragPoint, connection);
        }

        /// <summary>
        /// Event raised when the user has finished dragging out a connection.
        /// </summary>
        private void networkControl_ConnectionDragCompleted(object sender, ConnectionDragCompletedEventArgs e)
        {
            var connectorDraggedOut = (ConnectorViewModel)e.ConnectorDraggedOut;
            var connectorDraggedOver = (ConnectorViewModel)e.ConnectorDraggedOver;
            var newConnection = (ConnectionViewModel)e.Connection;
            this.ViewModel.ConnectionDragCompleted(newConnection, connectorDraggedOut, connectorDraggedOver);
        }
    }


    
}

