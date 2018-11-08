using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Buffer;
using Core.Camera;
using Core.MaterialBase;
using OpenTK;
using SharpOpenGL.Asset;
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
            freecam = new FreeCamera(60.0f, (float) 1024 / 768, 1.0f, 10000.0f);
            freecam.LookAtLocation = staticMeshAsset.CenterVertex;
            freecam.Destination = staticMeshAsset.CenterVertex + new Vector3(staticMeshAsset.XExtent, 0, 0);

            using (gbuffer = new GBuffer(1024, 768))
            {
                gbuffer.BindAndExecute(gbufferMaterial,
                () =>
                {
                    //
                    
                });
            }
        }

        private MaterialBase gbufferMaterial = null;
        private GBuffer gbuffer = null;
        private FreeCamera freecam = null;
    }
}
