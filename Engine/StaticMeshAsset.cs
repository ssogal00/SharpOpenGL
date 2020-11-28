using Core.Primitive;
using Core.StaticMesh;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Navigation;
using OpenTK.Mathematics;
using ZeroFormatter;


namespace Core.StaticMesh
{
    [ZeroFormattable]
    public class StaticMeshAsset : Core.AssetBase
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

        [Index(6)] public virtual bool HasMaterialFile { get; protected set; } = false;

        [Index(7)]
        public virtual string ObjFilePath { get; set; } = "";

        [Index(8)]
        public virtual string MtlFilePath { get; set; } = "";

        [Index(9)]
        public virtual Vector3 MinVertex { get; set; } = new Vector3();

        [Index(10)]
        public virtual Vector3 MaxVertex { get; set; } = new Vector3();

        [Index(11)]
        public virtual Vector3 CenterVertex { get; set; }

        [Index(12)]
        public virtual List<PC_VertexAttribute> TBNVertices { get; protected set; } = new List<PC_VertexAttribute>();

        [Index(13)]
        public virtual List<uint> TBNIndices { get; protected set; } = new List<uint>();

        [IgnoreFormat]
        public float XExtent => Math.Abs(MaxVertex.X - MinVertex.X);

        [IgnoreFormat]
        public float HalfXExtent => XExtent / 2.0f;

        [IgnoreFormat]
        public float HalfYExtent => YExtent / 2.0f;

        [IgnoreFormat]
        public float YExtent => Math.Abs(MaxVertex.Y - MinVertex.Y);

        [IgnoreFormat]
        public float ZExtent => Math.Abs(MaxVertex.Z - MinVertex.Z);

        protected float MinX = float.MaxValue;
        protected float MaxX = float.MinValue;

        protected float MinY = float.MaxValue;
        protected float MaxY = float.MinValue;

        protected float MinZ = float.MaxValue;
        protected float MaxZ = float.MinValue;

        // only used for mesh loading
        // will be cleared after mesh load
        List<Vector3> TempVertexList = new List<Vector3>();
        List<Vector2> TempTexCoordList = new List<Vector2>();
        List<Vector3> TempNormalList = new List<Vector3>();
        List<Vector4> TempTangentList = new List<Vector4>();

        List<uint> VertexIndexList = new List<uint>();
        List<uint> TexCoordIndexList = new List<uint>();
        List<uint> NormalIndexList = new List<uint>();

        [IgnoreFormat]
        public bool Debugging => m_bDebug;
        protected bool m_bDebug = false;
        

        // import sync
        public override void ImportAssetSync()
        {
            this.Import(ObjFilePath, MtlFilePath);
        }

        // import async
        public override async Task ImportAssetAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                this.Import(ObjFilePath, MtlFilePath);
            });
        }
        
        // save
        public override void SaveImportedAsset(string path)
        {
            var bytesarray = ZeroFormatter.ZeroFormatterSerializer.Serialize<StaticMeshAsset>(this);
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                fs.Write(bytesarray, 0, bytesarray.Count());
            }
        }

        private void Clear()
        {
            //
            TempVertexList.Clear();
            TempTexCoordList.Clear();
            TempNormalList.Clear();
            TempTangentList.Clear();
            //
            VertexIndexList.Clear();
            TexCoordIndexList.Clear();
            NormalIndexList.Clear();
        }

        public StaticMeshAsset(string objFilePath, string mtlFilePath)
        {
            ObjFilePath = objFilePath;
            MtlFilePath = mtlFilePath;
            HasMaterialFile = true;
        }

        public StaticMeshAsset(string objFilePath)
        {
            ObjFilePath = objFilePath;
            MtlFilePath = "";
            HasMaterialFile = false;
        }

        public StaticMeshAsset()
        {
        }

        public void Import(string FilePath, string MtlPath)
        {
            if (File.Exists(MtlPath))
            {
                ParseMtlFile(MtlPath);
            }

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

                            TempNormalList.Add(VN);
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

                            UpdateMinMaxVertex(ref V);

                            TempVertexList.Add(V);
                        }
                    }
                    else if (Trimmedline.StartsWith("vt"))
                    {
                        HasTexCoordinate = true;

                        var tokens = Trimmedline.Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);

                        if (tokens.Count() >= 3)
                        {
                            Vector2 V = new Vector2();
                            V.X = 1 - Convert.ToSingle(tokens[1]);
                            V.Y = 1 - Convert.ToSingle(tokens[2]);

                            TempTexCoordList.Add(V);
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

                            if (HasTexCoordinate)
                            {
                                uint TexIndex1 = Convert.ToUInt32(Token1[1]);
                                uint TexIndex2 = Convert.ToUInt32(Token2[1]);
                                uint TexIndex3 = Convert.ToUInt32(Token3[1]);

                                TexCoordIndexList.Add(TexIndex1 - 1);
                                TexCoordIndexList.Add(TexIndex2 - 1);
                                TexCoordIndexList.Add(TexIndex3 - 1);
                            }

                            if (HasNormal)
                            {
                                uint NormIndex1 = Convert.ToUInt32(Token1[2]);
                                uint NormIndex2 = Convert.ToUInt32(Token2[2]);
                                uint NormIndex3 = Convert.ToUInt32(Token3[2]);

                                NormalIndexList.Add(NormIndex1 - 1);
                                NormalIndexList.Add(NormIndex2 - 1);
                                NormalIndexList.Add(NormIndex3 - 1);
                            }

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

            // update min,max,center
            MinVertex = new Vector3(MinX, MinY, MinZ);
            MaxVertex = new Vector3(MaxX, MaxY, MaxZ);
            CenterVertex = (MinVertex + MaxVertex) / 2;


            if (HasTexCoordinate)
            {
                GenerateTangents();
            }

            GenerateVertices();

            if (m_bDebug)
            {
                GenerateTBNVertices();
            }

            Clear();
        }

        protected void UpdateMinMaxVertex(ref Vector3 newVertex)
        {
            // update X
            if (newVertex.X > MaxX)
            {
                MaxX = newVertex.X;
            }
            if (newVertex.X < MinX)
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

        private Dictionary<PNTT_VertexAttribute, uint> VertexCacheMap = new Dictionary<PNTT_VertexAttribute, uint>();

        bool GetSimilarVertexIndex(ref PNTT_VertexAttribute vertex, out uint index)
        {
            if (VertexCacheMap.ContainsKey(vertex))
            {
                index = VertexCacheMap[vertex];
                return true;
            }
            else
            {
                index = 0;
                return false;
            }
        }

        protected void GenerateTBNVertices()
        {
            foreach (var v in Vertices)
            {
                PC_VertexAttribute v1 = new PC_VertexAttribute();
                v1.VertexPosition = v.VertexPosition;
                v1.VertexColor = Vector3.UnitY;

                PC_VertexAttribute v2 = new PC_VertexAttribute();
                v2.VertexPosition = v.VertexPosition + v.VertexNormal * 3.0f;
                v2.VertexColor = Vector3.UnitY;

                PC_VertexAttribute v3 = new PC_VertexAttribute();
                v3.VertexPosition = v.VertexPosition ;
                v3.VertexColor = Vector3.UnitZ;

                PC_VertexAttribute v4 = new PC_VertexAttribute();
                v4.VertexPosition = v.VertexPosition + v.Tangent.Xyz * 3.0f;
                v4.VertexColor = Vector3.UnitZ;

                TBNVertices.Add(v1);
                TBNIndices.Add((uint)TBNVertices.Count - 1);

                TBNVertices.Add(v2);
                TBNIndices.Add((uint)TBNVertices.Count - 1);

                TBNVertices.Add(v3);
                TBNIndices.Add((uint)TBNVertices.Count - 1);

                TBNVertices.Add(v4);
                TBNIndices.Add((uint)TBNVertices.Count - 1);
            }
        }

        protected void GenerateVertices()
        {
            var Count = VertexIndices.Count;
            VertexIndices.Clear();

            List<ObjMeshSection> SortedMeshSectionList = new List<ObjMeshSection>();

            foreach (var sectionlist in MeshSectionList.GroupBy(x => x.SectionName))
            {
                var sectionName = sectionlist.First().SectionName;
                var sectionCount = sectionlist.Count();

                UInt32 StartIndex = (UInt32) VertexIndices.Count;

                foreach (var section in sectionlist)
                {
                    for (int i = (int) section.StartIndex; i < section.EndIndex; ++i)
                    {
                        var V1 = new PNTT_VertexAttribute();

                        V1.VertexPosition = TempVertexList[(int)VertexIndexList[i]];

                        if (HasTexCoordinate)
                        {
                            V1.TexCoord = TempTexCoordList[(int)TexCoordIndexList[i]];
                            V1.Tangent = TempTangentList[(int)VertexIndexList[i]];
                            //V1.Tangent = TempTangentList[(int) i];
                        }

                        if (HasNormal)
                        {
                            V1.VertexNormal = TempNormalList[(int)NormalIndexList[i]];
                        }

                        uint index = 0;
                        //if (GetSimilarVertexIndex(ref V1, out index))
                        if(false)
                        {
                            VertexIndices.Add(index);
                        }
                        else
                        {
                            Vertices.Add(V1);
                            uint newIndex = (uint)Vertices.Count - 1;
                            //VertexCacheMap.Add(V1, newIndex);
                            VertexIndices.Add(newIndex);
                        }
                    }
                }

                UInt32 EndIndex = (UInt32) VertexIndices.Count;

                SortedMeshSectionList.Add(new ObjMeshSection(sectionName, StartIndex, EndIndex));
            }

            MeshSectionList = SortedMeshSectionList;

            Debug.Assert(Count == VertexIndices.Count);
        }

        protected void GenerateTangents()
        {
            List<Vector3> tan1Accum = new List<Vector3>();
            List<Vector3> tan2Accum = new List<Vector3>();

            for (uint i = 0; i < TempVertexList.Count(); ++i)
            {
                tan1Accum.Add(new Vector3(0, 0, 0));
                tan2Accum.Add(new Vector3(0, 0, 0));
            }

            for (uint i = 0; i < VertexIndexList.Count(); i++)
            {
                TempTangentList.Add(new Vector4(0, 0, 0, 0));
            }

            // Compute the tangent vector
            for (uint i = 0; i < VertexIndexList.Count(); i += 3)
            {
                var p1 = TempVertexList[(int)VertexIndexList[(int)i]];
                var p2 = TempVertexList[(int)VertexIndexList[(int)i + 1]];
                var p3 = TempVertexList[(int)VertexIndexList[(int)i + 2]];

                var tc1 = TempTexCoordList[(int)TexCoordIndexList[(int)i]];
                var tc2 = TempTexCoordList[(int)TexCoordIndexList[(int)i + 1]];
                var tc3 = TempTexCoordList[(int)TexCoordIndexList[(int)i + 2]];

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

            for (uint i = 0; i < VertexIndexList.Count(); ++i)
            {
                var n = TempNormalList[(int)NormalIndexList[(int)i]];
                var t1 = tan1Accum[(int)VertexIndexList[(int)i]];
                var t2 = tan2Accum[(int)VertexIndexList[(int)i]];

                // Gram-Schmidt orthogonalize                
                var temp = Vector3.Normalize(t1 - (Vector3.Dot(n, t1) * n));
                // Store handedness in w                
                var W = (Vector3.Dot(Vector3.Cross(n, t1), t2) < 0.0f) ? -1.0f : 1.0f;

                bool bValid = true;
                if (Single.IsNaN(temp.X) || Single.IsNaN(temp.Y) || Single.IsNaN(temp.Z))
                {
                    bValid = false;
                }

                if (Single.IsInfinity(temp.X) || Single.IsInfinity(temp.Y) || Single.IsInfinity(temp.Z))
                {
                    bValid = false;
                }

                if (bValid == true)
                {
                    lastValidTangent = new Vector4(temp.X, temp.Y, temp.Z, W);
                }

                if (bValid == false)
                {
                    temp = lastValidTangent.Xyz;
                }

                TempTangentList[(int)i] = new Vector4(temp.X, temp.Y, temp.Z, W);
            }

            tan1Accum.Clear();
            tan2Accum.Clear();
        }

       

        protected void ParseMtlFile(string MtlPath)
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
