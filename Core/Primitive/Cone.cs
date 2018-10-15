using System;
using System.Collections.Generic;
using OpenTK;
using System.Diagnostics;
using Core.Buffer;
using OpenTK.Graphics.OpenGL;

namespace Core.Primitive
{
    public class Cone : RenderResource
    {
        public Cone(float radius, float height, uint count)
        {
            Debug.Assert(radius > 0 && height > 0 && count > 0);
            Radius = radius;
            Height = height;
            Count = count;
        }

        public override void Initialize()
        {
            GenerateVertices();

            VB = new StaticVertexBuffer<PNC_VertexAttribute>();
            VB.Bind();

            var vertexArray = VertexList.ToArray();
            VB.BufferData<PNC_VertexAttribute>(ref vertexArray);
        }

        public void Draw(MaterialBase.MaterialBase material)
        {
            using (var dummy = new ScopedBind(VB))
            {
                PNC_VertexAttribute.VertexAttributeBinding();
                GL.DrawArrays(PrimitiveType.Triangles, 0, (int)VertexCount);
            }
        }

        protected void GenerateVertices()
        {
            var bottomCenter = new PNC_VertexAttribute(new Vector3(0, 0, 0), -Vector3.UnitX, Color);

            for (var i = 0; i < Count; ++i)
            {
                // add center
                VertexList.Add(bottomCenter);

                var rad1 = OpenTK.MathHelper.DegreesToRadians((360 / (double)Count) * i);
                var y1 = Radius * Math.Sin(rad1);
                var z1 = Radius * Math.Cos(rad1);
                var position1 = new Vector3(0, (float)y1, (float)z1);

                // add V1
                VertexList.Add(new PNC_VertexAttribute(position1, -Vector3.UnitX, Color));

                var rad2 = OpenTK.MathHelper.DegreesToRadians((360 / (double)Count) * (i + 1));
                var y2 = Radius * Math.Sin(rad2);
                var z2 = Radius * Math.Cos(rad2);
                var position2 = new Vector3(0, (float)y2, (float)z2);

                // add V2
                VertexList.Add(new PNC_VertexAttribute(position2, -Vector3.UnitX, Color));
            }

            var topCenter = new PNC_VertexAttribute(new Vector3(Height, 0, 0), Vector3.UnitX, Color);

            for(var i = 0; i < Count; ++i)
            {
                var rad1 = OpenTK.MathHelper.DegreesToRadians((360 / (double)Count) * i);
                var y1 = Radius * Math.Sin(rad1);
                var z1 = Radius * Math.Cos(rad1);
                var position1 = new Vector3(0, (float)y1, (float)z1);

                var rad2 = OpenTK.MathHelper.DegreesToRadians((360 / (double)Count) * (i + 1));
                var y2 = Radius * Math.Sin(rad2);
                var z2 = Radius * Math.Cos(rad2);
                var position2 = new Vector3(0, (float)y2, (float)z2);

                var d1 = (position2 - new Vector3(Height, 0, 0));
                var d2 = (position1 - new Vector3(Height, 0, 0));

                var normal= Vector3.Cross(d2, d1).Normalized();

                // add top center
                VertexList.Add(new PNC_VertexAttribute(new Vector3(Height, 0,0), normal, Color));

                // add V1
                VertexList.Add(new PNC_VertexAttribute(position1, normal, Color));

                // add V2
                VertexList.Add(new PNC_VertexAttribute(position2, normal, Color));

            }

            VertexCount = VertexList.Count;
        }

        

        protected float Radius = 1.0f;
        protected float Height = 10.0f;
        protected uint Count = 10;
        protected Vector3 Color = new Vector3(1, 0, 0);

        protected int VertexCount = 0;

        protected List<PNC_VertexAttribute> VertexList = new List<PNC_VertexAttribute>();
        protected StaticVertexBuffer<PNC_VertexAttribute> VB = null;
    }
}
