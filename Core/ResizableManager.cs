using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.CustomEvent;

namespace Core
{
    public class ResizableManager : Singleton<ResizableManager>
    {
        public void AddResizable(IResizable newResizable)
        {
            ResizeEventHandler += newResizable.OnResize;
        }

        public EventHandler<ScreenResizeEventArgs> ResizeEventHandler;
    }
}
