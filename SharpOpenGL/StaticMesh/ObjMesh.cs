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
        StaticVertexBuffer<ObjMeshVertexAttribute> VB = null;
        IndexBuffer IB = null;

        List<ObjMeshVertexAttribute> Vertices = new List<ObjMeshVertexAttribute>();
        List<ushort> Indices = new List<ushort>();

        public ObjMesh()
        {            
        }

        public void PrepareToDraw()
        {
            VB = new StaticVertexBuffer<ObjMeshVertexAttribute>();
            IB = new IndexBuffer();

            VB.Bind();
            var Arr = Vertices.ToArray();
            VB.BufferData<ObjMeshVertexAttribute>(ref Arr);
            VB.VertexAttribPointer(Arr);

            IB.Bind();
            var IndexArr = Indices.ToArray();
            IB.BufferData<ushort>(ref IndexArr);
        }

        public int GetIndicesCount()
        {
            return Indices.Count();
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
                            ObjMeshVertexAttribute V = new ObjMeshVertexAttribute();
                            V.VertexPosition.X = Convert.ToSingle(tokens[1]);
                            V.VertexPosition.Y = Convert.ToSingle(tokens[2]);
                            V.VertexPosition.Z = Convert.ToSingle(tokens[3]);

                            Vertices.Add(V);
                        }
                    }
                    else if(line.StartsWith("f "))
                    {
                        var tokens = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        if(tokens.Count() == 4)
                        {
                            ushort Index1 = Convert.ToUInt16(tokens[1].Split('/')[0]);
                            ushort Index2 = Convert.ToUInt16(tokens[2].Split('/')[0]);
                            ushort Index3 = Convert.ToUInt16(tokens[3].Split('/')[0]);

                            Indices.Add((ushort)(Index1 - 1));
                            Indices.Add((ushort)(Index2 - 1));
                            Indices.Add((ushort)(Index3 - 1));
                        }
                    }
                }

                PrepareToDraw();
            }
        }        
    }
}
