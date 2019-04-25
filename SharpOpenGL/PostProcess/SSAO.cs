using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using CompiledMaterial.SSAOMaterial;
using Core;
using Core.Texture;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using MathHelper = Core.MathHelper;

namespace SharpOpenGL.PostProcess
{
    public class SSAO : PostProcessBase
    {
        public SSAO()
        : base()
        {
            this.bCreateCustomRenderTarget = true;
        }

        protected override void CreateCustomRenderTarget()
        {
            Output = new RenderTarget(1280,720,1, PixelInternalFormat.R16f, PixelFormat.Red, false, 0.5f);
            Output.Initialize();
        }

        public override void OnGLContextCreated(object sender, EventArgs e)
        {
            base.OnGLContextCreated(sender, e);

            PostProcessMaterial = ShaderManager.Get().GetMaterial<SSAOMaterial>();

            BuildKernel();
            BuildRandomRotationTexture();
        }

        public override void Render(TextureBase positionTex, TextureBase normalTex)
        {
            var ssaoMaterial = (SSAOMaterial) PostProcessMaterial;

            Output.BindAndExecute(ssaoMaterial, () =>
            {
                ssaoMaterial.NormalTex2D = normalTex;
                ssaoMaterial.PositionTex2D = positionTex;
                ssaoMaterial.ProjectionMatrix = CameraManager.Get().CurrentCameraProj;
                ssaoMaterial.RandTex2D = this.RandTexture;
                ssaoMaterial.SampleKernel = KernelArray;
                ssaoMaterial.Radius = 50;
                ssaoMaterial.ScreenSize = OutputRenderTarget.RenderTargetSize;

                BlitToScreenSpace();
            });
        }

        private void BuildRandomRotationTexture()
        {

            int size = 4;

            List<float> randomDirections = new List<float>(3 * size * size);

            for (int i = 0; i < size * size; ++i)
            {
                var v = MathHelper.UniformHemisphere();
                randomDirections.Add(v.X);
                randomDirections.Add(v.Y);
                randomDirections.Add(v.Z);
            }

            RandTexture = new Texture2D();
            RandTexture.Load(randomDirections.ToArray(), size, size, PixelInternalFormat.Rgb16f, PixelFormat.Rgb);
        }

        private void BuildKernel()
        {
            
            int KernelSize = 64;

            for (int i = 0; i < KernelSize; ++i)
            {
                Vector3 randDir = Core.MathHelper.UniformHemisphere();

                float scale = (float) (i * i) / (float) (KernelSize * KernelSize);
                randDir *= scale;
                KernelArray[i] = randDir;
            }
        }


        private Vector3[] KernelArray = new Vector3[64];

        private  Texture2D RandTexture = null;
    }
}
