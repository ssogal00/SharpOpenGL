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
    public class PBRSphere : Sphere
    {
        public PBRSphere()
           : base()
        {
            this.instanceCount = 36;
            this.Translation = new Vector3(-100, 0, 0);
            this.Scale = 2.0f;
            this.IsEditable = true;
        }
        public PBRSphere(float radius, int stackcount, int sectorcount, int instanceCount)
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

                defaultMaterial = ShaderManager.Get().GetMaterial<GBufferDraw.GBufferDraw>();

                if (normalTex == null)
                {
                    normalTex = TextureManager.Get().LoadTexture2D("./Resources/Imported/Texture/rustediron2_normal.imported");
                }

                if (diffuseTex == null)
                {
                    diffuseTex = TextureManager.Get().LoadTexture2D("./Resources/Imported/Texture/rustediron2_basecolor.imported");
                }

                if (roughTex == null)
                {
                    roughTex = TextureManager.Get().LoadTexture2D("./Resources/Imported/Texture/rustediron2_roughness.imported");
                }

                if (metalicTex == null)
                {
                    metalicTex = TextureManager.Get().LoadTexture2D("./Resources/Imported/Texture/rustediron2_metallic.imported");
                }

                bReadyToDraw = true;
            });
        }

        public void SetNormalTex(TextureBase normalTex)
        {
            this.normalTex = normalTex;
        }

        public void SetDiffuseTex(TextureBase diffuseTex)
        {
            this.diffuseTex = diffuseTex;
        }
        
        public void SetRoughnessTex(TextureBase roughnessTex)
        {
            this.roughTex = roughnessTex;
        }

        public void SetMetallicTex(TextureBase metallicTex)
        {
            this.metalicTex = metallicTex;
        }

        public override void Draw()
        {
            if (bReadyToDraw)
            {
                using (var dummy = new ScopedBind(defaultMaterial))
                {
                    var gbufferDraw = (GBufferDraw.GBufferDraw)defaultMaterial;

                    gbufferDraw.CameraTransform_View = CameraManager.Get().CurrentCameraView;
                    gbufferDraw.CameraTransform_Proj = CameraManager.Get().CurrentCameraProj;
                    gbufferDraw.ModelTransform_Model = this.LocalMatrix;

                    gbufferDraw.NormalMapExist = true;
                    gbufferDraw.MetalicExist = true;
                    gbufferDraw.RoughnessExist = true;
                    gbufferDraw.DiffuseMapExist = true;

                    gbufferDraw.NormalTex2D = normalTex;
                    gbufferDraw.DiffuseTex2D = diffuseTex;
                    gbufferDraw.RoughnessTex2D = roughTex;
                    gbufferDraw.MetalicTex2D = metalicTex;

                    drawable.DrawArrays(PrimitiveType.Triangles);
                }
            }
        }

        protected int instanceCount = 1;

        protected Vector3 Color = new Vector3(1, 1, 1);
    }
}
