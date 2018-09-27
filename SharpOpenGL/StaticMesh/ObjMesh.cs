using Core;
using Core.Texture;
using OpenTK;
using System.Collections.Generic;
using System.Linq;
using ZeroFormatter;
using ObjMeshVertexAttribute = Core.Primitive.PNTT_VertexAttribute;


namespace SharpOpenGL.StaticMesh
{
    [ZeroFormattable]
    public class ObjMesh
    {
        // 
        // Vertex Buffer and Index buffer to render        
        TriangleDrawable<ObjMeshVertexAttribute> meshdrawable = null;

        Dictionary<string, Texture2D> TextureMap = new Dictionary<string, Texture2D>();

        // 
        protected bool bHasNormal = false;
        protected bool bHasTexCoordinate = false;

        [IgnoreFormat]
        public bool HasNormal => bHasNormal;
        [IgnoreFormat]
        public bool HasTexCoord => bHasTexCoordinate;
        
        [IgnoreFormat]
        public virtual Vector3 MinVertex { get; set; } = new Vector3(0, 0, 0);

        [IgnoreFormat]
        public virtual Vector3 MaxVertex { get; set; } = new Vector3(0, 0, 0);

        public StaticMeshAsset MeshAsset = null;

        public ObjMesh(StaticMeshAsset asset)
        {
            MeshAsset = asset;
        }

        public void DrawWithoutMtl(Core.MaterialBase.MaterialBase material)
        {
            meshdrawable.BindVertexAndIndexBuffer();
            meshdrawable.Draw(0, (uint) MeshAsset.VertexIndices.Count);
        }
        
        public void Draw(Core.MaterialBase.MaterialBase material)
        {
            meshdrawable.BindVertexAndIndexBuffer();
            
            foreach( var sectionlist in MeshAsset.MeshSectionList.GroupBy(x => x.SectionName))
            {
                var sectionName = sectionlist.First().SectionName;
                // setup
                if (MeshAsset.MaterialMap.ContainsKey(sectionName))
                {
                    material.SetTexture("DiffuseTex", TextureMap[MeshAsset.MaterialMap[sectionName].DiffuseMap]);

                    if (MeshAsset.MaterialMap[sectionName].NormalMap != null)
                    {
                        material.SetUniformVarData("NormalMapExist", 1);
                        material.SetTexture("NormalTex", TextureMap[MeshAsset.MaterialMap[sectionName].NormalMap]);
                    }
                    else
                    {
                        material.SetUniformVarData("NormalMapExist", 0);
                    }

                    if (MeshAsset.MaterialMap[sectionName].MaskMap != null)
                    {
                        material.SetUniformVarData("MaskMapExist", 1);
                        material.SetTexture("MaskTex", TextureMap[MeshAsset.MaterialMap[sectionName].MaskMap]);
                    }
                    else
                    {
                        material.SetUniformVarData("MaskMapExist", 0);
                    }

                    if (MeshAsset.MaterialMap[sectionName].SpecularMap != null)
                    {
                        material.SetUniformVarData("SpecularMapExist", 1);
                        material.SetTexture("SpecularTex", TextureMap[MeshAsset.MaterialMap[sectionName].SpecularMap]);
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
        
        public void PrepareToDraw()
        {
            meshdrawable = new TriangleDrawable<ObjMeshVertexAttribute>();         
            var Arr = MeshAsset.Vertices.ToArray();            
            var IndexArr = MeshAsset.VertexIndices.ToArray();            
            meshdrawable.SetupData(ref Arr, ref IndexArr);
            LoadTextures();
        }

        public void LoadTextures()
        {
            foreach (var Mtl in MeshAsset.MaterialMap)
            {
                if (Mtl.Value.DiffuseMap.Length > 0)
                {
                    if (!TextureMap.ContainsKey(Mtl.Value.DiffuseMap))
                    {
                        var TextureObj = new Texture2D();
                        TextureObj.Load(Mtl.Value.DiffuseMap);
                        TextureMap.Add(Mtl.Value.DiffuseMap, TextureObj);
                    }
                }

                if (Mtl.Value.NormalMap != null)
                {
                    if(!TextureMap.ContainsKey(Mtl.Value.NormalMap))
                    {
                        var textureObj = new Texture2D();
                        textureObj.Load(Mtl.Value.NormalMap);
                        TextureMap.Add(Mtl.Value.NormalMap, textureObj);
                    }
                }

                if (Mtl.Value.SpecularMap != null)
                {
                    if(!TextureMap.ContainsKey(Mtl.Value.SpecularMap))
                    {
                        var textureObj = new Texture2D();
                        textureObj.Load(Mtl.Value.SpecularMap);
                        TextureMap.Add(Mtl.Value.SpecularMap, textureObj);
                    }
                }

                if(Mtl.Value.MaskMap != null)
                {
                    if (!TextureMap.ContainsKey(Mtl.Value.MaskMap))
                    {
                        var textureObj = new Texture2D();
                        textureObj.Load(Mtl.Value.MaskMap);
                        TextureMap.Add(Mtl.Value.MaskMap, textureObj);
                    }
                }
            }
        }

        public int GetIndicesCount()
        {
            return MeshAsset.VertexIndices.Count();
        }

        
        
     
    }
}
