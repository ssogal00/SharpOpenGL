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
            Thread thread = new Thread(Run);
            thread.IsBackground = true;
            thread.Start();
        }

        private void Run()
        {

        }

        public void ProcessQueue()
        {
        }

        public void Dispose()
        {
        }

        

        private List<MessageInfo> queue = new List<MessageInfo>();
    }
}
