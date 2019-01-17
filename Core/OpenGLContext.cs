using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using Core.Texture;
using System.Diagnostics;
using System.Threading;

namespace Core
{
    public class OpenGLContext : Singleton<OpenGLContext>
    {
        public void SetGameWindow(OpenTK.GameWindow window)
        {
            this.window = window;            
        }

        public TextureBase CreateTexture()
        {
            return null;
        }

        public bool IsValid { get { return this.window != null; } }

        public OpenTK.GameWindow GameWindow => window;

        public int WindowWidth => window.Width;
        public int WindowHeight => window.Height;

        public OpenGLContext()
        {
            this.window = null;
        }

        public OpenGLContext(OpenTK.GameWindow window)
        {
            this.window = window;
        }
        
        public void SetMainThreadId(int threadId)
        {
            mainThreadId = threadId;
        }

        public void SetRenderingThreadId(int threadId)
        {
            renderingThreadId = threadId;
        }

        public int MainTheadId { get { return mainThreadId; } }
        public int RenderingThreadId
        {
            get { return renderingThreadId; }
        }

        private OpenTK.GameWindow window = null;

        private int mainThreadId = 0;
        private int renderingThreadId = 0;

        public static EventHandler<EventArgs> OpenGLContextCreated;

        public void MakeCurrent()
        {
            window.MakeCurrent();
        }

        protected void CheckInRenderThread()
        {
            Debug.Assert(RenderingThreadId == Thread.CurrentThread.ManagedThreadId);
        }

        public void Clear()
        {   
            CheckInRenderThread();
            GL.ClearColor(Color.Brown);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void SwapBuffers()
        {   
            CheckInRenderThread();
            window.SwapBuffers();
        }

        public int GetActiveTexture()
        {
            return GL.GetInteger(GetPName.ActiveTexture);
        }

        public int GetMaxElementsIndices()
        {
            return GL.GetInteger(GetPName.MaxElementsIndices);
        }

        public int GetMaxElementsVertices()
        {
            return GL.GetInteger(GetPName.MaxElementsVertices);
        }

        public bool IsDepthTestEnabled()
        {            
            return GL.GetBoolean(GetPName.DepthTest);
        }

        public int GetMaxFragmentUniformBlocks()
        {            
            return GL.GetInteger(GetPName.MaxFragmentUniformBlocks);
        }

        public int GetMaxTextureUnits()
        {            
            return GL.GetInteger(GetPName.MaxTextureUnits);
        }
    }
}
