using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using Core;

namespace SharpOpenGL
{
    public class RenderingTheadJobQueue : Singleton<RenderingTheadJobQueue>
    {
       

        public void Enqueue(ThreadJob newJob)
        {
            JobQueue.Enqueue(newJob);
        }

        public void Execute()
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
            var stopwatch = new Stopwatch();

            double totalElapsed = 0;

            while (totalElapsed < milliseconds && JobQueue.IsEmpty == false)
            {
                ThreadJob job;
                JobQueue.TryDequeue(out job);

                stopwatch.Start();
                job.Do();
                stopwatch.Stop();

                var elapsed = stopwatch.ElapsedMilliseconds;
                totalElapsed += elapsed;
            }
            
            //
            //
        }

        ConcurrentQueue<ThreadJob> JobQueue = new ConcurrentQueue<ThreadJob>();
    }
}
