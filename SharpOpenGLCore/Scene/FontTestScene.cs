using System;
using System.Collections.Generic;
using System.Text;
using SharpOpenGL.Scene;
using SharpOpenGLCore.SceneRenderer;

namespace SharpOpenGLCore.Scene
{
    public class FontTestScene : SceneBase
    {
        public override void InitializeScene()
        {

        }

        protected override SceneRendererBase CreateSceneRenderer()
        {
            return new FontTestSceneRenderer();
        }
    }
}
