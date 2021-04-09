using System;
using System.Collections.Generic;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.Scene;

namespace Engine
{
    public class SponzaScene : SceneBase
    {
        public async override Task InitializeScene()
        {
            StaticMeshObject.CreateStaticMeshObjectAsync("sponza2.staticmesh").ToObservable().Subscribe(sponzamesh =>
            {
                mGameObjectList.Add(sponzamesh);

                sponzamesh.IsMetallicOverride = sponzamesh.IsRoughnessOverride = true;
                sponzamesh.Metallic = 0.30f;
                sponzamesh.Roughness = 0.30f;
            });
        }

        protected override SceneRendererBase CreateSceneRenderer()
        {
            return new DefaultSceneRenderer(this);
        }
    }
}
