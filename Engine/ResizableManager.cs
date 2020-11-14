using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using Core;
using Core.CustomEvent;
using System.Reactive;
using System.Reactive.Subjects;

namespace Core
{
    public class ResizableManager : Singleton<ResizableManager>
    {
        public void AddResizable(IResizable newResizable)
        {
            ResizeEventHandler.Subscribe(x => newResizable.OnResize(x.Item1, x.Item2));
        }

        public Subject<Tuple<int, int>> ResizeEventHandler = new Subject<Tuple<int, int>>();
    }
}
