using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Primitive;
using Core.Texture;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SharpOpenGL
{
    public class Cube : SceneObject
    {
        private static int CubeCount = 0;

        public override OpenTK.Matrix4 LocalMatrix
        {
            get
            {
                return Matrix4.CreateScale(Scale) * Matrix4.CreateRotationY(Yaw) * Matrix4.CreateRotationX(Pitch) * Matrix4.CreateTranslation(Translation);
            }
        }

        

        public Cube()
            : base("Cube", CubeCount++)
        {
            Initialize();
        }

        public override bool IsEditable { get; set; } = true;

        private void GenerateVertices()
        {
            VertexList = new List<PNT_VertexAttribute>();

            // front face
            VertexList.Add(new PNT_VertexAttribute(new OpenTK.Vector3(-1.0f, -1.0f, -1.0f), -Vector3.UnitZ, new Vector2(0, 0)));
            VertexList.Add(new PNT_VertexAttribute(new OpenTK.Vector3(-1.0f, 1.0f, -1.0f), -Vector3.UnitZ, new Vector2(0, 1)));
            VertexList.Add(new PNT_VertexAttribute(new OpenTK.Vector3(1.0f, -1.0f, -1.0f), -Vector3.UnitZ, new Vector2(1, 0)));

            VertexList.Add(new PNT_VertexAttribute(new OpenTK.Vector3(-1.0f, 1.0f, -1.0f), -Vector3.UnitZ, new Vector2(0, 1)));
            VertexList.Add(new PNT_VertexAttribute(new OpenTK.Vector3(1.0f, 1.0f, -1.0f), -Vector3.UnitZ, new Vector2(1, 1)));
            VertexList.Add(new PNT_VertexAttribute(new OpenTK.Vector3(1.0f, -1.0f, -1.0f), -Vector3.UnitZ, new Vector2(0, 0)));

            // back face
            VertexList.Add(new PNT_VertexAttribute(new OpenTK.Vector3(1.0f, -1.0f, 1.0f), Vector3.UnitZ, new Vector2(1, 0)));
            VertexList.Add(new PNT_VertexAttribute(new OpenTK.Vector3(-1.0f, 1.0f, 1.0f), Vector3.UnitZ, new Vector2(0, 1)));
            VertexList.Add(new PNT_VertexAttribute(new OpenTK.Vector3(-1.0f, -1.0f, 1.0f), Vector3.UnitZ, new Vector2(0, 0)));

            VertexList.Add(new PNT_VertexAttribute(new OpenTK.Vector3(1.0f, -1.0f, 1.0f), Vector3.UnitZ, new Vector2(0, 0)));
            VertexList.Add(new PNT_VertexAttribute(new OpenTK.Vector3(1.0f, 1.0f, 1.0f), Vector3.UnitZ, new Vector2(1, 1)));
            VertexList.Add(new PNT_VertexAttribute(new OpenTK.Vector3(-1.0f, 1.0f, 1.0f), Vector3.UnitZ, new Vector2(0, 1)));

            // up face
            VertexList.Add(new PNT_VertexAttribute(new OpenTK.Vector3(-1.0f, 1.0f, -1.0f), Vector3.UnitZ, new Vector2(1, 0)));
            VertexList.Add(new PNT_VertexAttribute(new OpenTK.Vector3(-1.0f, 1.0f, 1.0f), Vector3.UnitZ, new Vector2(0, 1)));
            VertexList.Add(new PNT_VertexAttribute(new OpenTK.Vector3(1.0f, 1.0f, -1.0f), Vector3.UnitZ, new Vector2(0, 0)));

            VertexList.Add(new PNT_VertexAttribute(new OpenTK.Vector3(-1.0f, 1.0f, 1.0f), Vector3.UnitZ, new Vector2(1, 0)));
            VertexList.Add(new PNT_VertexAttribute(new OpenTK.Vector3(1.0f, 1.0f, 1.0f), Vector3.UnitZ, new Vector2(0, 1)));
            VertexList.Add(new PNT_VertexAttribute(new OpenTK.Vector3(1.0f, 1.0f, -1.0f), Vector3.UnitZ, new Vector2(0, 0)));

            // down face
        }

        public override void Initialize()
        {
            this.Scale = 50.0f;

            GenerateVertices();
            
            RenderingThread.Get().ExecuteImmediatelyIfRenderingThread(() =>
            {
                drawable = new DrawableBase<PNT_VertexAttribute>();
                var vertexArray = VertexList.ToArray();
                drawable.SetupVertexData(ref vertexArray);

                VertexList.Clear();

                defaultMaterial = ShaderManager.Get().GetMaterial<GBufferCubeTest.GBufferCubeTest>();
                var hdr = new HDRTexture();
                hdr.Load("./Resources/Texture/HDR/Alexs_Apt_2k.hdr");
                cubemapTex = hdr;

                bReadyToDraw = true;
            });
        }

        public override void Draw()
        {   
            if (bReadyToDraw)
            {
                using (var dummy = new ScopedBind(defaultMaterial))
                {
                    var gbufferDraw = (GBufferCubeTest.GBufferCubeTest)defaultMaterial;

                    gbufferDraw.CameraTransform_View = CameraManager.Get().CurrentCameraView;
                    gbufferDraw.CameraTransform_Proj = CameraManager.Get().CurrentCameraProj;
                    gbufferDraw.ModelTransform_Model = this.LocalMatrix;

                    gbufferDraw.EquirectangularMap2D = cubemapTex;

                    drawable.DrawArrays(PrimitiveType.Triangles);
                }
            }
        }

        private List<PNT_VertexAttribute> VertexList = new List<PNT_VertexAttribute>();

        protected DrawableBase<PNT_VertexAttribute> drawable = null;

        protected TextureBase cubemapTex = null;
    }
}
