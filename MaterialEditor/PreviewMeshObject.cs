using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Asset;
using Core.Primitive;
using Core.StaticMesh;

namespace MaterialEditor
{
    public class PreviewMesh
    {
        public PreviewMesh(string assetPath)
        {
            var asset = AssetManager.LoadAssetSync<StaticMeshAsset>(assetPath);
            var vertexArray = asset.Vertices.ToArray();
            var indexArray = asset.VertexIndices.ToArray();

            meshDrawable.SetupData(ref vertexArray, ref indexArray);
        }

        public void Draw()
        {
            if (meshDrawable != null)
            {
                meshDrawable.Draw();
            }
        }

        private TriangleDrawable<PNTT_VertexAttribute> meshDrawable = new TriangleDrawable<PNTT_VertexAttribute>();
    }
}
