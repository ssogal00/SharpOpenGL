using System;
using System.Collections.Generic;
using System.Text;
using Core;
using GLTF;
using SharpOpenGL.Scene;
using SharpOpenGLCore.SceneRenderer;
using GLTF.V2;

namespace SharpOpenGLCore.Scene
{
    public class FontTestScene : SceneBase
    {
        public override void InitializeScene()
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
            return new DefaultSceneRenderer();
        }

        private GLTFStaticMeshObject mTestMeshObject;
    }
}
