using Core;
using Core.CustomAttribute;
using Core.Primitive;
using Core.StaticMesh;
using OpenTK;
using Core.Asset;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CompiledMaterial.GBufferDraw;
using CompiledMaterial.TBNMaterial;
using OpenTK.Mathematics;

namespace Engine
{
    public class StaticMeshObject : GameObject
    {
        public StaticMeshObject(string assetpath)
        : base(Path.GetFileNameWithoutExtension(assetpath))
        {
            this.assetpath = assetpath;
            Initialize();
        }

        [ExposeUI, Range(0.0f,1.0f)]
        public float Roughness { get; set; } = 0.5f;

        [ExposeUI] public bool IsRoughnessOverride { get; set; } = false;

        [ExposeUI, Range(0.0f, 1.0f)]
        public float Metallic { get; set; } = 0.5f;

        [ExposeUI] public bool IsMetallicOverride { get; set; } = false;

        protected override void PrepareRenderingData()
        {
            meshdrawable = new TriangleDrawable<PNTT_VertexAttribute>();
            var Arr = meshAsset.Vertices.ToArray();
            var IndexArr = meshAsset.VertexIndices.ToArray();
            meshdrawable.SetupData(ref Arr, ref IndexArr);

            if (meshAsset.Debugging)
            {
                linedrawable = new LineDrawable<PC_VertexAttribute>();
                var lineArr = meshAsset.TBNVertices.ToArray();
                var lineIndexArr = meshAsset.TBNIndices.ToArray();
                linedrawable.SetupData(ref lineArr, ref lineIndexArr);
            }

            this.LoadTextures();

            bReadyToDraw = true;
        }

        public StaticMeshObject()
        : base("StaticMesh")
        {
        }

        public static async Task<StaticMeshObject> CreateStaticMeshObjectAsync(string assetPath)
        {
            var asset = await AssetManager.LoadAssetAsync<StaticMeshAsset>(assetPath);
            return new StaticMeshObject(asset);
        }

        public StaticMeshObject(StaticMeshAsset asset)
        : base("StaticMesh")
        {
            meshAsset = asset;

            RenderingThread.Instance.ExecuteImmediatelyIfRenderingThread
            (
                () =>
                {
                    PrepareRenderingData();
                }
            );
        }

        public override void Initialize()
        {
            meshAsset = AssetManager.LoadAssetSync<StaticMeshAsset>(assetpath);

            RenderingThread.Instance.ExecuteImmediatelyIfRenderingThread
            (
                () =>
                {
                    PrepareRenderingData();
                }
            );
        }
        
        public override Matrix4 LocalMatrix
        {
            get
            {
                return Matrix4.CreateScale(Scale) * Matrix4.CreateRotationY(Yaw) * Matrix4.CreateRotationX(Pitch) * Matrix4.CreateTranslation(Translation);
            }
        }

        public void DrawWithBindedMaterial()
        {
            if (bReadyToDraw == false)
            {
                return;
            }
            
            meshdrawable.Draw(0, (uint)(meshAsset.VertexIndices.Count));
            
        }

        public void DebugDraw()
        {
            if(bReadyToDraw == false)
            {
                return;
            }

            var material = ShaderManager.Instance.GetMaterial<TBNMaterial>();

            Debug.Assert(material != null);

            material.Bind();
            material.ModelTransform_Model = this.ParentMatrix * this.LocalMatrix;
            material.CameraTransform_View = CameraManager.Instance.CurrentCameraView;
            material.CameraTransform_Proj = CameraManager.Instance.CurrentCameraProj;

            if (linedrawable != null)
            {
                linedrawable.Draw();
            }
        }
        

        public override void Render()
        {
            if (bReadyToDraw == false)
            {
                return;
            }

            

            //DebugDraw();
        }
       
        protected string assetpath = "";

        protected StaticMeshAsset meshAsset = null;

        // for rendering
        TriangleDrawable<PNTT_VertexAttribute> meshdrawable = null;

        // for tbn debug rendering
        LineDrawable<PC_VertexAttribute> linedrawable = null;

        private void LoadTextures()
        {
            foreach (var Mtl in meshAsset.MaterialMap)
            {
                if (Mtl.Value.DiffuseMap.Length > 0)
                {
                    TextureManager.Instance.CacheTexture2D(Mtl.Value.DiffuseMap);
                }

                if (Mtl.Value.NormalMap != null)
                {
                    TextureManager.Instance.CacheTexture2D(Mtl.Value.NormalMap);
                }

                if (Mtl.Value.SpecularMap != null)
                {
                    TextureManager.Instance.CacheTexture2D(Mtl.Value.SpecularMap);
                }

                if (Mtl.Value.MaskMap != null)
                {
                    TextureManager.Instance.CacheTexture2D(Mtl.Value.MaskMap);
                }

                if (Mtl.Value.RoughnessMap != null)
                {
                    TextureManager.Instance.CacheTexture2D(Mtl.Value.RoughnessMap);
                }
            }
        }
    }
}
