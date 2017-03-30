using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Tickable
{
    public abstract class TickableObject
    {
        public TickableObject()
        {
            TickableObjectManager.ObjectList.Add(this);
        }

        public abstract void Tick(double fDeltaSeconds);
    }
}
