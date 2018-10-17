using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Core.Primitive
{
    public class Torus : RenderResource
    {
        public Torus(float radius1, float radius2, int sampleCount)
        {
            Debug.Assert(radius1 > 0 && radius1 > radius2 && sampleCount > 8);
            Radius1 = radius1;
            Radius2 = radius2;
            SampleCount = sampleCount;
        }

        public override void Initialize()
        {
            base.Initialize();
            GenerateVertices();
            drawable = new DrawableBase<PNC_VertexAttribute>();
            var vertexArray = VertexList.ToArray();
            drawable.SetupVertexData(ref vertexArray);

            VertexList.Clear();
        }

        public void Draw(MaterialBase.MaterialBase material)
        {
            drawable.DrawPrimitiveWithoutIndex(PrimitiveType.Triangles);
        }

        protected void GenerateVertices()
        {
            for(int i = 0; i < SampleCount; ++i)
            {
                var degree1 = OpenTK.MathHelper.DegreesToRadians(((double)360.0 / SampleCount) * i);
                var degree2 = OpenTK.MathHelper.DegreesToRadians(((double)360.0 / SampleCount) * (i + 1));

                var x1 = Radius1 * (float)Math.Cos(degree1);
                var z1 = Radius1 * (float)Math.Sin(degree1);

                var x2 = Radius1 * (float)Math.Cos(degree2);
                var z2 = Radius1 * (float)Math.Sin(degree2);

                var vCenter1 = new Vector3(x1, 0, z1);
                var vDir1 = vCenter1.Normalized();

                var vCenter2 = new Vector3(x2, 0, z2);
                var vDir2 = vCenter2.Normalized();

                for (int j =0; j < SampleCount; ++j)
                {
                    var innerDegree1 = OpenTK.MathHelper.DegreesToRadians(((double)360.0 / SampleCount) * j);
                    var innerDegree2 = OpenTK.MathHelper.DegreesToRadians(((double)360.0 / SampleCount) * (j+1));

                    var offsetX1 = Radius2 * (float)Math.Cos(innerDegree1);
                    var offsetY1 = Radius2 * (float)Math.Sin(innerDegree1);

                    var offsetX2 = Radius2 * (float)Math.Cos(innerDegree2);
                    var offsetY2 = Radius2 * (float)Math.Sin(innerDegree2);

                    var v1 = vCenter1 + vDir1 * offsetX1 + new Vector3(0, offsetY1, 0);
                    var v2 = vCenter1 + vDir1 * offsetX2 + new Vector3(0, offsetY2, 0);

                    var v3 = vCenter2 + vDir2 * offsetX1 + new Vector3(0, offsetY1, 0);
                    var v4 = vCenter2 + vDir2 * offsetX2 + new Vector3(0, offsetY2, 0);

                    var d1 = (v3 - v4).Normalized();
                    var d2 = (v2 - v4).Normalized();
                    var norm1 = Vector3.Cross(d2, d1).Normalized();

                    var d3 = (v3 - v1).Normalized();
                    var d4 = (v2 - v1).Normalized();
                    var norm2 = Vector3.Cross(d3, d4).Normalized();
                    
                    // v4 - v3
                    // |  /  |
                    // v2 - v1

                    VertexList.Add(new PNC_VertexAttribute(v4, norm1, Color));
                    VertexList.Add(new PNC_VertexAttribute(v3, norm1, Color));
                    VertexList.Add(new PNC_VertexAttribute(v2, norm1, Color));
                    
                    VertexList.Add(new PNC_VertexAttribute(v2, norm2, Color));
                    VertexList.Add(new PNC_VertexAttribute(v3, norm2, Color));
                    VertexList.Add(new PNC_VertexAttribute(v1, norm2, Color));
                }
            }
        }

        protected float Radius1 = 10;

        protected float Radius2 = 3;

        protected int SampleCount = 10;

        protected Vector3 Color = new Vector3(1, 0, 0);

        protected int VertexCount = 0;

        protected List<PNC_VertexAttribute> VertexList = new List<PNC_VertexAttribute>();
        protected DrawableBase<PNC_VertexAttribute> drawable = null;
    }
}
