using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Tickable
{
    public static class TickableObjectManager
    {
        public static List<TickableObject> ObjectList = new List<TickableObject>();

        public static void Tick(double fDeltaSeconds)
        {
            CurrentTime += fDeltaSeconds;

            foreach(var Obj in ObjectList)
            {
                Obj.Tick(fDeltaSeconds);
            }
        }

        public static double CurrentTime = 0;
    }
}
