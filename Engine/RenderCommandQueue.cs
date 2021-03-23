using System;
using System.Collections.Generic;
using System.Text;
using Core;

namespace Engine
{
    public class RenderCommandQueue
    {
        public void Flush()
        {
            foreach (var cmd in mCommandQueue)
            {
                cmd.Execute();
            }
        }

        public void AddCommand(RenderCommand cmd)
        {
            mCommandQueue.Enqueue(cmd);
        }

        public void Clear()
        {
            mCommandQueue.Clear();
        }

        private Queue<RenderCommand> mCommandQueue = new Queue<RenderCommand>();
    }
}
