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

using ObjMeshVertexAttribute = Core.Primitive.PT_VertexAttribute;


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
        List<Core.Primitive.P_VertexAttribute> P_Vertices = new List<Core.Primitive.P_VertexAttribute>();
        List<Core.Primitive.PN_VertexAttribute> PN_Vertices = new List<Core.Primitive.PN_VertexAttribute>();
        List<Core.Primitive.PNT_VertexAttribute> PNT_Vertices = new List<Core.Primitive.PNT_VertexAttribute>();

        List<uint> VertexIndices = new List<uint>();

        // @only used for mesh loading
        List<Vector3> VertexList = new List<Vector3>();        
        List<Vector2> TexCoordList = new List<Vector2>();        
        List<Vector3> NormalList = new List<Vector3>();
        // clear after mesh loading
 
        List<ObjMeshSection> MeshSectionList = new List<ObjMeshSection>();        
        Dictionary<string, ObjMeshMaterial> MaterialMap = new Dictionary<string, ObjMeshMaterial>();

        Dictionary<string, Texture2D> TextureMap = new Dictionary<string, Texture2D>();

        Vector3 MinVertex = new Vector3(0,0,0);
        Vector3 MaxVertex = new Vector3(0,0,0);
        Vector3 MeshCenter = new Vector3(0, 0, 0);
        
        public Task LoadAsync(string FilePath, string MtlPath)
        {
            return Task.Run(() => { Load(FilePath, MtlPath); });
        }

        public static async Task<ObjMesh> LoadMeshAsync(string FilePath, string MtlPath)
        {
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

        public void Draw(SharpOpenGL.BasicMaterial.BasicMaterial Material)
        {
            VB.Bind();
            IB.Bind();

            SharpOpenGL.BasicMaterial.VertexAttribute.VertexAttributeBinding();

            foreach(var Section in MeshSectionList)            
            {
                if(MaterialMap.ContainsKey(Section.SectionName) )
                {                    
                    Material.SetTestTexture2D(TextureMap[MaterialMap[Section.SectionName].DiffuseMap]);                    
                }

                var ByteOffset = new IntPtr(Section.StartIndex * sizeof(uint) );
                GL.DrawElements(PrimitiveType.Triangles, (int)(Section.EndIndex - Section.StartIndex), DrawElementsType.UnsignedInt, ByteOffset);
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

                        if(tokens.Count() >= 4)
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
                            var V1 = new ObjMeshVertexAttribute();
                            var V2 = new ObjMeshVertexAttribute();
                            var V3 = new ObjMeshVertexAttribute();
                            

                            var Token1 = tokens[1].Split('/');
                            var Token2 = tokens[2].Split('/');
                            var Token3 = tokens[3].Split('/');

                            uint Index1 = Convert.ToUInt32(Token1[0]);
                            uint Index2 = Convert.ToUInt32(Token2[0]);
                            uint Index3 = Convert.ToUInt32(Token3[0]);

                            V1.VertexPosition = VertexList[(int)Index1-1];
                            V2.VertexPosition = VertexList[(int)Index2-1];
                            V3.VertexPosition = VertexList[(int)Index3-1];

                            uint TexIndex1 = Convert.ToUInt32(Token1[1]);
                            uint TexIndex2 = Convert.ToUInt32(Token2[1]);
                            uint TexIndex3 = Convert.ToUInt32(Token3[1]);


                            V1.TexCoord = TexCoordList[(int)TexIndex1-1];
                            V2.TexCoord = TexCoordList[(int)TexIndex2-1];
                            V3.TexCoord = TexCoordList[(int)TexIndex3-1];

                            Vertices.Add(V1); Vertices.Add(V2); Vertices.Add(V3);
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
                                MeshSectionList.Last().EndIndex = (UInt32) Vertices.Count;

                                ObjMeshSection NewSection = new ObjMeshSection();
                                NewSection.SectionName = MtlLine[1];
                                NewSection.StartIndex = (UInt32) Vertices.Count;                                
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

            VertexList.Clear();
            NormalList.Clear();
            TexCoordList.Clear();
        }        
    }
}
