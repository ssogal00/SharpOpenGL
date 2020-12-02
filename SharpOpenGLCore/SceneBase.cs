using Core.Primitive;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace SharpOpenGL.Scene
{
    public class SceneBase
    {
        public SceneBase()
        {
        }

        public async virtual void InitializeScene()
        {
        }

        public virtual void DestroyScene()
        {
            mGameObjectList.Clear();
        }

        protected virtual void InitializeCamera()
        {
            CameraManager.Get().CurrentCamera.EyeLocation = CameraStartPos;
            CameraManager.Get().CurrentCamera.LookAtLocation = CameraStartPos + CameraStartDir * 1.0f;
        }
        
        public virtual void Render()
        {
            foreach (var item in mGameObjectList)
            {
                item.Render();
            }
        }

        public T CreateGameObject<T>() where T : GameObject, new()
        {
            var result = new T();
            mGameObjectList.Add(result);
            return result;
        }

        public T CreateGameObject<T, TParam1>(TParam1 param1) where T : GameObject, new()
        {
            T result = Activator.CreateInstance(typeof(T), new object[] { param1 }) as T;
            mGameObjectList.Add(result);
            return result;
        }

        public async Task<T> CreateGameObjectAsync<T>() where T : GameObject, new()
        {
            T result = null;
            await Task.Factory.StartNew(() =>
            {
                result = new T();
            });

            return result;
        }

        public Vector3 CameraStartPos = Vector3.Zero;
        public Vector3 CameraStartDir = Vector3.UnitX;
        
        protected List<GameObject> mGameObjectList = new List<GameObject>();

        
    }
}
