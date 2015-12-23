using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpOpenGL
{
    public static class TickableObjectManager
    {
        public static List<TickableObject> ObjectList = new List<TickableObject>();

        public static void Tick(double fDeltaSeconds)
        {
            foreach(var Obj in ObjectList)
            {
                Obj.Tick(fDeltaSeconds);
            }
        }
    }
}
