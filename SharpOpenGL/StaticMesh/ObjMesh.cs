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

using ObjMeshVertexAttribute = Core.Primitive.PNTT_VertexAttribute;


namespace SharpOpenGL.StaticMesh
{
    public class ObjMesh
    {
        // 
        // Vertex Buffer and Index buffer to render
        StaticVertexBuffer<ObjMeshVertexAttribute> VB = null;
        IndexBuffer IB = null;
        
        //
        List<ObjMeshVertexAttribute> Vertices = new List<ObjMeshVertexAttribute>();        

        List<uint> VertexIndices = new List<uint>();

        // @only used for mesh loading
        List<Vector3> VertexList = new List<Vector3>();        
        List<Vector2> TexCoordList = new List<Vector2>();        
        List<Vector3> NormalList = new List<Vector3>();
        List<Vector4> TangentList = new List<Vector4>();

        List<uint> VertexIndexList = new List<uint>();
        List<uint> TexCoordIndexList = new List<uint>();
        List<uint> NormalIndexList = new List<uint>();
        
        // clear after mesh loading

        List<ObjMeshSection> MeshSectionList = new List<ObjMeshSection>();        
        Dictionary<string, ObjMeshMaterial> MaterialMap = new Dictionary<string, ObjMeshMaterial>();

        Dictionary<string, Texture2D> TextureMap = new Dictionary<string, Texture2D>();

        // 
        protected bool bHasNormal = false;
        protected bool bHasTexCoordinate = false;

        public bool HasNormal => bHasNormal;
        public bool HasTexCoord => bHasTexCoordinate;

        Vector3 MinVertex = new Vector3(0,0,0);
        Vector3 MaxVertex = new Vector3(0,0,0);
        Vector3 MeshCenter = new Vector3(0, 0, 0);
        
        public Task LoadAsync(string FilePath, string MtlPath)
        {
            Debug.Assert(File.Exists(FilePath) && File.Exists(MtlPath));

            return Task.Run(() => { Load(FilePath, MtlPath); });
        }

        public static async Task<ObjMesh> LoadMeshAsync(string FilePath, string MtlPath)
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

            for (uint i = 0; i < VertexIndexList.Count(); i++)
            {
                tan1Accum.Add(new Vector3(0,0,0));
                tan2Accum.Add(new Vector3(0,0,0));
                TangentList.Add(new Vector4(0,0,0,0));
            }

            // Compute the tangent vector
            for (uint i = 0; i < VertexIndexList.Count(); i += 3)
            {
                var p1 = VertexList[(int)VertexIndexList[(int)i]];
                var p2 = VertexList[(int)VertexIndexList[(int)i + 1]];
                var p3 = VertexList[(int)VertexIndexList[(int)i + 2]];

                var tc1 = TexCoordList[(int)TexCoordIndexList[(int)i]];
                var tc2 = TexCoordList[(int)TexCoordIndexList[(int)i+1]];
                var tc3 = TexCoordList[(int)TexCoordIndexList[(int)i+2]];

                Vector3 q1 = p2 - p1;
                Vector3 q2 = p3 - p1;
                float s1 = tc2.X - tc1.X, s2 = tc3.X - tc1.X;
                float t1 = tc2.Y - tc1.Y, t2 = tc3.Y - tc1.Y;
                float r = 1.0f / (s1 * t2 - s2 * t1);

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

            for(uint i = 0; i< VertexIndexList.Count(); ++i )
            {
                var n = NormalList[(int)NormalIndexList[(int)i]];
                var t1 = tan1Accum[(int)VertexIndexList[(int)i]];
                var t2 = tan2Accum[(int)VertexIndexList[(int)i]];

                // Gram-Schmidt orthogonalize                
                var temp = OpenTK.Vector3.Normalize(t1 - (OpenTK.Vector3.Dot(n, t1) * n));
                // Store handedness in w
                // tangents[i] = vec4(glm::normalize(t1 - (glm::dot(n, t1) * n) ), 0.0f);
                var W = (OpenTK.Vector3.Dot(OpenTK.Vector3.Cross(n, t1), t2) < 0.0f) ? -1.0f : 1.0f;

                TangentList[(int)VertexIndexList[(int)i]] = new Vector4(temp.X, temp.Y, temp.Z, W);
            }
            tan1Accum.Clear();
            tan2Accum.Clear();
        }

        public void Draw(Core.MaterialBase.MaterialBase material)
        {
            VB.Bind();
            IB.Bind();
            VB.BindVertexAttribute();

            foreach (var section in MeshSectionList)
            {
                if (MaterialMap.ContainsKey(section.SectionName))
                {
                    material.SetTexture("DiffuseTex", TextureMap[MaterialMap[section.SectionName].DiffuseMap]);
                    material.SetTexture("NormalTex", TextureMap[MaterialMap[section.SectionName].NormalMap]);
                }

                var ByteOffset = new IntPtr(section.StartIndex * sizeof(uint));
                GL.DrawElements(PrimitiveType.Triangles, (int)(section.EndIndex - section.StartIndex), DrawElementsType.UnsignedInt, ByteOffset);
            }
        }
        
        public void PrepareToDraw()
        {
            VB = new StaticVertexBuffer<ObjMeshVertexAttribute>();
            IB = new IndexBuffer();

            VB.Bind();
            var Arr = Vertices.ToArray();
            VB.BufferData<ObjMeshVertexAttribute>(ref Arr);
            VB.BindVertexAttribute();

            IB.Bind();
            var IndexArr = VertexIndices.ToArray();
            IB.BufferData<uint>(ref IndexArr);
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

                if (Mtl.Value.NormalMap.Length > 0)
                {
                    if(!TextureMap.ContainsKey(Mtl.Value.NormalMap))
                    {
                        var textureObj = new Texture2D();
                        textureObj.Load(Mtl.Value.NormalMap);
                        TextureMap.Add(Mtl.Value.NormalMap, textureObj);
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
                            uint NormIndex2 = Convert.ToUInt32(Token1[2]);
                            uint NormIndex3 = Convert.ToUInt32(Token1[2]);

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
                    MeshSectionList.Last().EndIndex = (UInt32) Vertices.Count;
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
