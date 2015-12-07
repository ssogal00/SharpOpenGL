using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace SharpOpenGL
{
    public class MainWindow : GameWindow
    {
        protected override void OnLoad(EventArgs e)
        {
            VSync = VSyncMode.Off;

            GL.ClearColor(System.Drawing.Color.MidnightBlue);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            SwapBuffers();
        }
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (MainWindow example = new MainWindow())            
            {                
                example.Run(60);
            }
        }
    }
}
