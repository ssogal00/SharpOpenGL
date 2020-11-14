using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.CustomEvent;

namespace Core
{
    public interface IResizable
    {
        void OnResize(int width, int height);
    }
}
