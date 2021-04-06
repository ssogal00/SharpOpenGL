using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core;

namespace Engine
{
    public class RenderingThread : Singleton<RenderingThread>
    {

        public static int RenderingThreadId = 0;
        private Stopwatch stopwatch = new Stopwatch();
        private RenderingThreadWindow window = null;

        public AutoResetEvent RenderingFinished = new AutoResetEvent(false);

        public RenderingThread()
        {
        }

        public void Run()
        {
            RenderingThreadId = Thread.CurrentThread.ManagedThreadId;
            window = new RenderingThreadWindow(1024,768);
            window.Run();
        }

        public static bool IsInRenderingThread()
        {
            if (Thread.CurrentThread.ManagedThreadId == RenderingThreadId)
            {
                return true;
            }

            return false;
        }

        public void ExecuteImmediatelyIfRenderingThread(Action action)
        {
            if (IsInRenderingThread())
            {
                action();
            }
            else
            {
                Enqueue(action);
            }
        }

        public void ExecuteImmediatelyIfRenderingThread(ThreadJob newJob)
        {
            if (IsInRenderingThread())
            {
                newJob.Do();
            }
            else
            {
                Enqueue(newJob);
            }
        }


        public void Enqueue(ThreadJob newJob)
        {
            JobQueue.Enqueue(newJob);
        }

        public void Enqueue(Action action)
        {
            JobQueue.Enqueue(new ActionJob(action));
        }

        public bool IsJobQueueEmpty()
        {
            return JobQueue.IsEmpty;
        }

        private void Execute()
        {
            while (JobQueue.IsEmpty == false)
            {
                ThreadJob job;
                JobQueue.TryDequeue(out job);
                job.Do();
            }
        }

        public void ExecuteTimeSlice(double milliseconds = 100)
        {
            double totalElapsed = 0;

            while (totalElapsed < milliseconds && JobQueue.IsEmpty == false)
            {
                ThreadJob job;
                JobQueue.TryDequeue(out job);

                stopwatch.Start();
                job.Do();

                var elapsed = stopwatch.ElapsedMilliseconds;
                totalElapsed += elapsed;

                stopwatch.Reset();
            }
        }

        ConcurrentQueue<ThreadJob> JobQueue = new ConcurrentQueue<ThreadJob>();
    }
}
