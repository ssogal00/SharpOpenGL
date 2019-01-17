using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core
{
    public class RenderingThread
    {
        public static int RenderingThreadId = 0;

        public RenderingThread()
        {
        }

        public bool IsIdle()
        {
            return RenderingThreadJobQueue.Get().IsJobQueueEmpty();
        }

        public void Run()
        {
            RenderingThreadId = Thread.CurrentThread.ManagedThreadId;

            while (bRequestExist == false)
            {
                //OpenGLContext.Get().MakeCurrent();
                OpenGLContext.Get().Clear();
                RenderingThreadJobQueue.Get().ExecuteTimeSlice();
                OpenGLContext.Get().SwapBuffers();
            }
        }

        public void RequestExit()
        {
            bRequestExist = true;
        }

        private bool bRequestExist = false;
    }
}
