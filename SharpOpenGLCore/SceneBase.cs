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
            InitializeScene();
        }

        protected virtual void InitializeScene()
        {
        }
        
        public virtual void Render()
        {
        }

        public Vector3 CameraStartPos = Vector3.Zero;
        public Vector3 CaemraStartDir = Vector3.UnitX;
        
        protected List<GameObject> mGameObjectList = new List<GameObject>();
    }
}
