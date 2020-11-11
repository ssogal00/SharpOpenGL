using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class BindState
    {
        protected static BindState Instance = new BindState();
        public static BindState Get()
        {
            return Instance;
        }

        public void SetBoundVertexBuffer(string vertexBufferName)
        {
            CurrentVertexBufferBound = vertexBufferName;
        }

        public void SetBoundFrameBuffer(string frameBufferName)
        {
            CurrentVertexBufferBound = frameBufferName;
        }

        protected string CurrentVertexBufferBound = "";
        protected string CurrentIndexBufferBound = "";
        protected string CurrentFrameBufferBound = "";
    }
}
