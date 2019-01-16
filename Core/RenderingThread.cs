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

        public void Run()
        {
            RenderingThreadId = Thread.CurrentThread.ManagedThreadId;

            while (bRequestExist == false)
            {
                RenderingTheadJobQueue.Get().ExecuteTimeSlice();
            }

            Thread.Sleep(10);
        }

        public void RequestExit()
        {
            bRequestExist = true;
        }

        private bool bRequestExist = false;
    }
}
