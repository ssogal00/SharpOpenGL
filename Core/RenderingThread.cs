using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core
{
    public class RenderingThread : Singleton<RenderingThread>
    {
        
        public static int RenderingThreadId = 0;
        private Stopwatch stopwatch = new Stopwatch();

        public RenderingThread()
        {
        }

        public bool IsIdle()
        {
            return JobQueue.IsEmpty;
        }

        public void Run()
        {
            RenderingThreadId = Thread.CurrentThread.ManagedThreadId;

            while (bRequestExist == false)
            {
                ExecuteTimeSlice();
            }
        }

        public void RequestExit()
        {
            bRequestExist = true;
        }

        private bool bRequestExist = false;


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

        private void ExecuteTimeSlice(double milliseconds = 100)
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
