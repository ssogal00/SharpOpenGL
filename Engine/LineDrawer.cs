using System.Collections.Generic;

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
            MyLineDrawable = new LineDrawable<P_VertexAttribute>();
            foreach (var line in LineList)
            {
                var VertexStart = new P_VertexAttribute();
                var VertexEnd = new P_VertexAttribute();

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

        public List<P_VertexAttribute> VertexList = new List<P_VertexAttribute>();

        public LineDrawable<P_VertexAttribute> MyLineDrawable = null;
    }
}
