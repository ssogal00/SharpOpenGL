using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Primitive;

namespace Core
{
    public class Cube
    {
        public Cube()
        {
            triangleDrawble = new TriangleDrawable<PT_VertexAttribute>();

            List<PT_VertexAttribute> vertexList = new List<PT_VertexAttribute>();
            List<uint> indexList = new List<uint>();
            // upper left            
            vertexList.Add(new PT_VertexAttribute(new OpenTK.Vector3(-1, 1, 1), new OpenTK.Vector2(0, 1)));
            // upper right
            vertexList.Add(new PT_VertexAttribute(new OpenTK.Vector3(1, 1, 1), new OpenTK.Vector2( 1, 1)));
            // lower right
            vertexList.Add(new PT_VertexAttribute(new OpenTK.Vector3(1, -1, 1), new OpenTK.Vector2(0, 0)));

            // upper left
            vertexList.Add(new PT_VertexAttribute(new OpenTK.Vector3(-1, 1, 1), new OpenTK.Vector2(0, 0)));
            // lower Right
            vertexList.Add(new PT_VertexAttribute(new OpenTK.Vector3(1, -1, 1), new OpenTK.Vector2(0, 0)));
            // lower left
            vertexList.Add(new PT_VertexAttribute(new OpenTK.Vector3(-1, -1, 1), new OpenTK.Vector2(0, 0)));            

            var vertexArray = vertexList.ToArray();
            var indexArray = Enumerable.Range(0, vertexList.Count).Select(x => (uint)x).ToArray();

            triangleDrawble.SetupData(ref vertexArray, ref indexArray);
        }

        public void Draw()
        {
            if (triangleDrawble != null)
            {
                triangleDrawble.Draw();
            }
        }

        TriangleDrawable<PT_VertexAttribute> triangleDrawble = null;
    }
}
