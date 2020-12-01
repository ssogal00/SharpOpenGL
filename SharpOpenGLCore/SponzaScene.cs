using System;
using System.Collections.Generic;
using System.Text;
using SharpOpenGL;
using SharpOpenGL.Scene;

namespace SharpOpenGLCore
{
    public class SponzaScene : SceneBase
    {
        public override void InitializeScene()
        {
            var sponzamesh = CreateGameObject<StaticMeshObject,string>("sponza2.staticmesh");
            sponzamesh.IsMetallicOverride = sponzamesh.IsRoughnessOverride = true;
            sponzamesh.Metallic = 0.7f;
            sponzamesh.Roughness = 0.3f;
        }

    }
}
