using System;
using Core.Buffer;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Diagnostics;

namespace Core.Primitive
{
    public class Cylinder : RenderResource
    {
        public Cylinder(float radius, float height, uint count)
        {
            Debug.Assert(radius > 0 && height > 0 && count >= 6);

            Radius = radius;
            Height = height;
            Count = count;
        }

        public override void Initialize()
        {
            base.Initialize();
            VB = new StaticVertexBuffer<PNC_VertexAttribute>();
        }


        protected void GenerateVertices()
        {
            VertexList.Clear();

            // bottom
            var bottomCenter = new PNC_VertexAttribute(new Vector3(0, 0, 0), -Vector3.UnitX, Color);
            
            for(var i = 0; i < Count; ++i)
            {
                // add center
                VertexList.Add(bottomCenter);

                var rad1 = OpenTK.MathHelper.DegreesToRadians((360 / (double)Count) * i);
                var y1 = Radius * Math.Cos(rad1);
                var z1 = Radius * Math.Sin(rad1);
                var position1 = new Vector3(0,(float)y1, (float)z1);
                var normal1 = -Vector3.UnitX;
                
                // add V1
                VertexList.Add(new PNC_VertexAttribute(position1, normal1, Color));

                var rad2 = OpenTK.MathHelper.DegreesToRadians((360 / (double)Count) * (i+1));
                var y2 = Radius * Math.Cos(rad2);
                var z2 = Radius * Math.Sin(rad2);
                var position2 = new Vector3(0, (float)y2, (float)z2);
                var normal2 = -Vector3.UnitX;

                // add V2
                VertexList.Add(new PNC_VertexAttribute(position2, normal2, Color));
            }

            // top
            var topCenter = new PNC_VertexAttribute(new Vector3(Height, 0, 0), Vector3.UnitX, Color);

            for (var i =0; i < Count;++i)
            {
                VertexList.Add(topCenter);

                var rad1 = OpenTK.MathHelper.DegreesToRadians((360 / (double)Count) * i);
                var y1 = Radius * Math.Cos(rad1);
                var z1 = Radius * Math.Sin(rad1);
                var position1 = new Vector3(Height, (float)y1, (float)z1);
                var normal1 = Vector3.UnitX;

                var rad2 = OpenTK.MathHelper.DegreesToRadians((360 / (double)Count) * i);
                var y2 = Radius * Math.Cos(rad2);
                var z2 = Radius * Math.Sin(rad2);
                var position2 = new Vector3(Height, (float)y2, (float)z2);
                var normal2 = Vector3.UnitX;

                // add V2
                VertexList.Add(new PNC_VertexAttribute(position2, normal2, Color));
                // add V1
                VertexList.Add(new PNC_VertexAttribute(position1, normal1, Color));
            }

            for(var i = 0; i < Count;++i)
            {
                // V1 ---- V2
                // |     / |
                // |    /  |
                // |   /   |
                // |  /    |
                // | /     |
                // |/      |
                // V3 ---- V4
                
                var rad1 = OpenTK.MathHelper.DegreesToRadians((360 / (double)Count) * i);
                var rad2 = OpenTK.MathHelper.DegreesToRadians((360 / (double)Count) * (i+1));

                var y1 = Radius * Math.Cos(rad1);
                var z1 = Radius * Math.Sin(rad1);
                var y2 = Radius * Math.Cos(rad2);
                var z2 = Radius * Math.Sin(rad2);

                var v1 = new Vector3(0, (float)y1, (float)z1);
                var v2 = new Vector3(Height, (float)y1, (float)z1);

                var v3 = new Vector3(0, (float)y2, (float)z2);
                var v4 = new Vector3(Height, (float)y2, (float)z2);

                var d1 = (v3 - v1).Normalized();
                var d2 = (v2 - v1).Normalized();
                var d3 = (v3 - v4).Normalized();
                var d4 = (v2 - v4).Normalized();

                var norm1 = Vector3.Cross(d2, d1).Normalized();
                var norm2 = Vector3.Cross(d4, d3).Normalized();

                VertexList.Add(new PNC_VertexAttribute(v1, norm1, Color));
                VertexList.Add(new PNC_VertexAttribute(v2, norm1, Color));
                VertexList.Add(new PNC_VertexAttribute(v3, norm1, Color));

                VertexList.Add(new PNC_VertexAttribute(v2, norm2, Color));
                VertexList.Add(new PNC_VertexAttribute(v4, norm2, Color));
                VertexList.Add(new PNC_VertexAttribute(v3, norm2, Color));
            }
        }

        protected float Height = 0;
        protected float Radius = 0;
        protected uint Count = 10;

        protected List<PNC_VertexAttribute> VertexList = new List<PNC_VertexAttribute>();
        protected StaticVertexBuffer<PNC_VertexAttribute> VB = null;
        protected IndexBuffer IB = null;
        protected Vector3 Color = new Vector3(1, 0, 0);
    }
}

