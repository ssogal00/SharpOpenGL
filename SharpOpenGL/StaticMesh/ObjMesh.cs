using Core.Buffer;
using Core.Texture;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Diagnostics;
using ZeroFormatter;
using ZeroFormatter.Internal;
using Core;

using ObjMeshVertexAttribute = Core.Primitive.PNTT_VertexAttribute;


namespace SharpOpenGL.StaticMesh
{
    [ZeroFormattable]
    public class ObjMesh
    {
        // 
        // Vertex Buffer and Index buffer to render        
        TriangleDrawable<ObjMeshVertexAttribute> meshdrawable = null;

        // serialized fields
        // vertices
        [Index(0)]
        public virtual List<ObjMeshVertexAttribute> Vertices { get; protected set;} = new List<ObjMeshVertexAttribute>();

        // serialized mesh sections
        [Index(1)]
        public virtual List<ObjMeshSection> MeshSectionList { get; protected set; } = new List<ObjMeshSection>();

        // serialized material map
        [Index(2)]
        public virtual Dictionary<string, ObjMeshMaterial> MaterialMap { get; protected set; } = new Dictionary<string, ObjMeshMaterial>();

        // serialized vertex indices
        [Index(3)]
        public virtual List<uint> VertexIndices { get; protected set; } = new List<uint>();

        // only used for mesh loading
        // will be cleared after mesh load
        List<Vector3> VertexList = new List<Vector3>();        
        List<Vector2> TexCoordList = new List<Vector2>();        
        List<Vector3> NormalList = new List<Vector3>();
        List<Vector4> TangentList = new List<Vector4>();

        List<uint> VertexIndexList = new List<uint>();
        List<uint> TexCoordIndexList = new List<uint>();
        List<uint> NormalIndexList = new List<uint>();        
                

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
        
       
        public void SaveSerialized(string path)
        {   
            var bytesarray = ZeroFormatter.ZeroFormatterSerializer.Serialize<ObjMesh>(this);
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                fs.Write(bytesarray, 0, bytesarray.Count());
            }
        }

        public static ObjMesh LoadSerialized(string path)
        {
            byte[] data = File.ReadAllBytes(path);
            ObjMesh result = ZeroFormatter.ZeroFormatterSerializer.Deserialize<ObjMesh>(data);
            return result;
        }

        public static async Task<ObjMesh> LoadSerializedAsync(string path)
        {
            Debug.Assert(File.Exists(path));
            ObjMesh result = new ObjMesh();

            await Task.Factory.StartNew(
            () =>
            {
                byte[] data = File.ReadAllBytes(path);
                result = ZeroFormatter.ZeroFormatterSerializer.Deserialize<ObjMesh>(data);
            });

            return result;
        }
        

        public static async Task<ObjMesh> ImportMeshAsync(string FilePath, string MtlPath)
        {
            Debug.Assert(File.Exists(FilePath) && File.Exists(MtlPath));

            ObjMesh result = new ObjMesh();

            await Task.Factory.StartNew(() =>
            {
                result.Load(FilePath, MtlPath);                
            });
            return result;
        }
        
        public ObjMesh()
        {
        }

        protected void GenerateVertices()
        {
            for (int i = 0; i < VertexIndexList.Count(); ++i)
            {
                var V1 = new ObjMeshVertexAttribute();
                V1.VertexPosition = VertexList[(int)VertexIndexList[i]];
                V1.TexCoord = TexCoordList[(int)TexCoordIndexList[i]];
                V1.VertexNormal = NormalList[(int)NormalIndexList[i]];
                V1.Tangent = TangentList[(int)VertexIndexList[i]];
                Vertices.Add(V1);
            }            
        }

        protected void GenerateTangents()
        {
            List<Vector3> tan1Accum = new List<Vector3>();
            List<Vector3> tan2Accum = new List<Vector3>();

            for(uint i = 0; i < VertexList.Count(); ++i)
            {
                tan1Accum.Add(new Vector3(0, 0, 0));
                tan2Accum.Add(new Vector3(0, 0, 0));
            }
            
            for (uint i = 0; i < VertexIndexList.Count(); i++)
            {                
                TangentList.Add(new Vector4(0,0,0,0));
            }

            // Compute the tangent vector
            for (uint i = 0; i < VertexIndexList.Count(); i += 3)
            {
                var p1 = VertexList[(int)VertexIndexList[(int)i]];
                var p2 = VertexList[(int)VertexIndexList[(int)i + 1]];
                var p3 = VertexList[(int)VertexIndexList[(int)i + 2]];

                var tc1 = TexCoordList[(int)TexCoordIndexList[(int)i]];
                var tc2 = TexCoordList[(int)TexCoordIndexList[(int)i + 1]];
                var tc3 = TexCoordList[(int)TexCoordIndexList[(int)i + 2]];

                Vector3 q1 = p2 - p1;
                Vector3 q2 = p3 - p1;
                float s1 = tc2.X - tc1.X, s2 = tc3.X - tc1.X;
                float t1 = tc2.Y - tc1.Y, t2 = tc3.Y - tc1.Y;

                // prevent degeneration
                float r = 1.0f / (s1 * t2 - s2 * t1);
                if (Single.IsInfinity(r))
                {
                    r = 1 / 0.1f;
                }

                var tan1 = new Vector3((t2 * q1.X - t1 * q2.X) * r,
                   (t2 * q1.Y - t1 * q2.Y) * r,
                   (t2 * q1.Z - t1 * q2.Z) * r);

                var tan2 = new Vector3((s1 * q2.X - s2 * q1.X) * r,
                   (s1 * q2.Y - s2 * q1.Y) * r,
                   (s1 * q2.Z - s2 * q1.Z) * r);             


                tan1Accum[(int)VertexIndexList[(int)i]] += tan1;
                tan1Accum[(int)VertexIndexList[(int)i + 1]] += tan1;
                tan1Accum[(int)VertexIndexList[(int)i + 2]] += tan1;                

                tan2Accum[(int)VertexIndexList[(int)i]] += tan2;
                tan2Accum[(int)VertexIndexList[(int)i + 1]] += tan2;
                tan2Accum[(int)VertexIndexList[(int)i + 2]] += tan2;                
            }

            Vector4 lastValidTangent = new Vector4();

            for(uint i = 0; i< VertexIndexList.Count(); ++i )
            {
                var n = NormalList[(int)NormalIndexList[(int)i]];
                var t1 = tan1Accum[(int)VertexIndexList[(int)i]];
                var t2 = tan2Accum[(int)VertexIndexList[(int)i]];               

                // Gram-Schmidt orthogonalize                
                var temp = OpenTK.Vector3.Normalize(t1 - (OpenTK.Vector3.Dot(n, t1) * n));
                // Store handedness in w                
                var W = (OpenTK.Vector3.Dot(OpenTK.Vector3.Cross(n, t1), t2) < 0.0f) ? -1.0f : 1.0f;

                bool bValid = true;
                if(Single.IsNaN(temp.X) || Single.IsNaN(temp.Y) || Single.IsNaN(temp.Z))
                {
                    bValid = false;
                }

                if (Single.IsInfinity(temp.X) || Single.IsInfinity(temp.Y) || Single.IsInfinity(temp.Z))
                {
                    bValid = false;
                }

                if(bValid == true)
                {
                    lastValidTangent = new Vector4(temp.X, temp.Y, temp.Z, W);
                }

                if(bValid == false)
                {
                    temp = lastValidTangent.Xyz;
                }

                TangentList[(int)i] = new Vector4(temp.X, temp.Y, temp.Z, W);
            }
            
            tan1Accum.Clear();
            tan2Accum.Clear();
        }

        public void Draw(Core.MaterialBase.MaterialBase material)
        {
            meshdrawable.BindVertexAndIndexBuffer();

            foreach (var section in MeshSectionList)
            {
                if (MaterialMap.ContainsKey(section.SectionName))
                {
                    material.SetTexture("DiffuseTex", TextureMap[MaterialMap[section.SectionName].DiffuseMap]);

                    if(MaterialMap[section.SectionName].NormalMap != null)
                    {
                        material.SetUniformVarData("NormalMapExist", 1);
                        material.SetTexture("NormalTex", TextureMap[MaterialMap[section.SectionName].NormalMap]);
                    }
                    else
                    {
                        material.SetUniformVarData("NormalMapExist", 0);
                    }

                    if (MaterialMap[section.SectionName].MaskMap != null)
                    {
                        material.SetUniformVarData("MaskMapExist", 1);
                        material.SetTexture("MaskTex", TextureMap[MaterialMap[section.SectionName].MaskMap]);
                    }
                    else
                    {
                        material.SetUniformVarData("MaskMapExist", 0);
                    }

                    if(MaterialMap[section.SectionName].SpecularMap != null)
                    {
                        material.SetUniformVarData("SpecularMapExist", 1);
                        material.SetTexture("SpecularTex", TextureMap[MaterialMap[section.SectionName].SpecularMap]);
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
            var Arr = Vertices.ToArray();            
            var IndexArr = VertexIndices.ToArray();            
            meshdrawable.SetupData(ref Arr, ref IndexArr);            
        }

        public void LoadTextures()
        {
            foreach (var Mtl in MaterialMap)
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
            return VertexIndices.Count();
        }

        protected void ParseMtlFile(string MtlPath)
        {
            if(File.Exists(MtlPath))
            {
                var Lines = File.ReadAllLines(MtlPath);
                ObjMeshMaterial NewMaterial = null;
                foreach (var line in Lines)
                {
                    var TrimmedLine = line.TrimStart(new char[] { ' ', '\t' });

                    if (TrimmedLine.StartsWith("newmtl"))
                    {
                        var Tokens = TrimmedLine.Split(new char[]{ ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        if(Tokens.Count() == 2)
                        {
                            NewMaterial = new ObjMeshMaterial();                            
                            NewMaterial.MaterialName = Tokens[1];
                        }
                    }
                    else if(TrimmedLine.StartsWith("map_Kd"))
                    {
                        var Tokens = TrimmedLine.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries); 
                        if(Tokens.Count() == 2)
                        {
                            if(NewMaterial != null)
                            {
                                NewMaterial.DiffuseMap = Tokens[1];
                            }
                        }
                    }
                    else if(TrimmedLine.StartsWith("map_bump"))
                    {
                        var tokens = TrimmedLine.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        if(tokens.Count() == 2)
                        {
                            if(NewMaterial != null)
                            {
                                NewMaterial.NormalMap = tokens[1];
                            }
                        }
                    }
                    else if(TrimmedLine.StartsWith("map_Ka"))
                    {
                        var tokens = TrimmedLine.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        if(tokens.Count() == 2)
                        {
                            if(NewMaterial != null)
                            {
                                NewMaterial.SpecularMap = tokens[1];
                            }
                        }
                    }
                    else if(TrimmedLine.StartsWith("map_Ns"))
                    {
                        var tokens = TrimmedLine.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        if (tokens.Count() == 2)
                        {
                            if (NewMaterial != null)
                            {
                                NewMaterial.RoughnessMap = tokens[1];
                            }
                        }
                    }
                    else if(TrimmedLine.StartsWith("map_d"))
                    {
                        var tokens = TrimmedLine.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        if (tokens.Count() == 2)
                        {
                            if (NewMaterial != null)
                            {
                                NewMaterial.MaskMap = tokens[1];
                            }
                        }
                    }
                    else if(TrimmedLine.Length == 0 && NewMaterial != null)
                    {
                        MaterialMap.Add(NewMaterial.MaterialName, NewMaterial);
                        NewMaterial = null;
                    }
                }

                if(NewMaterial != null)
                {
                    MaterialMap.Add(NewMaterial.MaterialName, NewMaterial);
                    NewMaterial = null;
                }
            }
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
                
        public void Load(string FilePath, string MtlPath)
        {
            ParseMtlFile(MtlPath);

            if(File.Exists(FilePath))
            {
                var Lines = File.ReadAllLines(FilePath);
                
                foreach(var line in Lines)
                {
                    var Trimmedline = line.TrimStart(new char[]{ ' ', '\t'});

                    if(Trimmedline.StartsWith("vn"))
                    {
                        var tokens = Trimmedline.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        bHasNormal = true;

                        if (tokens.Count() >= 4)
                        {
                            Vector3 VN = new Vector3();
                            VN.X = Convert.ToSingle(tokens[1]);
                            VN.Y = Convert.ToSingle(tokens[2]);
                            VN.Z = Convert.ToSingle(tokens[3]);

                            NormalList.Add(VN);
                        }
                    }
                    else if(Trimmedline.StartsWith("v "))
                    {
                        var tokens = Trimmedline.Split(new char[] {' ','\t'}, StringSplitOptions.RemoveEmptyEntries);

                        if(tokens.Count() == 4)
                        {
                            Vector3 V = new Vector3();
                            V.X = Convert.ToSingle(tokens[1]);
                            V.Y = Convert.ToSingle(tokens[2]);
                            V.Z = Convert.ToSingle(tokens[3]);

                            UpdateMinMaxVertex(ref V);

                            VertexList.Add(V);
                        }
                    }
                    else if(Trimmedline.StartsWith("vt"))
                    {
                        bHasTexCoordinate = true;

                        var tokens = Trimmedline.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        if(tokens.Count() >= 3)
                        {
                            Vector2 V = new Vector2();
                            V.X = Convert.ToSingle(tokens[1]);
                            V.Y = Convert.ToSingle(tokens[2]);

                            TexCoordList.Add(V);
                        }
                    }
                    else if(Trimmedline.StartsWith("f "))
                    {
                        var tokens = Trimmedline.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        if(tokens.Count() == 4)
                        {
                            var Token1 = tokens[1].Split('/');
                            var Token2 = tokens[2].Split('/');
                            var Token3 = tokens[3].Split('/');
                            
                            uint Index1 = Convert.ToUInt32(Token1[0]);
                            uint Index2 = Convert.ToUInt32(Token2[0]);
                            uint Index3 = Convert.ToUInt32(Token3[0]);

                            VertexIndexList.Add(Index1 - 1);
                            VertexIndexList.Add(Index2 - 1);
                            VertexIndexList.Add(Index3 - 1);
                            
                            uint TexIndex1 = Convert.ToUInt32(Token1[1]);
                            uint TexIndex2 = Convert.ToUInt32(Token2[1]);
                            uint TexIndex3 = Convert.ToUInt32(Token3[1]);

                            TexCoordIndexList.Add(TexIndex1 - 1);
                            TexCoordIndexList.Add(TexIndex2 - 1);
                            TexCoordIndexList.Add(TexIndex3 - 1);                            

                            uint NormIndex1 = Convert.ToUInt32(Token1[2]);
                            uint NormIndex2 = Convert.ToUInt32(Token2[2]);
                            uint NormIndex3 = Convert.ToUInt32(Token3[2]);

                            NormalIndexList.Add(NormIndex1 - 1);
                            NormalIndexList.Add(NormIndex2 - 1);
                            NormalIndexList.Add(NormIndex3 - 1);                         

                            VertexIndices.Add((uint)VertexIndices.Count);
                            VertexIndices.Add((uint)VertexIndices.Count);
                            VertexIndices.Add((uint)VertexIndices.Count);
                        }
                    }
                    else if(Trimmedline.StartsWith("usemtl"))
                    {
                        var MtlLine = Trimmedline.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        
                        if(MtlLine.Count() == 2)
                        {
                            if(MeshSectionList.Count() == 0)
                            {
                                ObjMeshSection NewSection = new ObjMeshSection();
                                NewSection.StartIndex = 0;
                                NewSection.SectionName = MtlLine[1];
                                MeshSectionList.Add(NewSection);
                            }
                            else
                            {   
                                MeshSectionList.Last().EndIndex = (UInt32)VertexIndices.Count;

                                ObjMeshSection NewSection = new ObjMeshSection();
                                NewSection.SectionName = MtlLine[1];
                                NewSection.StartIndex = (UInt32) VertexIndices.Count;                                
                                MeshSectionList.Add(NewSection);
                            }
                        }
                    }
                }

                if(MeshSectionList.Count > 0)
                {
                    MeshSectionList.Last().EndIndex = (UInt32)VertexIndices.Count;
                }
            }
            
            GenerateTangents();
            GenerateVertices();

            VertexList.Clear();
            NormalList.Clear();
            TexCoordList.Clear();
            TangentList.Clear();

            VertexIndexList.Clear();
            NormalIndexList.Clear();
            TexCoordIndexList.Clear();            
        }        
    }
}
