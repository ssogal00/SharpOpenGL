using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompiledMaterial.HDAOMaterial;
using Core;
using Core.Texture;
using OpenTK;
using MathHelper = Core.MathHelper;

namespace SharpOpenGL.PostProcess
{
    public class HDAO : PostProcessBase
    {
        public override void OnGLContextCreated(object sender, EventArgs e)
        {
            base.OnGLContextCreated(sender, e);

            PostProcessMaterial = ShaderManager.Get().GetMaterial<HDAOMaterial>();
        }

        private OpenTK.Vector2 FocalLen = new Vector2(0);
        private OpenTK.Vector2 InvFocalLen = new Vector2(0);

        private OpenTK.Vector4 OcclusionRejectRadius = new Vector4(15.0f);
        private OpenTK.Vector4 OcclusionAcceptRadius = new Vector4(1.0f);

        public override void Render(TextureBase postionTex, TextureBase normalTex)
        {
            var hdaoMaterial = (HDAOMaterial)PostProcessMaterial;

            Output.BindAndExecute(hdaoMaterial, () =>
            {
                hdaoMaterial.PositionMap2D = postionTex;
                //hdaoMaterial.FarClipDistance = CameraManager.Get().CurrentCamera.Far;
                hdaoMaterial.NormalMap2D = normalTex;

                hdaoMaterial.NormalScale = -3.0f;
                hdaoMaterial.HDAOIntensity = 1.84f;
                hdaoMaterial.AcceptAngle = 1.58f;
                hdaoMaterial.RTSize = Output.RenderTargetSize;
                hdaoMaterial.HDAORejectRadius = OcclusionRejectRadius;
                hdaoMaterial.HDAOAcceptRadius = OcclusionAcceptRadius;

                 FocalLen.X = 1.0f / (float)(Math.Tanh((double)CameraManager.Get().CurrentCamera.FOV * 0.5) * (double)Output.RenderTargetHeight / (double)Output.RenderTargetWidth);
                 FocalLen.Y = 1.0f / (float) (Math.Tanh((double) CameraManager.Get().CurrentCamera.FOV * 0.5));
                 InvFocalLen.X = 1.0f / FocalLen.X;
                 InvFocalLen.Y = 1.0f / FocalLen.Y;

                 BlitToScreenSpace();
            });
        }
    }
}
