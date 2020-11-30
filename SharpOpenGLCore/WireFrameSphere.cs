using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompiledMaterial.GBufferPNC;
using Core.Primitive;
using OpenTK;
using Core;
using Core.MaterialBase;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace SharpOpenGL
{
    public class WireFrameSphere : GameObject
    {
        public override Matrix4 LocalMatrix
        {
            get
            {
                return Matrix4.CreateScale(Scale) * Matrix4.CreateRotationY(Yaw) * Matrix4.CreateRotationX(Pitch) * Matrix4.CreateTranslation(Translation);
            }
        }

        public WireFrameSphere()
        {
            Radius = 10;
            StackCount = 50;
            SectorCount = 50;
            Initialize();
        }

        protected override void PrepareRenderingData()
        {
        }

        public override bool IsEditable { get; set; } = false;

        public WireFrameSphere(float r, int stack, int sector)
        {
            Radius = r;
            StackCount = stack;
            SectorCount = sector;
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

                VertexList.Clear();

                defaultMaterial = ShaderManager.Get().GetMaterial<GBufferPNC>();

                bReadyToDraw = true;
            });
        }

        protected void GenerateVertices()
        {
            for (int i = 0; i < StackCount; ++i)
            {
                var degree1 = (180.0f / (double)StackCount) * (double)i;
                var degree2 = (180.0f / (double)StackCount) * (double)(i + 1);

                var sectorRadius1 = Radius * Math.Sin(OpenTK.Mathematics.MathHelper.DegreesToRadians(degree1));
                var sectorRadius2 = Radius * Math.Sin(OpenTK.Mathematics.MathHelper.DegreesToRadians(degree2));

                var y1 = Radius * Math.Cos(OpenTK.Mathematics.MathHelper.DegreesToRadians(degree1));
                var y2 = Radius * Math.Cos(OpenTK.Mathematics.MathHelper.DegreesToRadians(degree2));

                for (int j = 0; j < SectorCount; ++j)
                {
                    var deg1 = (360 / (double)SectorCount) * j;
                    var deg2 = (360 / (double)SectorCount) * (j + 1);

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
                    var V2 = new Vector3((float)x2, (float)y1, (float)z2);
                    var V3 = new Vector3((float)x3, (float)y2, (float)z3);
                    var V4 = new Vector3((float)x4, (float)y2, (float)z4);
                    

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


                    VertexList.Add(new PNC_VertexAttribute(V1, norm1, Color));
                    VertexList.Add(new PNC_VertexAttribute(V2, norm1, Color));
                    VertexList.Add(new PNC_VertexAttribute(V3, norm1, Color));

                    VertexList.Add(new PNC_VertexAttribute(V3, norm2, Color));
                    VertexList.Add(new PNC_VertexAttribute(V2, norm2, Color));
                    VertexList.Add(new PNC_VertexAttribute(V4, norm2, Color));
                }
            }
        }


        public override void Render()
        {
            if(bReadyToDraw)
            {
                using (var dummy = new ScopedBind(defaultMaterial))
                using (var wire = new WireFrameMode())
                {
                    var gbufferDraw = (GBufferPNC)defaultMaterial;

                    gbufferDraw.CameraTransform_View = CameraManager.Get().CurrentCameraView;
                    gbufferDraw.CameraTransform_Proj = CameraManager.Get().CurrentCameraProj;
                    gbufferDraw.Model = this.LocalMatrix;

                    drawable.DrawArrays(PrimitiveType.Triangles);
                }
            }
        }

        protected float Radius = 10.0f;
        protected int StackCount = 10;
        protected int SectorCount = 10;
        protected Vector3 Color = new Vector3(0, 1, 0);
        protected DrawableBase<PNC_VertexAttribute> drawable = null;
        protected List<PNC_VertexAttribute> VertexList = new List<PNC_VertexAttribute>();
        protected MaterialBase wireframeMaterial = null;
    }
}
