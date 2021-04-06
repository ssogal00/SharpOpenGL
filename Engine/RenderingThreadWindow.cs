using Core;
using Core.CustomEvent;
using Core.Texture;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SharpOpenGLCore;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using SharpOpenGLCore.SceneRenderer;
using MouseButtonEventArgs = OpenTK.Windowing.Common.MouseButtonEventArgs;

namespace SharpOpenGL
{
    public class RenderingThreadWindow : GameWindow
    {
        protected Subject<Unit> GLContextCreated = new Subject<Unit>();
        protected Subject<Tuple<int,int>> WindowResized = new Subject<Tuple<int, int>>();

        public event EventHandler<KeyboardKeyEventArgs> OnKeyDownEvent;
        public event EventHandler<KeyboardKeyEventArgs> OnKeyUpEvent;

        public AutoResetEvent RenderingDone = new AutoResetEvent(false);

        
        protected SceneRendererBase mDefaultSceneRenderer = null;

        public static bool IsGLContextInitialized = false;

#region @Mouse Info
        private Vector2 LastMouseBtnDownPosition = new Vector2(0);
        private Vector2 LastMouseBtnMovePosition = new Vector2(0);
        private bool RightMouseBtnDown = false;
#endregion

        public RenderingThreadWindow(int width, int height)
        :base (new GameWindowSettings{IsMultiThreaded = false, UpdateFrequency = 500, RenderFrequency = 500}, NativeWindowSettings.Default)
        {
            GLContextCreated.Subscribe(_ =>
            {
                RenderingThreadObject.OnOpenGLContextCreated(null, null);
            });

            WindowResized.Subscribe((x) =>
            {
                CameraManager.Get().OnWindowResized(x.Item1, x.Item2);
            });
        }

        public int Width
        {
            get => this.ClientSize.X;
        }

        public int Height
        {
            get => this.ClientSize.Y;
        }


        protected override void OnLoad()
        {
            IsGLContextInitialized = true;

            Sampler.Initialize();
            ShaderManager.Get().CompileShaders();
            
            this.Title = "MyEngine";

            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.TextureCubeMap);
            GL.Enable(EnableCap.TextureCubeMapSeamless);
            
            OnKeyDownEvent += CameraManager.Get().OnKeyDown;
            OnKeyUpEvent += CameraManager.Get().OnKeyUp;
            OnKeyDownEvent += this.HandleKeyDownEvent;

            VSync = VSyncMode.Off;

            ResizableManager.Instance.ResizeEventHandler.OnNext(new Tuple<int,int>(this.Width, this.Height));

            mDefaultSceneRenderer = Engine.Get().CurrentScene.GetSceneRenderer();
            mDefaultSceneRenderer.Initialize();
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            OnKeyDownEvent?.Invoke(this, e);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (OnKeyUpEvent != null)
            {
                OnKeyUpEvent(this, e);
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Right)
            {
                RightMouseBtnDown = true;
            }
        }

        protected override void OnMouseEnter()
        {
            base.OnMouseEnter();
        }

        protected override void OnMouseLeave()
        {
            base.OnMouseLeave();
        }
        //
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (RightMouseBtnDown)
            {
                if (Math.Abs(e.DeltaX) > 0)
                {
                    CameraManager.Get().CurrentCamera.RotateYaw(e.DeltaX * 0.01f);
                }

                if (Math.Abs(e.DeltaY) > 0)
                {
                    CameraManager.Get().CurrentCamera.RotatePitch(e.DeltaY * 0.01f);
                }
            }
        }
        //
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Right)
            {
                RightMouseBtnDown = false;
            }
        }


        public void HandleKeyDownEvent(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Keys.F1)
            {
                CameraManager.Get().SwitchCamera();
            }
            else if (e.Key == Keys.F2)
            {
                CameraManager.Get().CurrentCamera.FOV += OpenTK.Mathematics.MathHelper.DegreesToRadians(1.0f);
            }
            else if (e.Key == Keys.F3)
            {
                CameraManager.Get().CurrentCamera.FOV -= OpenTK.Mathematics.MathHelper.DegreesToRadians(1.0f);
            }
            else if (e.Key == Keys.F6)
            {   
                
            }
            else if (e.Key == Keys.F4)
            {
                DebugDrawer.Get().IsGBufferDump = !DebugDrawer.Get().IsGBufferDump;
            }
            else if (e.Key == Keys.F5)
            {
                //gbufferVisualize.ChangeVisualizeMode();
            }
        }

        protected override void OnUnload()
        {
            Engine.Get().RequestExit();
        }
        
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);

            if (this.Width == 0 || this.Height == 0)
            {
                return;
            }

            ScreenResizeEventArgs eventArgs = new ScreenResizeEventArgs
            {
                Width = Width,
                Height = Height
            };

            float fAspectRatio = Width / (float)Height;

            var newSize = new Tuple<int, int>(Width, Height);

            WindowResized.OnNext(newSize);

            ResizableManager.Get().ResizeEventHandler.OnNext(newSize);
            
            this.Title = string.Format("MyEngine({0}x{1})", Width, Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            RenderingThread.Get().ExecuteTimeSlice();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            SharpOpenGL.Engine.Get().RequestExit();
        }
        
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Engine.Get().WaitForGameThread();

            this.MakeCurrent();
            GL.ClearColor(Color.BlueViolet);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

            if (Engine.Get().IsInitialized == false)
            {
                SwapBuffers();
                return;
            }

            mDefaultSceneRenderer.RenderScene(Engine.Get().CurrentScene);

            SwapBuffers();
        }
    }
}
