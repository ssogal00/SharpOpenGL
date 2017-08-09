using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Core.Buffer;

namespace Core.Primitive
{
    public class LineDrawer
    {
        public void Draw()
        {
            VB.Bind();
            IB.Bind();
            
            GL.DrawElements(PrimitiveType.Lines, IndexList.Count, DrawElementsType.UnsignedInt, 0);
        }

        public void Setup()
        {
            VB = new StaticVertexBuffer<PrimitiveVertexAttribute>();
            IB = new IndexBuffer();

            foreach (var line in LineList)
            {
                var VertexStart = new PrimitiveVertexAttribute();
                var VertexEnd = new PrimitiveVertexAttribute();

                VertexStart.VertexPosition = line.StartPoint;
                VertexEnd.VertexPosition = line.EndPoint;

                VertexList.Add(VertexStart);

                IndexList.Add((uint)IndexList.Count);

                VertexList.Add(VertexEnd);

                IndexList.Add((uint)IndexList.Count);
            }

            var VertexArray = VertexList.ToArray();
            VB.BufferData<PrimitiveVertexAttribute>(ref VertexArray);
            VB.VertexAttribPointer();

            var IndexArray = IndexList.ToArray();
            IB.BufferData<uint>(ref IndexArray);
        }

        public void AddLine(Line NewLine)
        {
            LineList.Add(NewLine);
        }

        StaticVertexBuffer<PrimitiveVertexAttribute> VB = null;
        IndexBuffer IB = null;
       
        public List<Line> LineList = new List<Line>();
        public List<uint> IndexList = new List<uint>();

        public List<PrimitiveVertexAttribute> VertexList = new List<PrimitiveVertexAttribute>();
    }
}
