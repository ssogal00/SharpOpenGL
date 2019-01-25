
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
                    material.SetTexture("DiffuseTex", textureMap[meshAsset.MaterialMap[sectionName].DiffuseMap]);

                    if (meshAsset.MaterialMap[sectionName].NormalMap != null)
                    {
                        material.SetUniformVarData("NormalMapExist", 1);
                        material.SetTexture("NormalTex", textureMap[meshAsset.MaterialMap[sectionName].NormalMap]);
                    }
                    else
                    {
                        material.SetUniformVarData("NormalMapExist", 0);
                    }

                    if (meshAsset.MaterialMap[sectionName].MaskMap != null)
                    {
                        material.SetUniformVarData("MaskMapExist", 1);
                        material.SetTexture("MaskTex", textureMap[meshAsset.MaterialMap[sectionName].MaskMap]);
                    }
                    else
                    {
                        material.SetUniformVarData("MaskMapExist", 0);
                    }

                    if (meshAsset.MaterialMap[sectionName].SpecularMap != null)
                    {
                        material.SetUniformVarData("SpecularMapExist", 1);
                        material.SetTexture("SpecularTex", textureMap[meshAsset.MaterialMap[sectionName].SpecularMap]);
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
                    if (!textureMap.ContainsKey(Mtl.Value.DiffuseMap))
                    {
                        var TextureObj = new Texture2D();
                        TextureObj.Load(Mtl.Value.DiffuseMap);
                        textureMap.Add(Mtl.Value.DiffuseMap, TextureObj);
                    }
                }

                if (Mtl.Value.NormalMap != null)
                {
                    if (!textureMap.ContainsKey(Mtl.Value.NormalMap))
                    {
                        var textureObj = new Texture2D();
                        textureObj.Load(Mtl.Value.NormalMap);
                        textureMap.Add(Mtl.Value.NormalMap, textureObj);
                    }
                }

                if (Mtl.Value.SpecularMap != null)
                {
                    if (!textureMap.ContainsKey(Mtl.Value.SpecularMap))
                    {
                        var textureObj = new Texture2D();
                        textureObj.Load(Mtl.Value.SpecularMap);
                        textureMap.Add(Mtl.Value.SpecularMap, textureObj);
                    }
                }

                if (Mtl.Value.MaskMap != null)
                {
                    if (!textureMap.ContainsKey(Mtl.Value.MaskMap))
                    {
                        var textureObj = new Texture2D();
                        textureObj.Load(Mtl.Value.MaskMap);
                        textureMap.Add(Mtl.Value.MaskMap, textureObj);
                    }
                }
            }
        }
    }
}
