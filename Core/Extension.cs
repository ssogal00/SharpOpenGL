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

            for(int i = 0; i < vectorList.Count(); i++)
            {
                result[2 * i]       = vectorList[i].X;
                result[2 * i + 1]   = vectorList[i].Y;
            }

            return result;
        }

        public static float[] Flatten(this List<OpenTK.Vector3> vectorList)
        {
            float[] result = new float[vectorList.Count() * 3];

            for (int i = 0; i < vectorList.Count(); i++)
            {
                result[3 * i] = vectorList[i].X;
                result[3 * i + 1] = vectorList[i].Y;
                result[3 * i + 2] = vectorList[i].Z;
            }

            return result;
        }

        public static float[] Flatten(this List<OpenTK.Vector4> vectorList)
        {
            float[] result = new float[vectorList.Count() * 4];
            for (int i = 0; i < vectorList.Count(); i++)
            {
                result[4 * i] = vectorList[i].X;
                result[4 * i + 1] = vectorList[i].Y;
                result[4 * i + 2] = vectorList[i].Z;
                result[4 * i + 3] = vectorList[i].W;
            }

            return result;
        }
    }
}
