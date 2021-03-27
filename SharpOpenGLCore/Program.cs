using System;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

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
            Engine.Instance.Initialize();

            var renderThread = new Thread(RenderingThread.Instance.Run);
            renderThread.Priority = ThreadPriority.AboveNormal;
            renderThread.Name = "RenderingThread";
            renderThread.Start();

            while (true)
            {
                if (Engine.Get().IsRequestExit)
                {
                    break;
                }
                Engine.Get().Tick();
            }

            // wait for rendering thread to finish
            renderThread.Join();
        }
    }
}
