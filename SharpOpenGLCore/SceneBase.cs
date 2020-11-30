using System;
using System.Collections.Generic;

using Core.Camera;
using Core.Buffer;
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


        public virtual void CreateSceneResources()
        { }

        public virtual void OnResize(object sender, ScreenResizeEventArgs args)
        {
        }

        public Vector3 CameraStartPos = new Vector3();
        public Vector3 CaemraStartDir = new Vector3();

    }
}
