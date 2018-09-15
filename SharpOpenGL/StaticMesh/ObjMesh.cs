﻿using Core;
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

        protected float MinX = float.MaxValue;
        protected float MaxX = float.MinValue;

        protected float MinY = float.MaxValue;
        protected float MaxY = float.MinValue;

        protected float MinZ = float.MaxValue;
        protected float MaxZ = float.MinValue;       
        
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

            foreach (var section in MeshAsset.MeshSectionList)
            {
                if (MeshAsset.MaterialMap.ContainsKey(section.SectionName))
                {
                    material.SetTexture("DiffuseTex", TextureMap[MeshAsset.MaterialMap[section.SectionName].DiffuseMap]);

                    if(MeshAsset.MaterialMap[section.SectionName].NormalMap != null)
                    {
                        material.SetUniformVarData("NormalMapExist", 1);
                        material.SetTexture("NormalTex", TextureMap[MeshAsset.MaterialMap[section.SectionName].NormalMap]);
                    }
                    else
                    {
                        material.SetUniformVarData("NormalMapExist", 0);
                    }

                    if (MeshAsset.MaterialMap[section.SectionName].MaskMap != null)
                    {
                        material.SetUniformVarData("MaskMapExist", 1);
                        material.SetTexture("MaskTex", TextureMap[MeshAsset.MaterialMap[section.SectionName].MaskMap]);
                    }
                    else
                    {
                        material.SetUniformVarData("MaskMapExist", 0);
                    }

                    if(MeshAsset.MaterialMap[section.SectionName].SpecularMap != null)
                    {
                        material.SetUniformVarData("SpecularMapExist", 1);
                        material.SetTexture("SpecularTex", TextureMap[MeshAsset.MaterialMap[section.SectionName].SpecularMap]);
                    }
                    else
                    {
                        material.SetUniformVarData("SpecularMapExist", 0);
                    }
                }
                
                meshdrawable.Draw(section.StartIndex, (uint)(section.EndIndex - section.StartIndex));
            }
        }
        
        public void PrepareToDraw()
        {
            meshdrawable = new TriangleDrawable<ObjMeshVertexAttribute>();         
            var Arr = MeshAsset.Vertices.ToArray();            
            var IndexArr = MeshAsset.VertexIndices.ToArray();            
            meshdrawable.SetupData(ref Arr, ref IndexArr);            
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

        
        protected void UpdateMinMaxVertex(ref OpenTK.Vector3 newVertex)
        {
            // update X
            if(newVertex.X > MaxX)
            {
                MaxX = newVertex.X;
            }
            if(newVertex.X < MinX)
            {
                MinX = newVertex.X;
            }

            if (newVertex.Y > MaxY)
            {
                MaxY = newVertex.Y;
            }
            if (newVertex.Y < MinY)
            {
                MinY = newVertex.Y;
            }

            if (newVertex.Z > MaxZ)
            {
                MaxZ = newVertex.Z;
            }
            if (newVertex.Z < MinZ)
            {
                MinZ = newVertex.Z;
            }
        }
     
    }
}
