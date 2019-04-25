using Core;
using Core.Texture;
using OpenTK.Graphics.OpenGL;
using System;

namespace SharpOpenGL.PostProcess
{
    public class Skybox : PostProcessBase
    {
        public Skybox()
            : base()
        {
            // just draw on current binded render target
            bOwnItsRenderTarget = false;
        }

        public override void OnGLContextCreated(object sender, EventArgs e)
        {
            base.OnGLContextCreated(sender, e);
            
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
            // caution
            // draw current binded render taret
            using (var dummy = new ScopedDepthFunc(DepthFunction.Lequal))
            {
                PostProcessMaterial.BindAndExecute(() =>
                {
                    var specificMaterial = (CubemapMaterial.CubemapMaterial) PostProcessMaterial;
                    specificMaterial.ModelMatrix = OpenTK.Matrix4.CreateScale(10.0f) * OpenTK.Matrix4.CreateTranslation(CameraManager.Get().CurrentCameraEye);
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
