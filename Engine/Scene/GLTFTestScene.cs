using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core;
using GLTF;
using Engine.Scene;
using GLTF.V2;

namespace Engine.Scene
{
    public class GLTFTestScene : SceneBase
    {
        public override async Task InitializeScene()
        {
            var v2Sample = GLTFLoader.LoadGLTFV2("./Resources/GLTF/FlightHelmet/glTF/FlightHelmet.gltf");

            var sampleMesh = GLTFMeshAsset.LoadFrom(v2Sample);

            sampleMesh.ForEach(item =>
            {
                var newmesh = new GLTFStaticMeshObject(item);
                newmesh.Scale = 10;
                mMeshList.Add(newmesh);
            });
        }

        protected override void InitializeCamera()
        {
            base.InitializeCamera();
        }

        public override void Render()
        {
            foreach (var mesh in mMeshList)
            {
                mesh?.Render();
            }
        }

        protected override SceneRendererBase CreateSceneRenderer()
        {
            return new DefaultSceneRenderer(this);
        }

        private List<GLTFStaticMeshObject> mMeshList = new List<GLTFStaticMeshObject>();
    }
}
