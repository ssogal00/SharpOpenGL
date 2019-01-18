using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Buffer;
using Core.CustomEvent;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SharpOpenGL.Asset;
using SharpOpenGL.PostProcess;

namespace SharpOpenGL
{
    public class RenderingThreadWindow : GameWindow
    {
        protected event EventHandler<EventArgs> OnGLContextCreated;
        protected event EventHandler<ScreenResizeEventArgs> OnWindowResize;

        protected Skybox skyboxPostProcess = new Skybox();
        protected GBuffer renderGBuffer = new GBuffer(1024, 768);

        public RenderingThreadWindow(int width, int height)
        :base (width, height)
        {
            OnGLContextCreated = this.GLContextCreate;
            OnGLContextCreated += RenderResource.OnOpenGLContextCreated;

            OnWindowResize = CameraManager.Get().OnWindowResized;
            OnWindowResize += ResizableManager.Get().ResizeEventHandler;
        }

        protected override void OnLoad(EventArgs e)
        {
            VSync = VSyncMode.Off;

            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.TextureCubeMap);
            GL.Enable(EnableCap.TextureCubeMapSeamless);

            OnGLContextCreated(this, e);
        }

        protected void GLContextCreate(object o, EventArgs e)
        {
        }

        protected override void OnUnload(EventArgs e)
        {
            Engine.Get().RequestExit();
        }
        
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);

            ScreenResizeEventArgs eventArgs = new ScreenResizeEventArgs
            {
                Width = Width,
                Height = Height
            };

            float fAspectRatio = Width / (float)Height;

            OnWindowResize(this, eventArgs);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            RenderingThread.Get().ExecuteTimeSlice();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(Color.Brown);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

          

            skyboxPostProcess.ModelMatrix = OpenTK.Matrix4.CreateScale(10.0f) * OpenTK.Matrix4.CreateTranslation(CameraManager.Get().CurrentCameraEye);
            skyboxPostProcess.ViewMatrix = CameraManager.Get().CurrentCameraView;
            skyboxPostProcess.ProjMatrix = CameraManager.Get().CurrentCameraProj;
            skyboxPostProcess.Render();

            renderGBuffer.BindAndExecute(
                () =>
                {
                    renderGBuffer.Clear();
                });

            skyboxPostProcess.GetOutputRenderTarget().Copy(renderGBuffer.GetColorAttachement);

            SwapBuffers();
        }
    }
}
