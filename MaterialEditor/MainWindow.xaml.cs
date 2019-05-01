using Core.Camera;
using Core.CustomEvent;
using Core.CustomSerialize;
using Core.MaterialBase;
using MaterialEditor.Utils;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ZeroFormatter.Formatters;
using CompiledMaterial.GBufferDraw;
using Core;
using Core.Buffer;
using OpenTK;
using OpenTK.Graphics;

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

            timer.Tick += new EventHandler(Tick);
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Start();

            watch.Start();
        }

        private void Tick(object sender, EventArgs e)
        {
            fAngle += 1.0f;

            if (mainCamera != null)
            {
                mainCamera.Tick(0.1f);
            }

            mGlControl.Invalidate();
        }
        
        
        public MainWindowViewModel ViewModel
        {
            get
            {
                return (MainWindowViewModel)DataContext;
            }
        }


        protected MaterialBase DeferredMaterial = null;
        protected Stopwatch watch = new Stopwatch();

        protected EventHandler<EventArgs> WindowCreateEvent;
        protected EventHandler<Core.CustomEvent.ScreenResizeEventArgs> WindowResizeEvent;

        protected DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer(DispatcherPriority.Normal);
        protected LiveMaterial liveMaterial = null;

        protected PreviewMesh previewMesh = null;
        protected OrbitCamera mainCamera = new OrbitCamera();
        protected GBufferDraw gbufferDrawMaterial = null;

        protected float fAngle = 0.0f;

        protected ImpObservableCollection<TextureFile> textureFileList = new ImpObservableCollection<TextureFile>();

        private void GLControlLoad(object sender, EventArgs e)
        {
            GL.ClearColor(System.Drawing.Color.LightGray);

            Formatter<DefaultResolver, OpenTK.Vector3>.Register(new Vector3Formatter<DefaultResolver>());
            Formatter<DefaultResolver, OpenTK.Vector2>.Register(new Vector2Formatter<DefaultResolver>());
            Formatter<DefaultResolver, OpenTK.Vector4>.Register(new Vector4Formatter<DefaultResolver>());
        }

        private void GLControlPaint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.Enable(EnableCap.DepthTest);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //

            if (gbufferDrawMaterial != null)
            {
                gbufferDrawMaterial.BindAndExecute(() =>
                {
                    gbufferDrawMaterial.CameraTransform_Proj = this.mainCamera.Proj;
                    gbufferDrawMaterial.CameraTransform_View = this.mainCamera.View;
                    gbufferDrawMaterial.ModelTransform_Model = Matrix4.Identity;
                    gbufferDrawMaterial.DiffuseMapExist = false;
                    gbufferDrawMaterial.DiffuseOverride = Vector3.One;
                    gbufferDrawMaterial.NormalMapExist = false;
                    gbufferDrawMaterial.MaskMapExist = false;
                    gbufferDrawMaterial.RoughnessExist = false;

                    previewMesh.Draw();
                });
            }

            mGlControl.SwapBuffers();
        }

        protected void OnLoaded(object sender, RoutedEventArgs e)
        {
            InitializeZeroFormatter();

            previewMesh = new PreviewMesh("./Resources/Imported/StaticMesh/myteapot.staticmesh");
            gbufferDrawMaterial = new GBufferDraw();
        }

        private void InitializeZeroFormatter()
        {
            Formatter<DefaultResolver, OpenTK.Vector3>.Register(new Vector3Formatter<DefaultResolver>());
            Formatter<DefaultResolver, OpenTK.Vector2>.Register(new Vector2Formatter<DefaultResolver>());
            Formatter<DefaultResolver, OpenTK.Vector4>.Register(new Vector4Formatter<DefaultResolver>());
        }

        public void GLControlMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (mainCamera != null)
            {
                mainCamera.MoveBackward();
            }
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

            if (mainCamera != null)
            {
                mainCamera.AspectRatio = fAspectRatio;
            }
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

        private void networkControl_QueryConnectionFeedback(object sender, QueryConnectionFeedbackEventArgs e)
        {
            var draggedOutConnector = (ConnectorViewModel)e.ConnectorDraggedOut;
            var draggedOverConnector = (ConnectorViewModel)e.DraggedOverConnector;
            object feedbackIndicator = null;
            bool connectionOk = true;

            this.ViewModel.QueryConnnectionFeedback(draggedOutConnector, draggedOverConnector, out feedbackIndicator, out connectionOk);

            //
            // Return the feedback object to NetworkView.
            // The object combined with the data-template for it will be used to create a 'feedback icon' to
            // display (in an adorner) to the user.
            //
            e.FeedbackIndicator = feedbackIndicator;

            //
            // Let NetworkView know if the connection is ok or not ok.
            //
            e.ConnectionOk = connectionOk;
        }

        private void CreateNode_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CreateNode();
        }

        private void CreateNode()
        {
            var newNodePosition = Mouse.GetPosition(networkControl);
            this.ViewModel.CreateNode("New Node!", newNodePosition, true);
        }

        private void DeleteConnection_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var connection = (ConnectionViewModel)e.Parameter;
            this.ViewModel.DeleteConnection(connection);
        }

        private void CreateConstantVector3(object sender, ExecutedRoutedEventArgs e)
        {
            var newNodePosition = Mouse.GetPosition(networkControl);
            this.ViewModel.CreateNode<ConstantVector3Node>("Vector3", newNodePosition);
        }

        private void CreateConstantVector4(object sender, ExecutedRoutedEventArgs e)
        {
            var newNodePosition = Mouse.GetPosition(networkControl);
            this.ViewModel.CreateNode<ConstantVector4Node>("Vector4", newNodePosition);
        }

        private void CreateVector3AddNode(object sender, ExecutedRoutedEventArgs e)
        {
            var newNodePosition = Mouse.GetPosition(networkControl);
            this.ViewModel.CreateNode<Vector3AddNode>("Add Vector3", newNodePosition);
        }

        private void CreateSineNode(object sender, ExecutedRoutedEventArgs e)
        {
            var newNodePosition = Mouse.GetPosition(networkControl);
            this.ViewModel.CreateNode<SineNode>("Sine", newNodePosition);
        }

        private void CreateTimeNode(object sender, ExecutedRoutedEventArgs e)
        {
            var newNodePosition = Mouse.GetPosition(networkControl);
            this.ViewModel.CreateNode<TimeParamNode>("Time", newNodePosition);
        }

        private void CreateVariableVector3(object sender, ExecutedRoutedEventArgs e)
        {
            var newNodePosition = Mouse.GetPosition(networkControl);
            this.ViewModel.CreateNode<VariableVector3Node>("Variable Vector3", newNodePosition);
        }

        private void CreateVector4AddNode(object sender, ExecutedRoutedEventArgs e)
        {
            var newNodePosition = Mouse.GetPosition(networkControl);
            this.ViewModel.CreateNode<Vector4AddNode>("Vector4 Add", newNodePosition);
        }

        private void CreateTextureNode(object sender, ExecutedRoutedEventArgs e)
        {
            var newNodePosition = Mouse.GetPosition(networkControl);

            if (textureListView != null && textureListView.SelectedIndex != -1)
            {
                if (textureFileList.Count > textureListView.SelectedIndex)
                {
                    var newNode = this.ViewModel.CreateNode<TextureParamNode>("Texture Param", newNodePosition);
                    newNode.ImageSource = textureFileList[textureListView.SelectedIndex].ImageSource;
                }
            }
        }

        private void CreateViewspaceNormalNode(object sender, ExecutedRoutedEventArgs e)
        {
            var newNodePosition = Mouse.GetPosition(networkControl);
            this.ViewModel.CreateNode<ViewspaceNormalNode>("Viewspace Normal", newNodePosition);
        }

        private void CreateViewspacePositionNode(object sender, ExecutedRoutedEventArgs e)
        {
            var newNodePosition = Mouse.GetPosition(networkControl);
            this.ViewModel.CreateNode<ViewspacePositionNode>("Viewspace Position", newNodePosition);
        }

        private void CreateMinNode(object sender, ExecutedRoutedEventArgs e)
        {
            var newNodePosition = Mouse.GetPosition(networkControl);
            this.ViewModel.CreateNode<MinNode>("Min", newNodePosition);
        }

        private void CreateMaxNode(object sender, ExecutedRoutedEventArgs e)
        {
            var newNodePosition = Mouse.GetPosition(networkControl);
            this.ViewModel.CreateNode<MaxNode>("Max", newNodePosition);
        }

        private void CreateAbsNode(object sender, ExecutedRoutedEventArgs e)
        {
            var newNodePosition = Mouse.GetPosition(networkControl);
            this.ViewModel.CreateNode<AbsNode>("Abs", newNodePosition);
        }

        private void CreateConstantFloatNode(object sender, ExecutedRoutedEventArgs e)
        {
            var newNodePosition = Mouse.GetPosition(networkControl);
            this.ViewModel.CreateNode<ConstantFloatNode>("Float", newNodePosition);
        }

        private void OnBtnCompileClick(object sender, RoutedEventArgs e)
        {   
            liveMaterial.Compile(this.ViewModel.Network);

            if(liveMaterial.IsValid())
            {
                mGlControl.Invalidate();
            }
        }
    }
}

