using System;
using Core.Buffer;
using OpenTK;
using System.Collections.Generic;
using System.Diagnostics;
using Core.MaterialBase;
using OpenTK.Graphics.OpenGL;
using System.Linq;
using CompiledMaterial.GBufferPNC;
using Core;
using Core.Primitive;
using OpenTK.Mathematics;

namespace Engine
{
    public class Cylinder : GameObject
    {
        public override Matrix4 LocalMatrix
        {
            get
            {
                return Matrix4.CreateScale(Scale) * Matrix4.CreateRotationY(Yaw) * Matrix4.CreateRotationX(Pitch) * Matrix4.CreateTranslation(Translation);
            }
        }

        public Cylinder(float radius, float height, uint count)
        {
            Debug.Assert(radius > 0 && height > 0 && count >= 6);

            Radius = radius;
            Height = height;
            Count = count;
            Initialize();
        }

        public override void Initialize()
        {
            GenerateVertices();

            RenderingThread.Get().ExecuteImmediatelyIfRenderingThread(() =>
            {
                drawable = new DrawableBase<PNC_VertexAttribute>();
                var vertexArray = VertexList.ToArray();
                drawable.SetupVertexData(ref vertexArray);

                defaultMaterial = ShaderManager.Get().GetMaterial<GBufferPNC>();

                VertexList.Clear();
                bReadyToDraw = true;
            });
        }

        protected override void PrepareRenderingData()
        {
        }
        public override void Render()
        {
            if (bReadyToDraw)
            {
                using (var wireframe = new WireFrameMode())
                using (var dummy = new ScopedBind(defaultMaterial))
                {
                    defaultMaterial.SetUniformVarData("Model", LocalMatrix * ParentMatrix, true);
                    drawable.DrawArrays(PrimitiveType.Triangles);
                }
            }
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

                var rad1 = OpenTK.Mathematics.MathHelper.DegreesToRadians((360 / (double)Count) * i);
                var y1 = Radius * Math.Sin(rad1);
                var z1 = Radius * Math.Cos(rad1);
                var position1 = new Vector3(0,(float)y1, (float)z1);
                var normal1 = -Vector3.UnitX;
                
                // add V1
                VertexList.Add(new PNC_VertexAttribute(position1, normal1, Color));

                var rad2 = OpenTK.Mathematics.MathHelper.DegreesToRadians((360 / (double)Count) * (i+1));
                var y2 = Radius * Math.Sin(rad2);
                var z2 = Radius * Math.Cos(rad2);
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

                var rad1 = OpenTK.Mathematics.MathHelper.DegreesToRadians((360 / (double)Count) * i);
                var y1 = Radius * Math.Sin(rad1);
                var z1 = Radius * Math.Cos(rad1);
                var position1 = new Vector3(Height, (float)y1, (float)z1);
                var normal1 = Vector3.UnitX;

                var rad2 = OpenTK.Mathematics.MathHelper.DegreesToRadians((360 / (double)Count) * i);
                var y2 = Radius * Math.Sin(rad2);
                var z2 = Radius * Math.Cos(rad2);
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
                
                var rad1 = OpenTK.Mathematics.MathHelper.DegreesToRadians((360 / (double)Count) * i);
                var rad2 = OpenTK.Mathematics.MathHelper.DegreesToRadians((360 / (double)Count) * (i+1));

                var y1 = Radius * Math.Sin(rad1);
                var z1 = Radius * Math.Cos(rad1);
                var y2 = Radius * Math.Sin(rad2);
                var z2 = Radius * Math.Cos(rad2);

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

            VertexCount = VertexList.Count;
        }
        
        protected float Height = 0;
        protected float Radius = 0;
        protected uint Count = 10;

        protected int VertexCount = 0;

        protected List<PNC_VertexAttribute> VertexList = new List<PNC_VertexAttribute>();

        protected DrawableBase<PNC_VertexAttribute> drawable = null;
        public Vector3 Color = new Vector3(1, 0, 0);
    }
}

