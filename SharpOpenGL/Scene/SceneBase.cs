using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Camera;
using Core.Buffer;
using Core.CustomEvent;

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
    }
}
