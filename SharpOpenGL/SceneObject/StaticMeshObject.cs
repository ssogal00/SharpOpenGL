
using System.Collections.Generic;
using System.Linq;
using Core;
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
        public StaticMeshObject(string assetpath)
        {
            this.assetpath = assetpath;
            Initialize();
        }

        public StaticMeshObject(StaticMeshAsset asset)
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
                return Matrix4.CreateScale(Scale) * Matrix4.CreateTranslation(Translation);
            }
        }

        public void Draw()
        {
            meshdrawable.BindVertexAndIndexBuffer();
            meshdrawable.Draw(0, (uint)meshAsset.VertexIndices.Count);
        }

        public override void Draw(MaterialBase material)
        {
            if (bReadyToDraw == false)
            {
                return;
            }

            meshdrawable.BindVertexAndIndexBuffer();

            if (meshAsset.MaterialMap.Count == 0)
            {
                material.SetTexture("DiffuseTex", textureMap["Default"]);
                meshdrawable.Draw(0, (uint)(meshAsset.VertexIndices.Count));
                return;
            }

            foreach (var sectionlist in meshAsset.MeshSectionList.GroupBy(x => x.SectionName))
            {
                var sectionName = sectionlist.First().SectionName;
                // setup
                if (meshAsset.MaterialMap.ContainsKey(sectionName))
                {
                    var diffuseTex = TextureManager.Get().GetTexture2D(meshAsset.MaterialMap[sectionName].DiffuseMap);
                    material.SetTexture("DiffuseTex", diffuseTex);

                    if (meshAsset.MaterialMap[sectionName].NormalMap != null)
                    {
                        material.SetUniformVarData("NormalMapExist", 1);
                        var normalTex = TextureManager.Get().GetTexture2D(meshAsset.MaterialMap[sectionName].NormalMap);
                        material.SetTexture("NormalTex", normalTex);
                    }
                    else
                    {
                        material.SetUniformVarData("NormalMapExist", 0);
                    }

                    if (meshAsset.MaterialMap[sectionName].MaskMap != null)
                    {
                        material.SetUniformVarData("MaskMapExist", 1);
                        var maskTex = TextureManager.Get().GetTexture2D(meshAsset.MaterialMap[sectionName].MaskMap);
                        material.SetTexture("MaskTex", maskTex);
                    }
                    else
                    {
                        material.SetUniformVarData("MaskMapExist", 0);
                    }

                    if (meshAsset.MaterialMap[sectionName].SpecularMap != null)
                    {
                        material.SetUniformVarData("SpecularMapExist", 1);
                        var specTex = TextureManager.Get().GetTexture2D(meshAsset.MaterialMap[sectionName].SpecularMap);
                        material.SetTexture("SpecularTex", specTex);
                    }
                    else
                    {
                        material.SetUniformVarData("SpecularMapExist", 0);
                    }
                }

                foreach (var section in sectionlist)
                {
                    meshdrawable.Draw(section.StartIndex, (uint)(section.EndIndex - section.StartIndex));
                }
            }
        }

        protected string assetpath = "";

        protected StaticMeshAsset meshAsset = null;

        // for rendering
        TriangleDrawable<PNTT_VertexAttribute> meshdrawable = null;
        Dictionary<string, Texture2D> textureMap = new Dictionary<string, Texture2D>();

        private void LoadTextures()
        {
            var DefaultTexObj = new Texture2D();
            DefaultTexObj.Load("./Resources/Texture/Checker.png");
            textureMap.Add("Default", DefaultTexObj);

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
            }
        }
    }
}
