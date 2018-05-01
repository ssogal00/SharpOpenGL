using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Camera;
using Core.Buffer;

namespace SharpOpenGL.Scene
{
    public class SceneBase
    {
        public SceneBase()
        {
        }

        public virtual void CreateSceneResources()
        {
            gBuffer = new GBuffer(1024,768);
        }
        
        public virtual void Draw()
        {

        }

        protected CameraBase camera = null;
        protected GBuffer gBuffer = null;
    }
}
