using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Core;
using Core.CustomAttribute;
using Core.MaterialBase;
using Core.Primitive;

namespace SharpOpenGL
{
    public class Sphere : SceneObject
    {
        public override OpenTK.Matrix4 LocalMatrix
        {
            get
            {
                return Matrix4.CreateScale(Scale) * Matrix4.CreateRotationY(Yaw) * Matrix4.CreateRotationX(Pitch) * Matrix4.CreateTranslation(Translation);
            }
        }

        public Sphere()
        {
            Radius = 10;
            StackCount = 50;
            SectorCount = 50;
            Initialize();
        }


        public Sphere(float radius, int stackcount, int sectorcount)
        {
            Debug.Assert(radius > 0 && StackCount > 0 && SectorCount > 0);
            Radius = radius;
            StackCount = stackcount;
            SectorCount = sectorcount;

            Initialize();
        }
        public override void Initialize()
        {   
            GenerateVertices();

            RenderingThread.Get().ExecuteImmediatelyIfRenderingThread(() =>
            {
                drawable = new DrawableBase<PNCT_VertexAttribute>();
                var vertexArray = VertexList.ToArray();
                drawable.SetupVertexData(ref vertexArray);

                VertexList.Clear();

                defaultMaterial = ShaderManager.Get().GetMaterial<GBufferPNCT.GBufferPNCT>();
                bReadyToDraw = true;
            });
        }

        public override void Draw()
        {
            if (bReadyToDraw)
            {
                using (var dummy = new ScopedBind(defaultMaterial))
                {
                    defaultMaterial.SetUniformVarData("Model", LocalMatrix * ParentMatrix, true);
                    drawable.DrawPrimitiveWithoutIndex(PrimitiveType.Triangles);
                }
            }
        }

        public override void Tick(double delta)
        {
            //Yaw += (float)OpenTK.MathHelper.DegreesToRadians(delta) * 50;
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

                var v1 = (float) i / (float) StackCount;
                var v2 = (float) (i + 1) / (float) StackCount;

                for (int j = 0; j < SectorCount; ++j)
                {
                    var deg1 = (360 / (double)SectorCount) * j;
                    var deg2 = (360 / (double)SectorCount) * (j+1);

                    var u1 = (float) j / (float) SectorCount;
                    var u2 = (float) (j+1) / (float)SectorCount;

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
                    var T1 = new Vector2(u1, v1);

                    var V2 = new Vector3((float)x2, (float)y1, (float)z2);
                    var T2 = new Vector2(u2,v1);

                    var V3 = new Vector3((float)x3, (float)y2, (float)z3);
                    var T3 = new Vector2(u1, v2);

                    var V4 = new Vector3((float)x4, (float)y2, (float)z4);
                    var T4 = new Vector2(u2, v2);

                    var d1 = (V2 - V1).Normalized();
                    var d2 = (V3 - V1).Normalized();

                    var d3 = (V2 - V4).Normalized();
                    var d4 = (V3 - V4).Normalized();

                    var norm1 = Vector3.Cross(d1, d2).Normalized();
                    var norm2 = Vector3.Cross(d4, d3).Normalized();

                    // V1-----V2
                    //       /
                    //      /
                    //     /
                    //    /
                    //   /
                    //  /
                    // V3-----V4

                    // face1
                    VertexList.Add(new PNCT_VertexAttribute(V1, norm1, Color, T1));
                    VertexList.Add(new PNCT_VertexAttribute(V2, norm1, Color, T2));
                    VertexList.Add(new PNCT_VertexAttribute(V3, norm1, Color, T3));

                    // face2
                    VertexList.Add(new PNCT_VertexAttribute(V3, norm2, Color, T3));
                    VertexList.Add(new PNCT_VertexAttribute(V2, norm2, Color, T2));
                    VertexList.Add(new PNCT_VertexAttribute(V4, norm2, Color, T4));
                }
            }

            VertexCount = VertexList.Count;
        }

        List<PNCT_VertexAttribute> VertexList = new List<PNCT_VertexAttribute>();

        [ExposeUI] private float Specular = 0.1f;

        int VertexCount = 0;
        protected float Radius = 10.0f;
        protected int StackCount = 10;
        protected int SectorCount = 10;
        protected Vector3 Color = new Vector3(1, 0, 0);
        protected DrawableBase<PNCT_VertexAttribute> drawable = null;
    }
}
