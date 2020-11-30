using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Primitive
{
    public class SceneObjectManager : Singleton<SceneObjectManager>
    {

        public T CreateSceneObject<T>() where T : GameObject, new()
        {
            var result = new T();
            SceneObjectMap.TryAdd(result.Name, result);
            return result;
        }

        public T CreateSceneObject<T, TParam1>(TParam1 param1) where T : GameObject, new()
        {
            T result = Activator.CreateInstance(typeof(T), new object[] {param1}) as T;
            //SceneObjectList.Add(result);
            return result;
        }

        public T CreateSceneObject<T, TParam1, TParam2>(TParam1 param1, TParam2 param2) where T : GameObject, new()
        {
            T result = Activator.CreateInstance(typeof(T), new object[] { param1, param2 }) as T;
            //SceneObjectList.Add(result);
            return result;
        }

        public T CreateSceneObject<T, TParam1, TParam2, TParam3>(TParam1 param1, TParam2 param2, TParam3 param3) where T : GameObject, new()
        {
            T result = Activator.CreateInstance(typeof(T), new object[] { param1, param2, param3 }) as T;
            SceneObjectMap.TryAdd(result.Name, result);
            return result;
        }

        public void Draw()
        {
            foreach (var obj in SceneObjectMap.Values.Where(x => x.IsVisible))
            {
                obj.Draw();
            }
        }

        public void Tick(double delta)
        {
            foreach (var obj in SceneObjectMap.Values)
            {
                obj.Tick(delta);
            }
        }

        public void AddSceneObject(GameObject obj)
        {
            SceneObjectMap.TryAdd(obj.Name, obj);
        }

        public void RemoveSceneObject(GameObject obj)
        {
            
        }

        public GameObject GetSceneObject(string name)
        {
            if (SceneObjectMap.ContainsKey(name))
            {
                return SceneObjectMap[name];
            }

            return null;
        }

        public GameObject GetAnySceneObjectOf<T>()
        {
            foreach (var obj in SceneObjectMap.Values)
            {
                if (obj is T)
                {
                    return obj;
                }
            }

            return null;
        }

        public IEnumerable<T> GetSceneObjectListOfType<T>() where  T : GameObject
        {
            foreach (var obj in SceneObjectMap.Values)
            {
                if (obj is T)
                {
                    yield return (T) obj;
                }
            }
        }

        public IEnumerable<object> GetSceneObjectList()
        {
            return SceneObjectMap.Values.Select(x => { return x; });
        }

        public IEnumerable<object> GetEditableSceneObjectList()
        {
            return SceneObjectMap.Values.Where(x => x.IsEditable);
        }

        protected ConcurrentDictionary<string, GameObject> SceneObjectMap = new ConcurrentDictionary<string, GameObject>();
    }
}
