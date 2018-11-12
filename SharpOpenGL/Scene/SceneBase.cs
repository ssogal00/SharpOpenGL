using System;
using System.Collections.Generic;

using Core.Camera;
using Core.Buffer;
using Core.CustomEvent;
using Core.MaterialBase;
using Core.Primitive;


namespace SharpOpenGL.Scene
{
    public class SceneBase
    {
        public SceneBase()
        {
        }
        
        public virtual void Draw()
        {
            
        }

        public virtual void Draw(MaterialBase material)
        {
            
        }

        public void AddSceneObject(ISceneObject newSceneObject, MaterialBase material)
        {
            if (SceneObjects.ContainsKey(material))
            {
                SceneObjects[material].Add(newSceneObject);
            }
            else
            {
                SceneObjects[material] = new List<ISceneObject> {newSceneObject};
            }
        }


        public virtual void CreateSceneResources()
        { }

        public virtual void OnResize(object sender, ScreenResizeEventArgs args)
        {
        }

        public virtual void OnResize(int width, int height) { }
        
        protected CameraBase camera = null;
        protected GBuffer gbuffer = new GBuffer(1024, 768);
        protected int width = 1024;
        protected int height = 768;

        protected Dictionary<MaterialBase, List<ISceneObject>> SceneObjects = new Dictionary<MaterialBase, List<ISceneObject>>();

    }
}
