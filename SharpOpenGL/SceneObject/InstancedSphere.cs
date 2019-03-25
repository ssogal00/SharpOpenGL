using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Primitive;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace SharpOpenGL
{
    public class InstancedSphere : Sphere
    {
        public InstancedSphere()
            : base()
        {
            this.instanceCount = 36;
            this.Translation = new Vector3(-100,0,0);
            this.Scale = 2.0f;
        }
        public InstancedSphere(float radius, int stackcount, int sectorcount, int instanceCount)
            : base(radius, stackcount, sectorcount)
        {
            this.instanceCount = instanceCount;
        }

        public override void Initialize()
        {
            GenerateVertices();

            RenderingThread.Get().ExecuteImmediatelyIfRenderingThread(() =>
            {
                drawable = new DrawableBase<PNTT_VertexAttribute>();
                var vertexArray = VertexList.ToArray();
                drawable.SetupVertexData(ref vertexArray);

                VertexList.Clear();

                defaultMaterial = ShaderManager.Get().GetMaterial<GBufferInstanced.GBufferInstanced>();

                normalTex = TextureManager.Get().LoadTexture2D("./Resources/Imported/Texture/metalgrid4_normal-dx.imported");
                diffuseTex = TextureManager.Get().LoadTexture2D("./Resources/Imported/Texture/metalgrid4_basecolor.imported");
                roughTex = TextureManager.Get().LoadTexture2D("./Resources/Imported/Texture/metalgrid4_roughness.imported");
                metalicTex = TextureManager.Get()
                    .LoadTexture2D("./Resources/Imported/Texture/copper-rock1-metal.imported");

                bReadyToDraw = true;
            });
        }

        public override void Draw()
        {
            if (bReadyToDraw)
            {
                using (var dummy = new ScopedBind(defaultMaterial))
                {
                    var gbufferDraw = (GBufferInstanced.GBufferInstanced) defaultMaterial;

                    gbufferDraw.CameraTransform_View = CameraManager.Get().CurrentCameraView;
                    gbufferDraw.CameraTransform_Proj = CameraManager.Get().CurrentCameraProj;
                    gbufferDraw.ModelTransform_Model = this.LocalMatrix;

                    //gbufferDraw.NormalMapExist = true;
                    gbufferDraw.NormalMapExist = false;
                    gbufferDraw.MetalicExist = false;
                    gbufferDraw.RoughnessExist = false;
                    gbufferDraw.DiffuseMapExist = false;
                    
                    gbufferDraw.DiffuseOverride = Color;
                    gbufferDraw.NormalTex2D = normalTex;

                    gbufferDraw.MetallicCount = 6;
                    gbufferDraw.RoughnessCount = 6;

                    drawable.DrawArraysInstanced(PrimitiveType.Triangles, 6*6);
                }
            }
        }

        protected int instanceCount = 1;

        protected Vector3 Color = new Vector3(1,1,1);
    }
}
