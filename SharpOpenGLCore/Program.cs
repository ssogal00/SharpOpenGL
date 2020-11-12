using Core;
using Core.Buffer;
using Core.CustomEvent;
using Core.CustomSerialize;
using Core.Texture;
using Core.Tickable;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using Core.Asset;
using Core.StaticMesh;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SharpOpenGL.Font;
using SharpOpenGL.PostProcess;
using ZeroFormatter.Formatters;

namespace SharpOpenGL
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var renderThread = new Thread(RenderingThread.Get().Run);
            renderThread.Priority = ThreadPriority.AboveNormal;
            renderThread.Name = "RenderingThread";
            renderThread.Start();

            Engine.Get().Initialize();

            while (true)
            {
                if (Engine.Get().IsRequestExit)
                {
                    break;
                }
                Engine.Get().Tick();
                Thread.Sleep(1000 / 60);
            }

            renderThread.Join();
        }
    }
}
