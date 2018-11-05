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
            foreach(var obj in SceneObjectList)
            {
                obj.Draw();
            }
        }

        public virtual void Draw(MaterialBase material)
        {
            foreach(var obj in SceneObjectList)
            {
                obj.Draw(material);
            }
        }

        public void AddSceneObject(ISceneObject newSceneObject)
        {
            SceneObjectList.Add(newSceneObject);
        }

        public virtual void CreateSceneResources()
        { }

        public virtual void OnResize(object sender, ScreenResizeEventArgs args)
        {
        }

        public virtual void OnResize(int width, int height) { }
        
        protected CameraBase camera = null;
        protected GBuffer gbuffer = null;
        protected int width = 1024;
        protected int height = 768;

        protected List<ISceneObject> SceneObjectList = new List<ISceneObject>();
    }
}
