using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace Core
{

    public enum MessageQueueState
    {
        Stopped, 
        Running, 
        Stopping,
    }

    public struct MessageInfo
    {
        public MessageInfo(Action<object> callback, object message)
        {
            Callback = callback;
            Message = message;
        }

        public Action<object> Callback;
        public object Message;
    }

    public class MessageQueue : IDisposable
    {
        public void Post(object message)
        {
            Post(null, message);
        }

        public void Post(Action<object> callback, object message)
        {
            lock(queue)
            {
                queue.Add(new MessageInfo(callback, message));
                Monitor.Pulse(queue);
            }
        }

        public void StartInAnotherThread()
        {

            lock(queue)
            {
                if (state != MessageQueueState.Stopped)
                {
                    throw new InvalidOperationException("message queue is already running");
                }

                state = MessageQueueState.Running;
            }
            Thread thread = new Thread(Run);
            thread.IsBackground = true;
            thread.Start();
        }

        private void Run()
        {
            List<MessageInfo> currentQueue = new List<MessageInfo>();

            do
            {
                lock (queue)
                {
                    if (queue.Count > 0)
                    {
                        currentQueue.AddRange(queue);
                        queue.Clear();
                    }
                    else
                    {
                        Monitor.Wait(queue);
                        currentQueue.AddRange(queue);
                        queue.Clear();
                    }
                }

                ProcessCurrentQueue(currentQueue);
                currentQueue.Clear();
            } while (state == MessageQueueState.Running);

            lock(queue)
            {
                state = MessageQueueState.Stopped;
            }
        }

        public void Terminate()
        {
            Post(StopQueue, null);
        }

        private void StopQueue(object userData)
        {
            state = MessageQueueState.Stopping;
        }

        public void ProcessQueue()
        {
            List<MessageInfo> currentQueue = null;

            lock (queue)
            {
                if (state != MessageQueueState.Stopped)
                {
                    throw new InvalidOperationException("The Message Queue is already running");
                }

                if (queue.Count > 0)
                {
                    state = MessageQueueState.Running;
                    currentQueue = new List<MessageInfo>(queue);
                    queue.Clear();
                }
            }

            if(currentQueue != null)
            {
                ProcessCurrentQueue(currentQueue);
                lock(queue)
                {
                    state = MessageQueueState.Stopped;
                }
            }
        }

        public void Dispose()
        {
        }

        private void ProcessCurrentQueue(List<MessageInfo> currentQueue)
        {
            for(int i = 0; i < currentQueue.Count; ++i)
            {
                if(state == MessageQueueState.Stopping)
                {
                    lock(queue)
                    {
                        currentQueue.RemoveRange(0, i);
                        queue.InsertRange(0, currentQueue);
                    }
                    break;
                }

                MessageInfo message = currentQueue[i];
                if(message.Callback != null)
                {
                    message.Callback(message.Message);
                }
                else
                {

                }
            }
        }

        

        private List<MessageInfo> queue = new List<MessageInfo>();
        private MessageQueueState state;
    }
}
