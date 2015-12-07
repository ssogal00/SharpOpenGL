using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

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

            VertexShader vs = new VertexShader();

            var content = File.ReadAllText("TestShader.vs");

            vs.CompileShader(content);

            FragmentShader fs = new FragmentShader();

            var fscontent = File.ReadAllText("TestShader.fs");

            fs.CompileShader(fscontent);

            ShaderProgram program = new ShaderProgram();

            program.AttachShader(vs);
            program.AttachShader(fs);

            program.LinkProgram();

            Console.WriteLine(program.ActiveUniformBlockCount);

            var names = program.GetUniformVariableNamesInBlock(0);
            
            foreach(var name in names)
            {
                Console.WriteLine(name);
            }
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
