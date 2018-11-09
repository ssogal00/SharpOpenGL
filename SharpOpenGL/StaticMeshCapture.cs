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

namespace SharpOpenGL
{
    public class StaticMeshCapture : Singleton<StaticMeshCapture>
    {
        public void Capture(string staticMeshAssetPath)
        {
            var staticMeshAsset = AssetManager.LoadAssetSync<StaticMeshAsset>(staticMeshAssetPath);

            gbufferMaterial = AssetManager.LoadAssetSync<MaterialBase>("GBufferDraw");

            // setup camera to capture static mesh
            freecam = new FreeCamera();
            freecam.LookAtLocation = staticMeshAsset.CenterVertex;
            freecam.Destination = freecam.EyeLocation = staticMeshAsset.CenterVertex + new Vector3(-staticMeshAsset.XExtent * 2, 0, 0);
            freecam.Yaw = 0;

            freecam.UpdateViewMatrix();
            freecam.UpdateProjMatrix();

            gbuffer.Initialize();
            
            {   
                gbuffer.BindAndExecute(gbufferMaterial,
                () =>
                {
                    //
                    gbuffer.Clear(Color.AntiqueWhite);
                    cameraTransform.Proj = freecam.Proj;
                    cameraTransform.View = freecam.View;

                    
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
        private GBuffer gbuffer = new GBuffer(1024,768);
        private FreeCamera freecam = null;
        private int width = 1024;
        private int height = 768;
        private float AspectRatio => (float) width / height;
        private CameraTransform cameraTransform = new CameraTransform();
        private  ModelTransform modelTransform = new ModelTransform();
    }
}
