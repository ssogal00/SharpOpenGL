using Core.Primitive;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Engine;

namespace Engine.Scene
{
    public class SceneBase
    {
        public SceneBase()
        {
        }

        public async virtual Task InitializeScene()
        {
        }

        public virtual SceneRendererBase GetSceneRenderer()
        {
            if (mSceneRenderer == null)
            {
                mSceneRenderer = CreateSceneRenderer();
            }

            return mSceneRenderer;
        }

        protected virtual SceneRendererBase CreateSceneRenderer()
        {
            return null;
        }

        public virtual void DestroyScene()
        {
            mGameObjectList.Clear();
        }

        protected virtual void InitializeCamera()
        {
            CameraManager.Instance.CurrentCamera.EyeLocation = mCameraStartPos;
            CameraManager.Instance.CurrentCamera.LookAtLocation = mCameraStartPos + mCameraStartDir * 1.0f;
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

        protected Vector3 mCameraStartPos = Vector3.Zero;
        protected Vector3 mCameraStartDir = Vector3.UnitX;
        
        protected List<GameObject> mGameObjectList = new List<GameObject>();

        public List<GameObject> GameObjectList => mGameObjectList;

        protected SceneRendererBase mSceneRenderer = null;


    }
}
