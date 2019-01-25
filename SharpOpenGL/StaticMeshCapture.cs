using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Buffer;
using Core.Camera;
using Core.MaterialBase;
using Core.Texture;
using OpenTK;
using SharpOpenGL.Asset;
using SharpOpenGL.BasicMaterial;
using SharpOpenGL.Font;
using SharpOpenGL.GBufferDraw;
using SharpOpenGL.StaticMesh;
using OpenTK.Platform.Windows;


namespace SharpOpenGL
{
    public class StaticMeshCapture : Singleton<StaticMeshCapture>
    {
        public void Capture(string staticMeshAssetPath)
        {
            var staticMeshAsset = AssetManager.LoadAssetSync<StaticMeshAsset>(staticMeshAssetPath);

            gbufferMaterial = ShaderManager.Get().GetMaterial("GBufferDraw");

            // setup camera to capture static mesh
            orbitcam = new OrbitCamera();
            orbitcam.LookAtLocation = staticMeshAsset.CenterVertex;

            var dir = new Vector3(1, 1, 1);
            dir.Normalize();

            orbitcam.EyeLocation = staticMeshAsset.CenterVertex + staticMeshAsset.XExtent * 1.5f *  dir;
            orbitcam.UpdateViewMatrix();
            orbitcam.UpdateProjMatrix();

            gbuffer.Initialize();
            {   
                gbuffer.BindAndExecute(gbufferMaterial,
                () =>
                {
                    //
                    gbuffer.Clear(Color.Black);
                    cameraTransform.Proj = orbitcam.Proj;
                    cameraTransform.View = orbitcam.View;
                    
                    modelTransform.Model = Matrix4.CreateScale(1.0f);
                    
                    gbufferMaterial.SetUniformBufferValue<GBufferDraw.CameraTransform>("CameraTransform", ref cameraTransform);
                    gbufferMaterial.SetUniformBufferValue<ModelTransform>("ModelTransform", ref modelTransform);
                    staticMeshAsset.Draw(gbufferMaterial);

                    FontManager.Get().RenderText(10, 100, "Hello");
                    //
                    
                });

                var colorData = gbuffer.GetColorAttachement.GetTexImage();
                FreeImageHelper.SaveAsBmp(ref colorData, width, height, "staticmesh.bmp");
            }
        }

        private MaterialBase gbufferMaterial = null;
        private GBuffer gbuffer = new GBuffer(640,480);
        private OrbitCamera orbitcam = null;
        private int width = 640;
        private int height = 480;
        private float AspectRatio => (float) width / height;
        private CameraTransform cameraTransform = new CameraTransform();
        private  ModelTransform modelTransform = new ModelTransform();
    }
}
