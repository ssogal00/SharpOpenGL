using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SharpOpenGL
{
    public class RenderingThreadWindow : GameWindow
    {
        public RenderingThreadWindow(int width, int height)
        :base (width, height)
        {
            VSync = VSyncMode.Off;
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(Color.Brown);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

            SwapBuffers();
        }
    }
}
