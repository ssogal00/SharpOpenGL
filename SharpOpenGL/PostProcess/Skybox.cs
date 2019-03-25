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

            Output.ClearColor = Color.Red;
            
            PostProcessMaterial = ShaderManager.Get().GetMaterial<CubemapMaterial.CubemapMaterial>();

            sphereMeshObject = new StaticMeshObject("./Resources/Imported/StaticMesh/sphere3.staticmesh");
            sphereMeshObject.SetVisible(false);
        }

        public override void Render()
        {
            if (cubemapTexture == null)
            {
                return;
            }
            using (var dummy = new ScopedDepthFunc(DepthFunction.Lequal))
            {
                Output.BindAndExecute(PostProcessMaterial, () =>
                {
                    Output.Clear();

                    var specificMaterial = (CubemapMaterial.CubemapMaterial) PostProcessMaterial;
                    specificMaterial.ModelMatrix = OpenTK.Matrix4.CreateScale(10.0f) * OpenTK.Matrix4.CreateTranslation(CameraManager.Get().CurrentCameraEye);
                    specificMaterial.ViewMatrix = CameraManager.Get().CurrentCameraView;
                    specificMaterial.ProjMatrix = CameraManager.Get().CurrentCameraProj;
                    specificMaterial.TexCubemap2D = cubemapTexture;
                    sphereMeshObject.DrawWithBindedMaterial();
                });
            }
        }

        public void SetCubemapTexture(TextureBase cubemapTextureOverride)
        {
            if (cubemapTexture != null)
            {
                cubemapTexture.Dispose();
            }

            cubemapTexture = cubemapTextureOverride;
        }

        protected TextureBase cubemapTexture = null;
        protected StaticMeshObject sphereMeshObject = null;
    }
}
