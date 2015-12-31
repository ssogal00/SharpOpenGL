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

        List<ushort> VertexIndices = new List<ushort>();
        List<ushort> TextureIndices = new List<ushort>();

        public ObjMesh()
        {            
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
            IB.BufferData<ushort>(ref IndexArr);
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
                    if(line.StartsWith("vn"))
                    {
                    }
                    else if(line.StartsWith("v "))
                    {
                        var tokens = line.Split(new char[] {' ','\t'}, StringSplitOptions.RemoveEmptyEntries);

                        if(tokens.Count() == 4)
                        {
                            Vector3 V = new Vector3();
                            V.X = Convert.ToSingle(tokens[1]);
                            V.Y = Convert.ToSingle(tokens[2]);
                            V.Z = Convert.ToSingle(tokens[3]);

                            TempVertices.Add(V);
                        }
                    }
                    else if(line.StartsWith("vt"))
                    {
                        var tokens = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        if(tokens.Count() >= 3)
                        {
                            Vector2 V = new Vector2();
                            V.X = Convert.ToSingle(tokens[1]);
                            V.Y = Convert.ToSingle(tokens[2]);

                            TempTexCoord.Add(V);
                        }
                    }
                    else if(line.StartsWith("f "))
                    {
                        var tokens = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        if(tokens.Count() == 4)
                        {
                            TestShaderVertexAttributes V1 = new TestShaderVertexAttributes();
                            TestShaderVertexAttributes V2 = new TestShaderVertexAttributes();
                            TestShaderVertexAttributes V3 = new TestShaderVertexAttributes();
                            

                            var Token1 = tokens[1].Split('/');
                            var Token2 = tokens[2].Split('/');
                            var Token3 = tokens[3].Split('/');

                            ushort Index1 = Convert.ToUInt16(Token1[0]);
                            ushort Index2 = Convert.ToUInt16(Token2[0]);
                            ushort Index3 = Convert.ToUInt16(Token3[0]);

                            V1.VertexPosition = TempVertices[Index1-1];
                            V2.VertexPosition = TempVertices[Index2-1];
                            V3.VertexPosition = TempVertices[Index3-1];

                            ushort TexIndex1 = Convert.ToUInt16(Token1[1]);
                            ushort TexIndex2 = Convert.ToUInt16(Token2[1]);
                            ushort TexIndex3 = Convert.ToUInt16(Token3[1]);


                            V1.TexCoord = TempTexCoord[TexIndex1-1];
                            V2.TexCoord = TempTexCoord[TexIndex2-1];
                            V3.TexCoord = TempTexCoord[TexIndex3-1];

                            Vertices.Add(V1); Vertices.Add(V2); Vertices.Add(V3);
                            VertexIndices.Add((ushort)VertexIndices.Count);
                            VertexIndices.Add((ushort)VertexIndices.Count);
                            VertexIndices.Add((ushort)VertexIndices.Count);
                        }
                    }

                }



                PrepareToDraw();
            }
        }        
    }
}
