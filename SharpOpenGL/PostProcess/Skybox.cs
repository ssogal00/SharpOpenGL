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

            PostProcessMaterial = AssetManager.GetAsset<MaterialBase>("CubemapMaterial");

            cubemapTexture = new CubemapTexture();
          
            cubemapTexture.Load();
             
            SphereMesh = AssetManager.LoadAssetSync<StaticMeshAsset>("./Resources/Imported/StaticMesh/sphere3.staticmesh");
        }

        public override void Render()
        {
            using (var dummy = new ScopedDepthFunc(DepthFunction.Lequal))
            {
                Output.BindAndExecute(PostProcessMaterial, () =>
                {
                    PostProcessMaterial.SetUniformVarData("ModelMatrix", ref ModelMatrix);
                    PostProcessMaterial.SetUniformVarData("ViewMatrix", ref ViewMatrix);
                    PostProcessMaterial.SetUniformVarData("ProjMatrix", ref ProjMatrix);
                    PostProcessMaterial.SetTexture("texCubemap", cubemapTexture);
                    SphereMesh.Draw();
                });
            }
        }

        protected CubemapTexture cubemapTexture = null;

        public OpenTK.Matrix4 ViewMatrix = Matrix4.Identity;
        public OpenTK.Matrix4 ProjMatrix = Matrix4.Identity;
        public OpenTK.Matrix4 ModelMatrix = Matrix4.Identity;

        protected StaticMeshAsset SphereMesh = null;
    }
}
