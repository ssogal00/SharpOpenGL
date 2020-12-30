using System;
using System.Collections.Generic;
using System.Text;
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
            var v2Sample = GLTFLoader.LoadGLTFV2(@"C:\MyGitHub\glTF-Sample-Models\2.0\DamagedHelmet\glTF\DamagedHelmet.gltf");

            var sampleMesh = new GLTFMesh(v2Sample);

            Console.WriteLine(v2Sample.Path);
        }

        protected override SceneRendererBase CreateSceneRenderer()
        {
            return new FontTestSceneRenderer();
        }
    }
}
