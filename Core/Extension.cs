using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public static class Extension
    {
        public static float[] Flatten(this List<OpenTK.Vector2> vectorList)
        {
            float[] result = new float[vectorList.Count() * 2];

            for(int i = 0; i < vectorList.Count(); ++i)
            {
                result[i]       = vectorList[i].X;
                result[i + 1]   = vectorList[i].Y;
            }

            return result;
        }

        public static float[] Flatten(this List<OpenTK.Vector3> vectorList)
        {
            float[] result = new float[vectorList.Count() * 3];

            for (int i = 0; i < vectorList.Count(); ++i)
            {
                result[i] = vectorList[i].X;
                result[i + 1] = vectorList[i].Y;
                result[i + 2] = vectorList[i].Z;
            }

            return result;
        }
    }
}
