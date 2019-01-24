using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Primitive
{
    public class SceneObjectManager : Singleton<SceneObjectManager>
    {

        public T CreateSceneObject<T>() where T : SceneObject, new()
        {
            var result = new T();
            return result;
        }

        public T CreateSceneObject<T, TParam1>(TParam1 param1) where T : SceneObject, new()
        {
            T result = Activator.CreateInstance(typeof(T), new object[] {param1}) as T;
            return result;
        }

        public T CreateSceneObject<T, TParam1, TParam2>(TParam1 param1, TParam2 param2) where T : SceneObject, new()
        {
            T result = Activator.CreateInstance(typeof(T), new object[] { param1, param2 }) as T;
            return result;
        }
    }
}
