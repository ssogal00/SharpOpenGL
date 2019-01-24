using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Primitive;

namespace SharpOpenGL
{
    public class Cube
    {
        public Cube()
        {
            patchDrawable = new PatchDrawable<P_VertexAttribute>();

            List<P_VertexAttribute> vertexList = new List<P_VertexAttribute>();
            List<uint> indexList = new List<uint>();
            
            vertexList.Add(new P_VertexAttribute(new OpenTK.Vector3(-0.25f, -0.25f, -0.25f)));
            vertexList.Add(new P_VertexAttribute(new OpenTK.Vector3(-0.25f, 0.25f, -0.25f)));
            vertexList.Add(new P_VertexAttribute(new OpenTK.Vector3(0.25f, -0.25f, -0.25f)));
            vertexList.Add(new P_VertexAttribute(new OpenTK.Vector3(0.25f, 0.25f, -0.25f)));

            vertexList.Add(new P_VertexAttribute(new OpenTK.Vector3(0.25f, -0.25f, 0.25f)));
            vertexList.Add(new P_VertexAttribute(new OpenTK.Vector3(0.25f, 0.25f, 0.25f)));
            vertexList.Add(new P_VertexAttribute(new OpenTK.Vector3(-0.25f, -0.25f, 0.25f)));
            vertexList.Add(new P_VertexAttribute(new OpenTK.Vector3(-0.25f, 0.25f, 0.25f)));

            uint[] indexArray = 
            {
                0, 1, 2, 3,
                2, 3, 4, 5,
                4, 5, 6, 7,
                6, 7, 0, 1,
                0, 2, 6, 4,
                1, 7, 3, 5
            };

            var vertexArray = vertexList.ToArray();

            patchDrawable.SetupData(ref vertexArray, ref indexArray);
        }

        public void Draw()
        {
            patchDrawable.Draw();
        }

        PatchDrawable<P_VertexAttribute> patchDrawable = null;
    }
}
