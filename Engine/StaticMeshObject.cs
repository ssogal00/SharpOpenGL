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

            var gbufferMaterial = ShaderManager.Instance.GetMaterial<GBufferDraw>();

            Debug.Assert(gbufferMaterial != null);

            gbufferMaterial.Bind();

            gbufferMaterial.LightChannel = (int) Light.LightChannel.StaticMeshChannel;

            gbufferMaterial.PrevTransform_PrevModel = this.ParentMatrix * this.LocalMatrix;
            gbufferMaterial.ModelTransform_Model = this.ParentMatrix * this.LocalMatrix;

            gbufferMaterial.PrevTransform_PrevProj = CameraManager.Instance.PrevCameraProj;
            gbufferMaterial.PrevTransform_PrevView = CameraManager.Instance.PrevCameraView;

            gbufferMaterial.CameraTransform_Proj = CameraManager.Instance.CurrentCameraProj;
            gbufferMaterial.CameraTransform_View = CameraManager.Instance.CurrentCameraView;

            if (meshAsset.MaterialMap.Count == 0)
            {
                gbufferMaterial.SetTexture("DiffuseTex",
                    TextureManager.Instance.GetTexture2D("./Resources/Texture/Checker.png"));
                meshdrawable.Draw(0, (uint) (meshAsset.VertexIndices.Count));
                return;
            }

            foreach (var section in meshAsset.MeshSectionList)
            {
                var sectionName = section.SectionName;
                // setup
                if (meshAsset.MaterialMap.ContainsKey(sectionName))
                {
                    var diffuseTex = TextureManager.Instance.GetTexture2D(meshAsset.MaterialMap[sectionName].DiffuseMap);
                    gbufferMaterial.DiffuseMapExist = true;
                    gbufferMaterial.DiffuseTex2D = diffuseTex;

                    if (meshAsset.MaterialMap[sectionName].NormalMap != null)
                    {
                        gbufferMaterial.NormalMapExist = true;
                        var normalTex = TextureManager.Instance.GetTexture2D(meshAsset.MaterialMap[sectionName].NormalMap);
                        gbufferMaterial.NormalTex2D = normalTex;
                    }
                    else
                    {
                        gbufferMaterial.NormalMapExist = false;
                    }

                    if (meshAsset.MaterialMap[sectionName].MaskMap != null)
                    {
                        gbufferMaterial.MaskMapExist = true;
                        var maskTex = TextureManager.Instance.GetTexture2D(meshAsset.MaterialMap[sectionName].MaskMap);
                        gbufferMaterial.MaskTex2D = maskTex;
                    }
                    else
                    {
                        gbufferMaterial.MaskMapExist = false;
                    }

                    if (IsMetallicOverride)
                    {
                        gbufferMaterial.MetallicExist = false;
                        gbufferMaterial.Metalic = this.Metallic;
                    }
                    else if(meshAsset.MaterialMap[sectionName].SpecularMap != null)
                    {
                        gbufferMaterial.MetallicExist = true;
                        var specTex = TextureManager.Instance.GetTexture2D(meshAsset.MaterialMap[sectionName].SpecularMap);
                        gbufferMaterial.MetallicTex2D = specTex;
                    }
                    else
                    {
                        gbufferMaterial.MetallicExist = false;
                        gbufferMaterial.Metalic = DebugDrawer.Instance.SceneMetallic;
                    }

                    if(IsRoughnessOverride)
                    {
                        gbufferMaterial.RoughnessExist = false;
                        gbufferMaterial.Roughness = this.Roughness;
                    }
                    else if(meshAsset.MaterialMap[sectionName].RoughnessMap != null)
                    {
                        gbufferMaterial.RoughnessExist = true;
                        var roughnessTex = TextureManager.Instance.GetTexture2D(meshAsset.MaterialMap[sectionName].RoughnessMap);
                        gbufferMaterial.RoughnessTex2D = roughnessTex;
                    }
                    else
                    {
                        gbufferMaterial.RoughnessExist = false;
                        gbufferMaterial.Roughness = DebugDrawer.Instance.SceneRoughness;
                    }
                }

                meshdrawable.Draw(section.StartIndex, (uint) (section.EndIndex - section.StartIndex));

            }
            gbufferMaterial.LightChannel = (int)Light.LightChannel.SkyBoxChannel;

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
