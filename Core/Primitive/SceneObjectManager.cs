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
            SceneObjectList.Add(result);
            return result;
        }

        public T CreateSceneObject<T, TParam1, TParam2>(TParam1 param1, TParam2 param2) where T : SceneObject, new()
        {
            T result = Activator.CreateInstance(typeof(T), new object[] { param1, param2 }) as T;
            SceneObjectList.Add(result);
            return result;
        }

        public T CreateSceneObject<T, TParam1, TParam2, TParam3>(TParam1 param1, TParam2 param2, TParam3 param3) where T : SceneObject, new()
        {
            T result = Activator.CreateInstance(typeof(T), new object[] { param1, param2, param3 }) as T;
            SceneObjectList.Add(result);
            return result;
        }

        public void Draw(MaterialBase.MaterialBase material)
        {
            foreach (var obj in SceneObjectList)
            {
                obj.Draw(material);
            }
        }

        public void AddSceneObject(SceneObject obj)
        {
            SceneObjectList.Add(obj);
        }

        protected List<SceneObject> SceneObjectList = new List<SceneObject>();
    }
}
