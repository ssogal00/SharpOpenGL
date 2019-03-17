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
using MathHelper = Core.MathHelper;

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

        public override void Tick(double Elapsed)
        {
            this.Yaw += (float) OpenTK.MathHelper.DegreesToRadians(Elapsed * 10.0f);
        }

        public override bool IsEditable { get; set; } = true;

        private void GenerateVertices()
        {
            VertexList = new List<PN_VertexAttribute>();

            // front face
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(-1.0f, -1.0f, -1.0f), -Vector3.UnitZ));
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(-1.0f, 1.0f, -1.0f), -Vector3.UnitZ));
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(1.0f, -1.0f, -1.0f), -Vector3.UnitZ));

            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(-1.0f, 1.0f, -1.0f), -Vector3.UnitZ));
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(1.0f, 1.0f, -1.0f), -Vector3.UnitZ));
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(1.0f, -1.0f, -1.0f), -Vector3.UnitZ));

            // back face
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(1.0f, -1.0f, 1.0f), Vector3.UnitZ));
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(-1.0f, 1.0f, 1.0f), Vector3.UnitZ));
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(-1.0f, -1.0f, 1.0f), Vector3.UnitZ));

            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(1.0f, -1.0f, 1.0f), Vector3.UnitZ));
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(1.0f, 1.0f, 1.0f), Vector3.UnitZ));
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(-1.0f, 1.0f, 1.0f), Vector3.UnitZ));

            // right face
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(1.0f, -1.0f, -1.0f), Vector3.UnitX));
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(1.0f, 1.0f, -1.0f), Vector3.UnitX));
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(1.0f, 1.0f, 1.0f), Vector3.UnitX));

            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(1.0f, -1.0f, -1.0f), Vector3.UnitX));
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(1.0f, 1.0f, 1.0f), Vector3.UnitX));
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(1.0f, -1.0f, 1.0f), Vector3.UnitX));

            // left face
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(-1.0f, 1.0f, 1.0f), -Vector3.UnitX));
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(-1.0f, 1.0f, -1.0f), -Vector3.UnitX));
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(-1.0f, -1.0f, -1.0f), -Vector3.UnitX));


            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(-1.0f, -1.0f, 1.0f), -Vector3.UnitX));
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(-1.0f, 1.0f, 1.0f), -Vector3.UnitX));
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(-1.0f, -1.0f, -1.0f), -Vector3.UnitX));
            

            // up face
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(-1.0f, 1.0f, -1.0f), Vector3.UnitY));
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(-1.0f, 1.0f, 1.0f), Vector3.UnitY));
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(1.0f, 1.0f, -1.0f), Vector3.UnitY));

            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(-1.0f, 1.0f, 1.0f), Vector3.UnitY));
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(1.0f, 1.0f, 1.0f), Vector3.UnitY));
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(1.0f, 1.0f, -1.0f), Vector3.UnitY));

            // down face
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(1.0f, -1.0f, -1.0f), Vector3.UnitY));
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(-1.0f, -1.0f, 1.0f), Vector3.UnitY));
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(-1.0f, -1.0f, -1.0f), Vector3.UnitY));


            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(1.0f, -1.0f, -1.0f), Vector3.UnitY));
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(1.0f, -1.0f, 1.0f), Vector3.UnitY));
            VertexList.Add(new PN_VertexAttribute(new OpenTK.Vector3(-1.0f, -1.0f, 1.0f), Vector3.UnitY));
            

        }

        public override void Initialize()
        {
            this.Scale = 50.0f;
            Translation = new Vector3(0,50,0);

            GenerateVertices();
            
            RenderingThread.Get().ExecuteImmediatelyIfRenderingThread(() =>
            {
                drawable = new DrawableBase<PN_VertexAttribute>();
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

        private List<PN_VertexAttribute> VertexList = new List<PN_VertexAttribute>();

        protected DrawableBase<PN_VertexAttribute> drawable = null;

        protected TextureBase cubemapTex = null;
    }
}
