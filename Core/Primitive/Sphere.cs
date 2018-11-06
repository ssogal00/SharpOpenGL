using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Core;

namespace Core.Primitive
{
    public class Sphere : RenderResource, ISceneObject
    {
        // @ ISceneobject interface
        public Vector3 Translation { get; set; } = new Vector3(0, 0, 0);

        public float Scale { get; set; } = 1.0f;

        public OpenTK.Matrix4 ParentMatrix { get; set; } = Matrix4.Identity;

        public OpenTK.Matrix4 ModelMatrix
        {
            get
            {
                return Matrix4.CreateScale(Scale) * Matrix4.CreateRotationY(Yaw) * Matrix4.CreateRotationX(Pitch) * Matrix4.CreateTranslation(Translation);
            }
        }

        public float Yaw { get; set; } = 0;
        public float Pitch { get; set; } = 0;
        public float Roll { get; set; } = 0;
        // @ ISceneobject interface

        public Sphere(float radius, int stackcount, int sectorcount)
        {
            Debug.Assert(radius > 0 && StackCount > 0 && SectorCount > 0);
            Radius = radius;
            StackCount = stackcount;
            SectorCount = sectorcount;
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
            material.SetUniformVarData("Model", ModelMatrix * ParentMatrix , true);
            drawable.DrawPrimitiveWithoutIndex(PrimitiveType.Triangles);
        }


        protected void GenerateVertices()
        {
            for(int i = 0; i < StackCount; ++i)
            {
                var degree1 = (180.0f / (double)StackCount) * (double)i;
                var degree2 = (180.0f / (double)StackCount) * (double)(i+1);

                var sectorRadius1 = Radius * Math.Sin(OpenTK.MathHelper.DegreesToRadians(degree1));
                var sectorRadius2 = Radius * Math.Sin(OpenTK.MathHelper.DegreesToRadians(degree2));

                var y1 = Radius * Math.Cos(OpenTK.MathHelper.DegreesToRadians(degree1));
                var y2 = Radius * Math.Cos(OpenTK.MathHelper.DegreesToRadians(degree2));

                for (int j = 0; j < SectorCount; ++j)
                {
                    var deg1 = (360 / (double)SectorCount) * j;
                    var deg2 = (360 / (double)SectorCount) * (j+1);

                    // v1
                    var x1 = sectorRadius1 * Math.Cos(OpenTK.MathHelper.DegreesToRadians(deg1));
                    var z1 = sectorRadius1 * Math.Sin(OpenTK.MathHelper.DegreesToRadians(deg1));

                    // v2
                    var x2 = sectorRadius1 * Math.Cos(OpenTK.MathHelper.DegreesToRadians(deg2));
                    var z2 = sectorRadius1 * Math.Sin(OpenTK.MathHelper.DegreesToRadians(deg2));

                    // v3
                    var x3 = sectorRadius2 * Math.Cos(OpenTK.MathHelper.DegreesToRadians(deg1));
                    var z3 = sectorRadius2 * Math.Sin(OpenTK.MathHelper.DegreesToRadians(deg1));

                    // v4
                    var x4 = sectorRadius2 * Math.Cos(OpenTK.MathHelper.DegreesToRadians(deg2));
                    var z4 = sectorRadius2 * Math.Sin(OpenTK.MathHelper.DegreesToRadians(deg2));

                    var V1 = new Vector3((float)x1, (float)y1, (float)z1);
                    var V2 = new Vector3((float)x2, (float)y1, (float)z2);
                    var V3 = new Vector3((float)x3, (float)y2, (float)z3);
                    var V4 = new Vector3((float)x4, (float)y2, (float)z4);

                    var d1 = (V2 - V1).Normalized();
                    var d2 = (V3 - V1).Normalized();

                    var d3 = (V2 - V4).Normalized();
                    var d4 = (V3 - V4).Normalized();

                    var norm1 = Vector3.Cross(d2, d1).Normalized();
                    var norm2 = Vector3.Cross(d3, d4).Normalized();

                    // V1-----V2
                    //       /
                    //      /
                    //     /
                    //    /
                    //   /
                    //  /
                    // V3-----V4

                    // face1
                    VertexList.Add(new PNC_VertexAttribute(V1, norm1, Color));
                    VertexList.Add(new PNC_VertexAttribute(V2, norm1, Color));
                    VertexList.Add(new PNC_VertexAttribute(V3, norm1, Color));

                    // face2
                    VertexList.Add(new PNC_VertexAttribute(V3, norm2, Color));
                    VertexList.Add(new PNC_VertexAttribute(V2, norm2, Color));
                    VertexList.Add(new PNC_VertexAttribute(V4, norm2, Color));
                }
            }

            VertexCount = VertexList.Count;
        }

        List<PNC_VertexAttribute> VertexList = new List<PNC_VertexAttribute>();

        int VertexCount = 0;
        protected float Radius = 10.0f;
        protected int StackCount = 10;
        protected int SectorCount = 10;
        protected Vector3 Color = new Vector3(1, 0, 0);
        protected DrawableBase<PNC_VertexAttribute> drawable = null;
    }
}
