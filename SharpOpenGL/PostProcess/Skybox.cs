using System;
using System.Drawing;
using Core;
using OpenTK;
using Core.Texture;
using SharpOpenGL.StaticMesh;
using SharpOpenGL.Asset;
using OpenTK.Graphics.OpenGL;
using Core.MaterialBase;

namespace SharpOpenGL.PostProcess
{
    public class Skybox : PostProcessBase
    {
        public Skybox()
        :base()
        { }

        public override void OnGLContextCreated(object sender, EventArgs e)
        {
            base.OnGLContextCreated(sender, e);

            Output.ClearColor = Color.AntiqueWhite;
            
            PostProcessMaterial = ShaderManager.Get().GetMaterial<CubemapMaterial.CubemapMaterial>();

            cubemapTexture = new CubemapTexture();
          
            cubemapTexture.Load();

            sphereMeshObject = new StaticMeshObject("./Resources/Imported/StaticMesh/sphere3.staticmesh");
        }

        public override void Render()
        {
            using (var dummy = new ScopedDepthFunc(DepthFunction.Lequal))
            {
                Output.BindAndExecute(PostProcessMaterial, () =>
                {
                    var test = (CubemapMaterial.CubemapMaterial) PostProcessMaterial;
                    test.ModelMatrix = ModelMatrix;
                    test.ViewMatrix = ViewMatrix;
                    test.ProjMatrix = ProjMatrix;
                    test.TexCubemap2D = cubemapTexture;
                    sphereMeshObject.Draw();
                });
            }
        }

        protected CubemapTexture cubemapTexture = null;

        public OpenTK.Matrix4 ViewMatrix = Matrix4.Identity;
        public OpenTK.Matrix4 ProjMatrix = Matrix4.Identity;
        public OpenTK.Matrix4 ModelMatrix = Matrix4.Identity;

        protected StaticMeshObject sphereMeshObject = null;
    }
}
