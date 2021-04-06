using Core;
using Core.Texture;
using OpenTK.Graphics.OpenGL;
using System;
using CompiledMaterial.CubemapMaterial;
using OpenTK.Mathematics;
namespace Engine.PostProcess
{
    public class Skybox : PostProcessBase
    {
        public Skybox()
            : base()
        {
            // just draw on current binded render target
            bOwnItsRenderTarget = false;
            PostProcessMaterial = ShaderManager.Get().GetMaterial<CubemapMaterial>();

            sphereMeshObject = new StaticMeshObject("./Resources/Imported/StaticMesh/sphere3.staticmesh");
            sphereMeshObject.SetVisible(false);
        }

        public override void Render()
        {
            if (cubemapTexture == null)
            {
                return;
            }
            // caution
            // draw current binded render taret
            using (var dummy = new ScopedDepthFunc(DepthFunction.Lequal))
            {
                PostProcessMaterial.BindAndExecute(() =>
                {
                    var specificMaterial = (CubemapMaterial) PostProcessMaterial;
                    specificMaterial.ModelMatrix = Matrix4.CreateScale(10.0f) * Matrix4.CreateTranslation(CameraManager.Get().CurrentCameraEye);
                    specificMaterial.ViewMatrix = CameraManager.Get().CurrentCameraView;
                    specificMaterial.ProjMatrix = CameraManager.Get().CurrentCameraProj;
                    specificMaterial.TexCubemap2D = cubemapTexture;
                    specificMaterial.LightChannel = (int) Light.LightChannel.SkyBoxChannel;
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
