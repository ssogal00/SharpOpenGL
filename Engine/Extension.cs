using System;
using System.Collections.Generic;
using System.Linq;
using Core.Buffer;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Core
{
    public static class Extension
    {   
        public static void BindAndExecute(this IBindable bindable, Action action)
        {
            bindable.Bind();
            action();
            bindable.Unbind();
        }

        public static void BindAndExecute(this IBindable first, IBindable second, Action action)
        {
            first.Bind(); second.Bind();
            action();
            first.Unbind(); second.Unbind();
        }

        public static void BindAndExecute(this IBindable first, IBindable second, IBindable third, Action action)
        {
            first.Bind(); second.Bind(); third.Bind();
            action();
            first.Unbind(); second.Unbind(); third.Unbind();
        }

        public static void BindAndExecute(this IBindable first, IBindable second, IBindable third, IBindable forth, Action action)
        {
            first.Bind(); second.Bind(); third.Bind(); forth.Bind();
            action();
            first.Unbind(); second.Unbind(); third.Unbind(); forth.Unbind();
        }


        public static void BindAndExecute(this IEnumerable<IBindable> bindableList, Action action)
        {
            //            
            foreach (var each in bindableList)
            {
                each.Bind();
            }

            //
            action();

            //            
            foreach(var each in bindableList)
            {
                each.Unbind();
            }
        }
        
        public static T BindAndExecute<T>(this IBindable bindable, Func<T> function)
        {
            bindable.Bind();
            var ret = function();
            bindable.Unbind();
            return ret;
        }

        public static float[] FlattenVec2List(this List<Vector2> vectorList)
        {
            float[] result = new float[vectorList.Count() * 2];

            for(int i = 0; i < vectorList.Count(); i++)
            {
                result[2 * i]       = vectorList[i].X;
                result[2 * i + 1]   = vectorList[i].Y;
            }

            return result;
        }

        public static float[] FlattenVec2Array(this Vector2[] vectorList)
        {
            float[] result = new float[vectorList.Count() * 2];

            for (int i = 0; i < vectorList.Count(); i++)
            {
                result[2 * i] = vectorList[i].X;
                result[2 * i + 1] = vectorList[i].Y;
            }

            return result;
        }

        public static float[] FlattenVec3List(this List<Vector3> vectorList)
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

        public static float[] FlattenVec3Array(this Vector3[] vectorList)
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

        public static float[] FlattenVec4Array(this Vector4[] vectorList)
        {
            float[] result = new float[vectorList.Count() * 4];

            for (int i = 0; i < vectorList.Count(); i++)
            {
                result[3 * i] = vectorList[i].X;
                result[3 * i + 1] = vectorList[i].Y;
                result[3 * i + 2] = vectorList[i].Z;
                result[3 * i + 3] = vectorList[i].W;
            }

            return result;
        }

        public static float[] FlattenVec4List(this List<Vector4> vectorList)
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

        public static int GetComponentCount(this Vector3 dummy)
        {
            return 3;
        }
    }
}

