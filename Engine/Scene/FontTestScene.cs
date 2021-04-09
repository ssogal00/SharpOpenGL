using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core;
using GLTF;
using Engine.Scene;
using Engine.SceneRenderer;
using GLTF.V2;

namespace Engine.Scene
{
    public class FontTestScene : SceneBase
    {
        public async override Task InitializeScene()
        {
            var v2Sample = GLTFLoader.LoadGLTFV2("./Resources/GLTF/DamagedHelmet/glTF/DamagedHelmet.gltf");

            var sampleMesh = GLTFMeshAsset.LoadFrom(v2Sample);

            if (sampleMesh.Count > 0)
            {
                mTestMeshObject = new GLTFStaticMeshObject(sampleMesh[0]);
                mTestMeshObject.Scale = 10.0f;
            }
        }

        public override void Render()
        {
            mTestMeshObject?.Render();
        }

        protected override SceneRendererBase CreateSceneRenderer()
        {
            //return new FontTestSceneRenderer();
            return new DefaultSceneRenderer(this);
        }

        private GLTFStaticMeshObject mTestMeshObject;
    }
}
