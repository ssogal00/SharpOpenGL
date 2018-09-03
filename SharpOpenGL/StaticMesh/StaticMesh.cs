using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Core.Asset;
using Core.Primitive;
using ZeroFormatter;

namespace SharpOpenGL.StaticMesh
{
    public class StaticMesh : Asset
    {   
        // serialized fields
        // vertices
        [Index(0)]
        public virtual List<PNTT_VertexAttribute> Vertices { get; protected set; } = new List<PNTT_VertexAttribute>();

        // serialized mesh sections
        [Index(1)]
        public virtual List<ObjMeshSection> MeshSectionList { get; protected set; } = new List<ObjMeshSection>();

        // serialized material map
        [Index(2)]
        public virtual Dictionary<string, ObjMeshMaterial> MaterialMap { get; protected set; } = new Dictionary<string, ObjMeshMaterial>();

        // serialized vertex indices
        [Index(3)]
        public virtual List<uint> VertexIndices { get; protected set; } = new List<uint>();

        [Index(4)] public virtual bool HasNormal { get; protected set; } = false;

        [Index(5)] public virtual bool HasTexCoordinate { get; protected set; } = false;

        protected string ObjFilePath = "";
        protected string MtlFilePath = "";

        

        // only used for mesh loading
        // will be cleared after mesh load
        List<Vector3> VertexList = new List<Vector3>();
        List<Vector2> TexCoordList = new List<Vector2>();
        List<Vector3> NormalList = new List<Vector3>();
        List<Vector4> TangentList = new List<Vector4>();

        List<uint> VertexIndexList = new List<uint>();
        List<uint> TexCoordIndexList = new List<uint>();
        List<uint> NormalIndexList = new List<uint>();

        public override void ImportAssetSync()
        {

        }

        public override Task ImportAssetAsync()
        {
            return null;
        }

        
        public StaticMesh(string objFilePath, string mtlFilePath)
        {
            ObjFilePath = objFilePath;
            MtlFilePath = mtlFilePath;
        }

        public void Load(string FilePath, string MtlPath)
        {
            ParseMtlFile(MtlPath);

            if (File.Exists(FilePath))
            {
                var Lines = File.ReadAllLines(FilePath);

                foreach (var line in Lines)
                {
                    var Trimmedline = line.TrimStart(new char[] {' ', '\t'});

                    if (Trimmedline.StartsWith("vn"))
                    {
                        var tokens = Trimmedline.Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);

                        HasNormal = true;

                        if (tokens.Count() >= 4)
                        {
                            Vector3 VN = new Vector3();
                            VN.X = Convert.ToSingle(tokens[1]);
                            VN.Y = Convert.ToSingle(tokens[2]);
                            VN.Z = Convert.ToSingle(tokens[3]);

                            NormalList.Add(VN);
                        }
                    }
                    else if (Trimmedline.StartsWith("v "))
                    {
                        var tokens = Trimmedline.Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);

                        if (tokens.Count() == 4)
                        {
                            Vector3 V = new Vector3();
                            V.X = Convert.ToSingle(tokens[1]);
                            V.Y = Convert.ToSingle(tokens[2]);
                            V.Z = Convert.ToSingle(tokens[3]);

                            //UpdateMinMaxVertex(ref V);

                            VertexList.Add(V);
                        }
                    }
                    else if (Trimmedline.StartsWith("vt"))
                    {
                        HasTexCoordinate = true;

                        var tokens = Trimmedline.Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);

                        if (tokens.Count() >= 3)
                        {
                            Vector2 V = new Vector2();
                            V.X = Convert.ToSingle(tokens[1]);
                            V.Y = Convert.ToSingle(tokens[2]);

                            TexCoordList.Add(V);
                        }
                    }
                    else if (Trimmedline.StartsWith("f "))
                    {
                        var tokens = Trimmedline.Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);

                        if (tokens.Count() == 4)
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

                            VertexIndices.Add((uint) VertexIndices.Count);
                            VertexIndices.Add((uint) VertexIndices.Count);
                            VertexIndices.Add((uint) VertexIndices.Count);
                        }
                    }
                    else if (Trimmedline.StartsWith("usemtl"))
                    {
                        var MtlLine = Trimmedline.Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);

                        if (MtlLine.Count() == 2)
                        {
                            if (MeshSectionList.Count() == 0)
                            {
                                ObjMeshSection NewSection = new ObjMeshSection();
                                NewSection.StartIndex = 0;
                                NewSection.SectionName = MtlLine[1];
                                MeshSectionList.Add(NewSection);
                            }
                            else
                            {
                                MeshSectionList.Last().EndIndex = (UInt32) VertexIndices.Count;

                                ObjMeshSection NewSection = new ObjMeshSection();
                                NewSection.SectionName = MtlLine[1];
                                NewSection.StartIndex = (UInt32) VertexIndices.Count;
                                MeshSectionList.Add(NewSection);
                            }
                        }
                    }
                }

                if (MeshSectionList.Count > 0)
                {
                    MeshSectionList.Last().EndIndex = (UInt32) VertexIndices.Count;
                }
            }
        }

        protected void ParseMtlFile(string MtlPath)
        {
            if (File.Exists(MtlPath))
            {
                var Lines = File.ReadAllLines(MtlPath);
                ObjMeshMaterial NewMaterial = null;
                foreach (var line in Lines)
                {
                    var TrimmedLine = line.TrimStart(new char[] { ' ', '\t' });

                    if (TrimmedLine.StartsWith("newmtl"))
                    {
                        var Tokens = TrimmedLine.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        if (Tokens.Count() == 2)
                        {
                            NewMaterial = new ObjMeshMaterial();
                            NewMaterial.MaterialName = Tokens[1];
                        }
                    }
                    else if (TrimmedLine.StartsWith("map_Kd"))
                    {
                        var Tokens = TrimmedLine.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        if (Tokens.Count() == 2)
                        {
                            if (NewMaterial != null)
                            {
                                NewMaterial.DiffuseMap = Tokens[1];
                            }
                        }
                    }
                    else if (TrimmedLine.StartsWith("map_bump"))
                    {
                        var tokens = TrimmedLine.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        if (tokens.Count() == 2)
                        {
                            if (NewMaterial != null)
                            {
                                NewMaterial.NormalMap = tokens[1];
                            }
                        }
                    }
                    else if (TrimmedLine.StartsWith("map_Ka"))
                    {
                        var tokens = TrimmedLine.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        if (tokens.Count() == 2)
                        {
                            if (NewMaterial != null)
                            {
                                NewMaterial.SpecularMap = tokens[1];
                            }
                        }
                    }
                    else if (TrimmedLine.StartsWith("map_Ns"))
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
                    else if (TrimmedLine.StartsWith("map_d"))
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
                    else if (TrimmedLine.Length == 0 && NewMaterial != null)
                    {
                        MaterialMap.Add(NewMaterial.MaterialName, NewMaterial);
                        NewMaterial = null;
                    }
                }

                if (NewMaterial != null)
                {
                    MaterialMap.Add(NewMaterial.MaterialName, NewMaterial);
                    NewMaterial = null;
                }
            }
        }

    }
}
