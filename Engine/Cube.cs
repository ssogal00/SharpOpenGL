using CompiledMaterial.GBufferCubeTest;
using Core;
using Core.Primitive;
using Core.Texture;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Collections.Generic;


namespace Engine
{
    
    public class Cube : GameObject
    {
        private static int CubeCount = 0;

        public override Matrix4 LocalMatrix
        {
            get
            {
                return Matrix4.CreateScale(Scale) * Matrix4.CreateRotationY(Yaw) * Matrix4.CreateRotationX(Pitch) * Matrix4.CreateTranslation(Translation);
            }
        }
        protected override void PrepareRenderingData()
        {
        }
        public Cube()
            : base("Cube")
        {
            Initialize();
        }

        public override void Tick(double Elapsed)
        {
            this.Yaw += (float) OpenTK.Mathematics.MathHelper.DegreesToRadians(Elapsed * 10.0f);
        }

        public override bool IsEditable { get; set; } = true;

        private void GenerateVertices()
        {
            VertexList = new List<PN_VertexAttribute>();

            // front face
            VertexList.Add(new PN_VertexAttribute(new Vector3(-1.0f, -1.0f, -1.0f), -Vector3.UnitZ));
            VertexList.Add(new PN_VertexAttribute(new Vector3(-1.0f, 1.0f, -1.0f), -Vector3.UnitZ));
            VertexList.Add(new PN_VertexAttribute(new Vector3(1.0f, -1.0f, -1.0f), -Vector3.UnitZ));

            VertexList.Add(new PN_VertexAttribute(new Vector3(-1.0f, 1.0f, -1.0f), -Vector3.UnitZ));
            VertexList.Add(new PN_VertexAttribute(new Vector3(1.0f, 1.0f, -1.0f), -Vector3.UnitZ));
            VertexList.Add(new PN_VertexAttribute(new Vector3(1.0f, -1.0f, -1.0f), -Vector3.UnitZ));

            // back face
            VertexList.Add(new PN_VertexAttribute(new Vector3(1.0f, -1.0f, 1.0f), Vector3.UnitZ));
            VertexList.Add(new PN_VertexAttribute(new Vector3(-1.0f, 1.0f, 1.0f), Vector3.UnitZ));
            VertexList.Add(new PN_VertexAttribute(new Vector3(-1.0f, -1.0f, 1.0f), Vector3.UnitZ));

            VertexList.Add(new PN_VertexAttribute(new Vector3(1.0f, -1.0f, 1.0f), Vector3.UnitZ));
            VertexList.Add(new PN_VertexAttribute(new Vector3(1.0f, 1.0f, 1.0f), Vector3.UnitZ));
            VertexList.Add(new PN_VertexAttribute(new Vector3(-1.0f, 1.0f, 1.0f), Vector3.UnitZ));

            // right face
            VertexList.Add(new PN_VertexAttribute(new Vector3(1.0f, -1.0f, -1.0f), Vector3.UnitX));
            VertexList.Add(new PN_VertexAttribute(new Vector3(1.0f, 1.0f, -1.0f), Vector3.UnitX));
            VertexList.Add(new PN_VertexAttribute(new Vector3(1.0f, 1.0f, 1.0f), Vector3.UnitX));

            VertexList.Add(new PN_VertexAttribute(new Vector3(1.0f, -1.0f, -1.0f), Vector3.UnitX));
            VertexList.Add(new PN_VertexAttribute(new Vector3(1.0f, 1.0f, 1.0f), Vector3.UnitX));
            VertexList.Add(new PN_VertexAttribute(new Vector3(1.0f, -1.0f, 1.0f), Vector3.UnitX));

            // left face
            VertexList.Add(new PN_VertexAttribute(new Vector3(-1.0f, 1.0f, 1.0f), -Vector3.UnitX));
            VertexList.Add(new PN_VertexAttribute(new Vector3(-1.0f, 1.0f, -1.0f), -Vector3.UnitX));
            VertexList.Add(new PN_VertexAttribute(new Vector3(-1.0f, -1.0f, -1.0f), -Vector3.UnitX));


            VertexList.Add(new PN_VertexAttribute(new Vector3(-1.0f, -1.0f, 1.0f), -Vector3.UnitX));
            VertexList.Add(new PN_VertexAttribute(new Vector3(-1.0f, 1.0f, 1.0f), -Vector3.UnitX));
            VertexList.Add(new PN_VertexAttribute(new Vector3(-1.0f, -1.0f, -1.0f), -Vector3.UnitX));
            

            // up face
            VertexList.Add(new PN_VertexAttribute(new Vector3(-1.0f, 1.0f, -1.0f), Vector3.UnitY));
            VertexList.Add(new PN_VertexAttribute(new Vector3(-1.0f, 1.0f, 1.0f), Vector3.UnitY));
            VertexList.Add(new PN_VertexAttribute(new Vector3(1.0f, 1.0f, -1.0f), Vector3.UnitY));

            VertexList.Add(new PN_VertexAttribute(new Vector3(-1.0f, 1.0f, 1.0f), Vector3.UnitY));
            VertexList.Add(new PN_VertexAttribute(new Vector3(1.0f, 1.0f, 1.0f), Vector3.UnitY));
            VertexList.Add(new PN_VertexAttribute(new Vector3(1.0f, 1.0f, -1.0f), Vector3.UnitY));

            // down face
            VertexList.Add(new PN_VertexAttribute(new Vector3(1.0f, -1.0f, -1.0f), Vector3.UnitY));
            VertexList.Add(new PN_VertexAttribute(new Vector3(-1.0f, -1.0f, 1.0f), Vector3.UnitY));
            VertexList.Add(new PN_VertexAttribute(new Vector3(-1.0f, -1.0f, -1.0f), Vector3.UnitY));


            VertexList.Add(new PN_VertexAttribute(new Vector3(1.0f, -1.0f, -1.0f), Vector3.UnitY));
            VertexList.Add(new PN_VertexAttribute(new Vector3(1.0f, -1.0f, 1.0f), Vector3.UnitY));
            VertexList.Add(new PN_VertexAttribute(new Vector3(-1.0f, -1.0f, 1.0f), Vector3.UnitY));
        }

        public override List<RenderCommand> GetRenderCommands()
        {
            return null;
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

                defaultMaterial = ShaderManager.Get().GetMaterial<GBufferCubeTest>();

                bReadyToDraw = true;
            });
        }

        public override void Render()
        {   
            if (bReadyToDraw)
            {
                using (var dummy = new ScopedBind(defaultMaterial))
                {
                    var gbufferDraw = (GBufferCubeTest)defaultMaterial;

                    gbufferDraw.CameraTransform_View = CameraManager.Get().CurrentCameraView;
                    gbufferDraw.CameraTransform_Proj = CameraManager.Get().CurrentCameraProj;
                    gbufferDraw.ModelTransform_Model = this.LocalMatrix;

                    gbufferDraw.EquirectangularMap2D = cubemapTex;

                    drawable.DrawArrays(PrimitiveType.Triangles);

                }
            }
        }

        public override void JustDraw()
        {
            if (bReadyToDraw)
            {
                drawable.DrawArrays(PrimitiveType.Triangles);
            }
        }


        private List<PN_VertexAttribute> VertexList = new List<PN_VertexAttribute>();

        public DrawableBase<PN_VertexAttribute> Drawable => drawable;

        protected DrawableBase<PN_VertexAttribute> drawable = null;

        protected TextureBase cubemapTex = null;
    }
}
