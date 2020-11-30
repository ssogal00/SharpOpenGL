using System.Collections.Generic;
using Core.CustomEvent;
using Core.MaterialBase;
using Core.Primitive;
using OpenTK.Mathematics;


namespace SharpOpenGL.Scene
{
    public class SceneBase
    {
        public SceneBase()
        {
        }

        protected virtual void InitializeScene()
        {
        }
        
        public virtual void Draw()
        {
            
        }

        public virtual void Draw(MaterialBase material)
        {
        }

        public virtual void AddSceneObject(GameObject obj)
        {

        }

        public Vector3 CameraStartPos = Vector3.Zero;
        public Vector3 CaemraStartDir = Vector3.UnitX;
        
        protected List<GameObject> mSceneObjectList = new List<GameObject>();
    }
}
