using System;
using System.Collections.Generic;
using System.Text;
using Engine.Scene;

namespace Engine
{
    public class SceneRendererBase 
    {
        public SceneRendererBase(SceneBase referenceScene)
        {
            mReferenceScene = referenceScene;
        }

        public virtual void Initialize()
        {

        }

        protected virtual void SyncGameObjectWithRenderObject()
        {

        }

        public virtual void RenderScene(SceneBase scene)
        {
            //
        }
        public virtual void UnloadScene()
        {

        }

        protected bool mSceneLoaded = false;

        protected SceneBase mReferenceScene = null;

        protected Dictionary<int, RenderThreadGameObject> mRenderThreadGameObjectMap = new Dictionary<int, RenderThreadGameObject>();
    }
}
