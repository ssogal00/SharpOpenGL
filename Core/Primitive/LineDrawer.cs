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
            MyLineDrawable.Draw();
        }

        public void Setup()
        {
            MyLineDrawable = new LineDrawable<PrimitiveVertexAttribute>();
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
            var IndexArray = IndexList.ToArray();

            MyLineDrawable.SetupData(ref VertexArray, ref IndexArray);
        }

        public void AddLine(Line NewLine)
        {
            LineList.Add(NewLine);
        }
               
        public List<Line> LineList = new List<Line>();
        public List<uint> IndexList = new List<uint>();

        public List<PrimitiveVertexAttribute> VertexList = new List<PrimitiveVertexAttribute>();

        public LineDrawable<PrimitiveVertexAttribute> MyLineDrawable = null;
    }
}
