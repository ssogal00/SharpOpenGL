using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using SharpOpenGL;
using SharpOpenGL.Buffer;

namespace SharpOpenGL.StaticMesh
{
    public class ObjMesh
    {
        StaticVertexBuffer<TestShaderVertexAttributes> VB = null;
        IndexBuffer IB = null;

        List<TestShaderVertexAttributes> Vertices = new List<TestShaderVertexAttributes>();
        
        List<Vector3> TempVertices = new List<Vector3>();
        List<Vector2> TempTexCoord = new List<Vector2>();

        List<uint> VertexIndices = new List<uint>();
        List<uint> TextureIndices = new List<uint>();

        List<ObjMeshSection> MeshSectionList = new List<ObjMeshSection>();

        public ObjMesh()
        {
        }

        public void Draw()
        {
            VB.Bind();
            IB.Bind();

            TestShaderVertexAttributes.VertexAttributeBinding();

            foreach(var Section in MeshSectionList)
            {
                var ByteOffset = new IntPtr(Section.StartIndex * sizeof(uint) );
                GL.DrawElements(PrimitiveType.Triangles, (int)(Section.EndIndex - Section.StartIndex), DrawElementsType.UnsignedInt, ByteOffset);
            }
        }

        public void PrepareToDraw()
        {
            VB = new StaticVertexBuffer<TestShaderVertexAttributes>();
            IB = new IndexBuffer();

            VB.Bind();
            var Arr = Vertices.ToArray();
            VB.BufferData<TestShaderVertexAttributes>(ref Arr);
            VB.VertexAttribPointer(Arr);

            IB.Bind();
            var IndexArr = VertexIndices.ToArray();
            IB.BufferData<uint>(ref IndexArr);
        }

        public int GetIndicesCount()
        {
            return VertexIndices.Count();
        }
                
        public void Load(string FilePath)
        {
            if(File.Exists(FilePath))
            {
                var Lines = File.ReadAllLines(FilePath);
                
                foreach(var line in Lines)
                {
                    var Trimmedline = line.TrimStart(new char[]{ ' ', '\t'});

                    if(Trimmedline.StartsWith("vn"))
                    {
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

                            TempVertices.Add(V);
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

                            TempTexCoord.Add(V);
                        }
                    }
                    else if(Trimmedline.StartsWith("f "))
                    {
                        var tokens = Trimmedline.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        if(tokens.Count() == 4)
                        {
                            TestShaderVertexAttributes V1 = new TestShaderVertexAttributes();
                            TestShaderVertexAttributes V2 = new TestShaderVertexAttributes();
                            TestShaderVertexAttributes V3 = new TestShaderVertexAttributes();
                            

                            var Token1 = tokens[1].Split('/');
                            var Token2 = tokens[2].Split('/');
                            var Token3 = tokens[3].Split('/');

                            uint Index1 = Convert.ToUInt32(Token1[0]);
                            uint Index2 = Convert.ToUInt32(Token2[0]);
                            uint Index3 = Convert.ToUInt32(Token3[0]);

                            V1.VertexPosition = TempVertices[(int)Index1-1];
                            V2.VertexPosition = TempVertices[(int)Index2-1];
                            V3.VertexPosition = TempVertices[(int)Index3-1];

                            uint TexIndex1 = Convert.ToUInt16(Token1[1]);
                            uint TexIndex2 = Convert.ToUInt16(Token2[1]);
                            uint TexIndex3 = Convert.ToUInt16(Token3[1]);


                            V1.TexCoord = TempTexCoord[(int)TexIndex1-1];
                            V2.TexCoord = TempTexCoord[(int)TexIndex2-1];
                            V3.TexCoord = TempTexCoord[(int)TexIndex3-1];

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

                PrepareToDraw();
            }
        }        
    }
}
