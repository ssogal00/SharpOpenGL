using System;
using System.Collections.Generic;
using System.Text;
using SharpOpenGL;
using SharpOpenGL.Scene;

namespace SharpOpenGLCore
{
    public class SponzaScene : SceneBase
    {
        public async override void InitializeScene()
        {
            var sponzamesh = await StaticMeshObject.CreateStaticMeshObjectAsync("sponza2.staticmesh");

            mGameObjectList.Add(sponzamesh);

            sponzamesh.IsMetallicOverride = sponzamesh.IsRoughnessOverride = true;
            sponzamesh.Metallic = 0.7f;
            sponzamesh.Roughness = 0.3f;
        }

    }
}
