
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Core;
using Core.CustomAttribute;
using Core.MaterialBase;
using Core.Primitive;
using Core.Texture;
using OpenTK;
using SharpOpenGL.Asset;
using SharpOpenGL.StaticMesh;

namespace SharpOpenGL
{
    public class StaticMeshObject : SceneObject
    {
        private static int StaticMeshCount = 0;
        public StaticMeshObject(string assetpath)
        : base("StaticMesh", StaticMeshCount++)
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

        public StaticMeshObject(StaticMeshAsset asset)
        : base("StaticMesh", StaticMeshCount++)
        {
            RenderingThread.Get().ExecuteImmediatelyIfRenderingThread
            (
                () =>
                {
                    meshdrawable = new TriangleDrawable<PNTT_VertexAttribute>();
                    var Arr = meshAsset.Vertices.ToArray();
                    var IndexArr = meshAsset.VertexIndices.ToArray();
                    meshdrawable.SetupData(ref Arr, ref IndexArr);

                    this.LoadTextures();
                    bReadyToDraw = true;
                }
            );
        }

        public override void Initialize()
        {
            meshAsset = AssetManager.LoadAssetSync<StaticMeshAsset>(assetpath);

            RenderingThread.Get().ExecuteImmediatelyIfRenderingThread
            (
                () =>
                {
                    meshdrawable = new TriangleDrawable<PNTT_VertexAttribute>();
                    var Arr = meshAsset.Vertices.ToArray();
                    var IndexArr = meshAsset.VertexIndices.ToArray();
                    meshdrawable.SetupData(ref Arr, ref IndexArr);

                    this.LoadTextures();
                    bReadyToDraw = true;
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
            meshdrawable.BindVertexAndIndexBuffer();
            meshdrawable.Draw(0, (uint)(meshAsset.VertexIndices.Count));
        }

        public override void Draw()
        {
            if (bReadyToDraw == false)
            {
                return;
            }

            var gbufferMaterial = ShaderManager.Get().GetMaterial<GBufferDraw.GBufferDraw>();
            Debug.Assert(gbufferMaterial != null);

            gbufferMaterial.Bind();
            
            gbufferMaterial.ModelTransform_Model = this.ParentMatrix * this.LocalMatrix;
            gbufferMaterial.CameraTransform_Proj = CameraManager.Get().CurrentCameraProj;
            gbufferMaterial.CameraTransform_View = CameraManager.Get().CurrentCameraView;

            meshdrawable.BindVertexAndIndexBuffer();

            if (meshAsset.MaterialMap.Count == 0)
            {
                gbufferMaterial.SetTexture("DiffuseTex",
                    TextureManager.Get().GetTexture2D("./Resources/Texture/Checker.png"));
                meshdrawable.Draw(0, (uint) (meshAsset.VertexIndices.Count));
                return;
            }

            foreach (var sectionlist in meshAsset.MeshSectionList.GroupBy(x => x.SectionName))
            {
                var sectionName = sectionlist.First().SectionName;
                // setup
                if (meshAsset.MaterialMap.ContainsKey(sectionName))
                {
                    var diffuseTex = TextureManager.Get().GetTexture2D(meshAsset.MaterialMap[sectionName].DiffuseMap);
                    gbufferMaterial.DiffuseMapExist = 1;
                    gbufferMaterial.DiffuseTex2D = diffuseTex;

                    if (meshAsset.MaterialMap[sectionName].NormalMap != null)
                    {
                        gbufferMaterial.NormalMapExist = 1;
                        var normalTex = TextureManager.Get().GetTexture2D(meshAsset.MaterialMap[sectionName].NormalMap);
                        gbufferMaterial.NormalTex2D = normalTex;
                    }
                    else
                    {
                        gbufferMaterial.NormalMapExist = 0;
                    }

                    if (meshAsset.MaterialMap[sectionName].MaskMap != null)
                    {
                        gbufferMaterial.MaskMapExist = 1;
                        var maskTex = TextureManager.Get().GetTexture2D(meshAsset.MaterialMap[sectionName].MaskMap);
                        gbufferMaterial.MaskTex2D = maskTex;
                    }
                    else
                    {
                        gbufferMaterial.MaskMapExist = 0;
                    }

                    if (IsMetallicOverride)
                    {
                        gbufferMaterial.MetalicExist = 0;
                        gbufferMaterial.Metalic = this.Metallic;
                    }
                    else if(meshAsset.MaterialMap[sectionName].SpecularMap != null)
                    {
                        gbufferMaterial.MetalicExist = 1;
                        var specTex = TextureManager.Get().GetTexture2D(meshAsset.MaterialMap[sectionName].SpecularMap);
                        gbufferMaterial.MetalicTex2D = specTex;
                    }
                    else
                    {
                        gbufferMaterial.MetalicExist = 0;
                        gbufferMaterial.Metalic = DebugDrawer.Get().SceneMetallic;
                    }

                    if(IsRoughnessOverride)
                    {
                        gbufferMaterial.RoughnessExist = 0;
                        gbufferMaterial.Roughness = this.Roughness;
                    }
                    else if(meshAsset.MaterialMap[sectionName].RoughnessMap != null)
                    {
                        gbufferMaterial.RoughnessExist = 1;
                        var roughnessTex = TextureManager.Get().GetTexture2D(meshAsset.MaterialMap[sectionName].RoughnessMap);
                        gbufferMaterial.RoughnessTex2D = roughnessTex;
                    }
                    else
                    {
                        gbufferMaterial.RoughnessExist = 0;
                        gbufferMaterial.Roughness = DebugDrawer.Get().SceneRoughness;
                    }
                }

                foreach (var section in sectionlist)
                {
                    meshdrawable.Draw(section.StartIndex, (uint) (section.EndIndex - section.StartIndex));
                }
            }
        }
       
        protected string assetpath = "";

        protected StaticMeshAsset meshAsset = null;

        // for rendering
        TriangleDrawable<PNTT_VertexAttribute> meshdrawable = null;

        private void LoadTextures()
        {
            foreach (var Mtl in meshAsset.MaterialMap)
            {
                if (Mtl.Value.DiffuseMap.Length > 0)
                {
                    TextureManager.Get().CacheTexture2D(Mtl.Value.DiffuseMap);
                }

                if (Mtl.Value.NormalMap != null)
                {
                    TextureManager.Get().CacheTexture2D(Mtl.Value.NormalMap);
                }

                if (Mtl.Value.SpecularMap != null)
                {
                    TextureManager.Get().CacheTexture2D(Mtl.Value.SpecularMap);
                }

                if (Mtl.Value.MaskMap != null)
                {
                    TextureManager.Get().CacheTexture2D(Mtl.Value.MaskMap);
                }

                if (Mtl.Value.RoughnessMap != null)
                {
                    TextureManager.Get().CacheTexture2D(Mtl.Value.RoughnessMap);
                }
            }
        }
    }
}
