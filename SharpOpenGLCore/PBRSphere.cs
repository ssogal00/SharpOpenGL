using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CompiledMaterial.GBufferDraw;
using Core;
using Core.Primitive;
using Core.Texture;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace SharpOpenGL
{
    public class PBRSphere : Sphere
    {
        public PBRSphere()
           : base()
        {
            this.instanceCount = 36;
            this.Translation = new Vector3(-100, 0, 0);
            this.Color = new Vector3(1,1,1);
            this.Scale = 2.0f;
            this.IsEditable = true;
        }

        public PBRSphere(string diffuseTex, string normalTex, string roghnessTex, string metallicTex)
        : this()
        {
            this.diffuseTexPath = diffuseTex;
            this.normalTexPath = normalTex;
            this.roughnessTexPath = roghnessTex;
            this.metallicTexPath = metallicTex;
        }

        private Matrix4 PrevModel = Matrix4.Identity;

        public override void Tick(double elapsed)
        {
            Elapsed += elapsed;
            TranslationY = TranslationY + (float) Math.Sin((float)Elapsed) ;
        }

        private double Elapsed = 0;

        protected override void PrepareRenderingData()
        {
            drawable = new DrawableBase<PNTT_VertexAttribute>();
            var vertexArray = VertexList.ToArray();
            drawable.SetupVertexData(ref vertexArray);

            VertexList.Clear();

            defaultMaterial = ShaderManager.Get().GetMaterial<GBufferDraw>();

            if (normalTex == null && normalTexPath?.Length > 0)
            {
                normalTex = TextureManager.Get().LoadTexture2D(normalTexPath);
                if (normalTex != null)
                {
                    bNormalExist = true;
                }
            }

            if (diffuseTex == null && diffuseTexPath?.Length > 0)
            {
                diffuseTex = TextureManager.Get().LoadTexture2D(diffuseTexPath);
                if (diffuseTex != null)
                {
                    bDiffuseExist = true;
                }
            }

            if (roughTex == null && roughnessTexPath?.Length > 0)
            {
                roughTex = TextureManager.Get().LoadTexture2D(roughnessTexPath);

                if (roughTex != null)
                {
                    bRoughnessExist = true;
                }
            }

            if (metalicTex == null && metallicTexPath?.Length > 0)
            {
                metalicTex = TextureManager.Get().LoadTexture2D(metallicTexPath);
                if (metalicTex != null)
                {
                    bMetallicExist = true;
                }
            }

            bReadyToDraw = true;
        }

        public override void Initialize()
        {
            GenerateVertices();
        }

        public void SetNormalTex(string normalTex)
        {
            this.normalTexPath = normalTex;
        }

        public void SetDiffuseTex(string diffuseTex)
        {
            this.diffuseTexPath = diffuseTex;
        }

        public void SetRoughnessTex(string roughnessTex)
        {
            this.roughnessTexPath = roughnessTex;
        }


        public void SetMetallicTex(string metallicTex)
        {
            this.metallicTexPath = metallicTex;
        }

        public override void Render()
        {
            if (bReadyToDraw == false)
            {
                PrepareRenderingData();
            }

            if (bReadyToDraw)
            {
                using (var dummy = new ScopedBind(defaultMaterial))
                {
                    var gbufferDraw = (GBufferDraw)defaultMaterial;

                    gbufferDraw.LightChannel = (int) Light.LightChannel.StaticMeshChannel;
                    gbufferDraw.CameraTransform_View = CameraManager.Get().CurrentCameraView;
                    gbufferDraw.CameraTransform_Proj = CameraManager.Get().CurrentCameraProj;
                    gbufferDraw.PrevTransform_PrevProj = CameraManager.Get().PrevCameraProj;
                    gbufferDraw.PrevTransform_PrevView = CameraManager.Get().PrevCameraView;
                    gbufferDraw.PrevTransform_PrevModel = this.PrevModel;
                    gbufferDraw.ModelTransform_Model = this.LocalMatrix;

                    if (bMetallicExist)
                    {
                        gbufferDraw.MetalicExist = true;
                        gbufferDraw.MetalicTex2D = metalicTex;
                    }
                    else
                    {
                        gbufferDraw.MetalicExist = false;
                    }

                    if (bNormalExist)
                    {
                        gbufferDraw.NormalMapExist = true;
                        gbufferDraw.NormalTex2D = normalTex;
                    }
                    else
                    {
                        gbufferDraw.NormalMapExist = false;
                    }

                    if (bRoughnessExist)
                    {
                        gbufferDraw.RoughnessExist = true;
                        gbufferDraw.RoughnessTex2D = roughTex;
                    }

                    if (bDiffuseExist)
                    {
                        gbufferDraw.DiffuseMapExist = true;
                        gbufferDraw.DiffuseTex2D = diffuseTex;
                    }

                    drawable.DrawArrays(PrimitiveType.Triangles);

                    this.PrevModel = LocalMatrix;
                }
            }
        }

        protected int instanceCount = 1;


        private bool bRoughnessExist = false;
        private bool bMetallicExist = false;
        private bool bNormalExist = false;
        private bool bDiffuseExist = false;

        private string normalTexPath = null;
        private string roughnessTexPath = null;
        private string metallicTexPath = null;
        private string diffuseTexPath = null;
    }
}
