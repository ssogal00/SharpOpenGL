using System;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using Engine;

namespace SharpOpenGLCore
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Engine.Engine.Instance.Initialize();

            var renderThread = new Thread(RenderingThread.Instance.Run);
            renderThread.Priority = ThreadPriority.AboveNormal;
            renderThread.Name = "RenderingThread";
            renderThread.Start();

            while (true)
            {
                if (Engine.Engine.Instance.IsRequestExit)
                {
                    break;
                }
                Engine.Engine.Instance.Tick();
            }

            // wait for rendering thread to finish
            renderThread.Join();
        }
    }
}
