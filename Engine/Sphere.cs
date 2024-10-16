﻿using Core;
using Core.CustomAttribute;
using Core.Primitive;
using Core.Texture;
using Engine.Rendering;
using GLTF;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using AttributeType = GLTF.V2.AttributeType;

namespace Engine
{
    public class Sphere : GameObject
    {
        protected static int SphereCount = 0;

        public override Matrix4 LocalMatrix
        {
            get
            {
                return Matrix4.CreateScale(Scale) * Matrix4.CreateRotationY(Yaw) * Matrix4.CreateRotationX(Pitch) * Matrix4.CreateTranslation(Translation);
            }
        }

        public Sphere()
        : base("Sphere")
        {
            Radius = 10;
            StackCount = 25;
            SectorCount = 25;
            GenerateVertices();
        }

        public override bool IsEditable { get; set; } = false;


        public Sphere(float radius, int stackcount, int sectorcount)
            : base("Sphere")
        {
            Debug.Assert(radius > 0 && StackCount > 0 && SectorCount > 0);
            Radius = radius;
            StackCount = stackcount;
            SectorCount = sectorcount;
            GenerateVertices();
        }
        public override void Initialize()
        {   
            GenerateVertices();
        }

        protected void GenerateVertices()
        {
            for(int i = 0; i < StackCount; ++i)
            {
                var degree1 = (180.0f / (double)StackCount) * (double)i;
                var degree2 = (180.0f / (double)StackCount) * (double)(i+1);

                var sectorRadius1 = Radius * Math.Sin(OpenTK.Mathematics.MathHelper.DegreesToRadians(degree1));
                var sectorRadius2 = Radius * Math.Sin(OpenTK.Mathematics.MathHelper.DegreesToRadians(degree2));

                var y1 = Radius * Math.Cos(OpenTK.Mathematics.MathHelper.DegreesToRadians(degree1));
                var y2 = Radius * Math.Cos(OpenTK.Mathematics.MathHelper.DegreesToRadians(degree2));

                var v1 = (float) i / (float) StackCount;
                var v2 = (float) (i + 1) / (float) StackCount;

                for (int j = 0; j < SectorCount; ++j)
                {
                    var deg1 = (360 / (double)SectorCount) * j;
                    var deg2 = (360 / (double)SectorCount) * (j+1);

                    var u1 = (float) j / (float) SectorCount;
                    var u2 = (float) (j+1) / (float)SectorCount;

                    // v1
                    var x1 = sectorRadius1 * Math.Cos(OpenTK.Mathematics.MathHelper.DegreesToRadians(deg1));
                    var z1 = sectorRadius1 * Math.Sin(OpenTK.Mathematics.MathHelper.DegreesToRadians(deg1));

                    // v2
                    var x2 = sectorRadius1 * Math.Cos(OpenTK.Mathematics.MathHelper.DegreesToRadians(deg2));
                    var z2 = sectorRadius1 * Math.Sin(OpenTK.Mathematics.MathHelper.DegreesToRadians(deg2));

                    // v3
                    var x3 = sectorRadius2 * Math.Cos(OpenTK.Mathematics.MathHelper.DegreesToRadians(deg1));
                    var z3 = sectorRadius2 * Math.Sin(OpenTK.Mathematics.MathHelper.DegreesToRadians(deg1));

                    // v4
                    var x4 = sectorRadius2 * Math.Cos(OpenTK.Mathematics.MathHelper.DegreesToRadians(deg2));
                    var z4 = sectorRadius2 * Math.Sin(OpenTK.Mathematics.MathHelper.DegreesToRadians(deg2));

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

                    //var norm1 = Vector3.Cross(d1, d2).Normalized();
                    //var norm2 = Vector3.Cross(d4, d3).Normalized();
                    var norm1 = V1.Normalized();
                    var norm2 = V2.Normalized();
                    var norm3 = V3.Normalized();
                    var norm4 = V4.Normalized();

                    // V1-----V2
                    //       /
                    //      /
                    //     /
                    //    /
                    //   /
                    //  /
                    // V3-----V4

                    TempVertexList.Add(V1);
                    TempVertexList.Add(V2);
                    TempVertexList.Add(V3);

                    TempVertexList.Add(V3);
                    TempVertexList.Add(V2);
                    TempVertexList.Add(V4);

                    TempNormalList.Add(norm1);
                    TempNormalList.Add(norm2);
                    TempNormalList.Add(norm3);

                    TempNormalList.Add(norm3);
                    TempNormalList.Add(norm2);
                    TempNormalList.Add(norm4);

                    TempTexCoordList.Add(T1);
                    TempTexCoordList.Add(T2);
                    TempTexCoordList.Add(T3);

                    TempTexCoordList.Add(T3);
                    TempTexCoordList.Add(T2);
                    TempTexCoordList.Add(T4);
                }
            }

            GenerateTangents();

            for (int i = 0; i < TempVertexList.Count; i += 3)
            {
                var V1 = TempVertexList[i];
                var V2 = TempVertexList[i + 1];
                var V3 = TempVertexList[i + 2];

                var norm1 = TempNormalList[i];
                var norm2 = TempNormalList[i+1];
                var norm3 = TempNormalList[i+2];

                var tan1 = TempTangentList[i];
                var tan2 = TempTangentList[i+1];
                var tan3 = TempTangentList[i+2];

                var T1 = TempTexCoordList[i];
                var T2 = TempTexCoordList[i+1];
                var T3 = TempTexCoordList[i+2];

                // face1
                VertexList.Add(new PNTT_VertexAttribute(V1, norm1,  T1, tan1));
                VertexList.Add(new PNTT_VertexAttribute(V2, norm2, T2, tan2));
                VertexList.Add(new PNTT_VertexAttribute(V3, norm3,  T3, tan3));
            }
            

            mVertexCount = VertexList.Count;

            var positionAttribute = new VertexAttributeSemantic(0, "POSITION", AttributeType.VEC3);
            var normalAttribute = new VertexAttributeSemantic(1, "NORMAL", AttributeType.VEC3);
            var texcoordAttribute = new VertexAttributeSemantic(2, "TEXCOORD", AttributeType.VEC2);
            var tangentAttribute = new VertexAttributeSemantic(3, "TANGENT", AttributeType.VEC4);

            var vector3VertexAttributes = new Dictionary<VertexAttributeSemantic, List<Vector3>>();
            var vector2VertexAttributes = new Dictionary<VertexAttributeSemantic, List<Vector2>>();
            var vector4VertexAttributes = new Dictionary<VertexAttributeSemantic, List<Vector4>>();

            vector3VertexAttributes.Add(positionAttribute, TempVertexList);
            vector3VertexAttributes.Add(normalAttribute, TempNormalList);
            vector2VertexAttributes.Add(texcoordAttribute, TempTexCoordList);
            vector4VertexAttributes.Add(tangentAttribute, TempTangentList);

            var vertexAttributeMap = new Dictionary<string, VertexAttributeSemantic>();

            vertexAttributeMap.Add(positionAttribute.AttributeName, positionAttribute);
            vertexAttributeMap.Add(normalAttribute.AttributeName, normalAttribute);
            vertexAttributeMap.Add(texcoordAttribute.AttributeName, texcoordAttribute);
            vertexAttributeMap.Add(tangentAttribute.AttributeName, tangentAttribute);

            mMeshSectionList.Add(new MeshSection("GBufferDraw", 
                vertexAttributeMap,
                vector2VertexAttributes,
                vector3VertexAttributes,
                vector4VertexAttributes, null, null));
        }

        protected void GenerateTangents()
        {
            List<Vector3> tan1Accum = new List<Vector3>();
            List<Vector3> tan2Accum = new List<Vector3>();

            for (uint i = 0; i < TempVertexList.Count; ++i)
            {
                tan1Accum.Add(new Vector3(0, 0, 0));
                tan2Accum.Add(new Vector3(0, 0, 0));
            }

            for (uint i = 0; i < TempVertexList.Count; i++)
            {
                TempTangentList.Add(new Vector4(0, 0, 0, 0));
            }

            // Compute the tangent vector
            for (uint i = 0; i < TempVertexList.Count; i += 3)
            {
                var p1 = TempVertexList[(int)i];
                var p2 = TempVertexList[(int)i + 1];
                var p3 = TempVertexList[(int)i + 2];

                var tc1 = TempTexCoordList[(int) i];
                var tc2 = TempTexCoordList[(int)i + 1];
                var tc3 = TempTexCoordList[(int)i + 2];

                Vector3 q1 = p2 - p1;
                Vector3 q2 = p3 - p1;
                float s1 = tc2.X - tc1.X, s2 = tc3.X - tc1.X;
                float t1 = tc2.Y - tc1.Y, t2 = tc3.Y - tc1.Y;

                // prevent degeneration
                float r = 1.0f / (s1 * t2 - s2 * t1);
                if (Single.IsInfinity(r))
                {
                    r = 1 / 0.1f;
                }

                var tan1 = new Vector3((t2 * q1.X - t1 * q2.X) * r,
                   (t2 * q1.Y - t1 * q2.Y) * r,
                   (t2 * q1.Z - t1 * q2.Z) * r);

                var tan2 = new Vector3((s1 * q2.X - s2 * q1.X) * r,
                   (s1 * q2.Y - s2 * q1.Y) * r,
                   (s1 * q2.Z - s2 * q1.Z) * r);


                tan1Accum[(int)i] += tan1;
                tan1Accum[(int)i + 1] += tan1;
                tan1Accum[(int)i + 2] += tan1;

                tan2Accum[(int)i] += tan2;
                tan2Accum[(int)i + 1] += tan2;
                tan2Accum[(int)i + 2] += tan2;
            }

            Vector4 lastValidTangent = new Vector4();

            for (uint i = 0; i < TempVertexList.Count; ++i)
            {
                var n = TempNormalList[(int)i];
                var t1 = tan1Accum[(int)i];
                var t2 = tan2Accum[(int)i];

                // Gram-Schmidt orthogonalize                
                var temp = Vector3.Normalize(t1 - (Vector3.Dot(n, t1) * n));
                // Store handedness in w                
                var W = (Vector3.Dot(Vector3.Cross(n, t1), t2) < 0.0f) ? -1.0f : 1.0f;

                bool bValid = true;
                if (Single.IsNaN(temp.X) || Single.IsNaN(temp.Y) || Single.IsNaN(temp.Z))
                {
                    bValid = false;
                }

                if (Single.IsInfinity(temp.X) || Single.IsInfinity(temp.Y) || Single.IsInfinity(temp.Z))
                {
                    bValid = false;
                }

                if (bValid == true)
                {
                    lastValidTangent = new Vector4(temp.X, temp.Y, temp.Z, W);
                }

                if (bValid == false)
                {
                    temp = lastValidTangent.Xyz;
                }

                TempTangentList[(int)i] = new Vector4(temp.X, temp.Y, temp.Z, W);
            }

            tan1Accum.Clear();
            tan2Accum.Clear();
        }

        protected List<PNTT_VertexAttribute> VertexList = new List<PNTT_VertexAttribute>();

        [ExposeUI]
        public float Specular = 0.1f;

        

        protected float Radius = 10.0f;
        protected int StackCount = 10;
        protected int SectorCount = 10;
        protected Vector3 Color = new Vector3(1, 0, 0);
        protected DrawableBase<PNTT_VertexAttribute> drawable = null;

        protected List<Vector4> TempTangentList = new List<Vector4>();
        protected List<Vector2> TempTexCoordList = new List<Vector2>();
        protected List<Vector3> TempVertexList = new List<Vector3>();
        protected List<Vector3> TempNormalList = new List<Vector3>();

        protected TextureBase normalTex = null;
        protected TextureBase diffuseTex = null;
        protected TextureBase roughTex = null;
        protected TextureBase metalicTex = null;

        public float Roughness { get; set; } = 0.0f;
        public float Metallic { get; set; } = 0.0f;
    }
}
