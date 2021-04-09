using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompiledMaterial.CubemapConvolution;
using Core;
using Core.MaterialBase;
using Core.Texture;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using MathHelper = OpenTK.Mathematics.MathHelper;

namespace Engine.Transform
{
    public class CubemapConvolutionTransform : TransformBase
    {
        public CubemapConvolutionTransform()
        {
            Initialize();
        }

        public override void Initialize()
        {
            material = ShaderManager.Instance.GetMaterial<CubemapConvolution>();

            //
            cubeMesh = new Cube();
            cubeMesh.SetVisible(false);

            PositiveX.Initialize();
            PositiveY.Initialize();
            PositiveZ.Initialize();
            NegativeX.Initialize();
            NegativeY.Initialize();
            NegativeZ.Initialize();
        }

        public void SetSourceCubemap(CubemapTexture sourceCubemap)
        {
            this.sourceCubemap = sourceCubemap;
        }

        public void Save()
        {
            if (bSaved == false)
            {
                if (PositiveX != null && PositiveY != null && PositiveZ != null)
                {
                    var colorDataX = PositiveX.ColorAttachment0.GetTexImageAsByte();
                    FreeImageHelper.SaveAsBmp(ref colorDataX, SizeX, SizeY, "ConvolutionPositiveX.bmp");

                    var colorDataY = PositiveY.ColorAttachment0.GetTexImageAsByte();
                    FreeImageHelper.SaveAsBmp(ref colorDataY, SizeX, SizeY, "ConvolutionPositiveY.bmp");

                    var colorDataZ = PositiveZ.ColorAttachment0.GetTexImageAsByte();
                    FreeImageHelper.SaveAsBmp(ref colorDataZ, SizeX, SizeY, "ConvolutionPositiveZ.bmp");
                }

                if (NegativeX != null && NegativeY != null && NegativeZ != null)
                {
                    var colorDataX = NegativeX.ColorAttachment0.GetTexImageAsByte();
                    FreeImageHelper.SaveAsBmp(ref colorDataX, SizeX, SizeY, "ConvolutionNegativeX.bmp");

                    var colorDataY = NegativeY.ColorAttachment0.GetTexImageAsByte();
                    FreeImageHelper.SaveAsBmp(ref colorDataY, SizeX, SizeY, "ConvolutionNegativeY.bmp");

                    var colorDataZ = NegativeZ.ColorAttachment0.GetTexImageAsByte();
                    FreeImageHelper.SaveAsBmp(ref colorDataZ, SizeX, SizeY, "ConvolutionNegativeZ.bmp");
                }

                bSaved = true;
            }
        }

        public override void Transform()
        {
            if (sourceCubemap == null)
            {
                return;
            }

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            var cubemapConvolution = (CubemapConvolution) material;

            using (var mtl = new ScopedBind(cubemapConvolution))
            {
                //
                cubemapConvolution.EnvironmentMap2D = sourceCubemap;
                cubemapConvolution.Projection = CaptureProjection;

                using (var dummy = new ScopedBind(PositiveX))
                {
                    cubemapConvolution.View = CaptureViews[0];
                    cubeMesh.JustDraw();
                }

                using (var dummy = new ScopedBind(NegativeX))
                {
                    cubemapConvolution.View = CaptureViews[1];
                    cubeMesh.JustDraw();
                }

                using (var dummy = new ScopedBind(PositiveY))
                {
                    cubemapConvolution.View = CaptureViews[2];
                    cubeMesh.JustDraw();
                }

                using (var dummy = new ScopedBind(NegativeY))
                {
                    cubemapConvolution.View = CaptureViews[3];
                    cubeMesh.JustDraw();
                }

                using (var dummy = new ScopedBind(PositiveZ))
                {
                    cubemapConvolution.View = CaptureViews[4];
                    cubeMesh.JustDraw();
                }

                using (var dummy = new ScopedBind(NegativeZ))
                {
                    cubemapConvolution.View = CaptureViews[5];
                    cubeMesh.JustDraw();
                }

                resultCubemap = new CubemapTexture();
                resultCubemap.LoadFromTexture(
                    PositiveX.ColorAttachment0, NegativeX.ColorAttachment0,
                    PositiveY.ColorAttachment0, NegativeY.ColorAttachment0,
                    PositiveZ.ColorAttachment0, NegativeZ.ColorAttachment0
                    );

                IsTransformCompleted = true;
            }
        }


        private RenderTarget PositiveX = new RenderTarget(SizeX, SizeY, 1, true);
        private RenderTarget NegativeX = new RenderTarget(SizeX, SizeY, 1, true);
        private RenderTarget PositiveY = new RenderTarget(SizeX, SizeY, 1, true);
        private RenderTarget NegativeY = new RenderTarget(SizeX, SizeY, 1, true);
        private RenderTarget PositiveZ = new RenderTarget(SizeX, SizeY, 1, true);
        private RenderTarget NegativeZ = new RenderTarget(SizeX, SizeY, 1, true);

        private static readonly int SizeX = 256;
        private static readonly int SizeY = 256;


        private bool bSaved = false;

        public CubemapTexture ResultCubemap => resultCubemap;
        private CubemapTexture resultCubemap = null;

        private CubemapTexture sourceCubemap = null;


        private Matrix4 CaptureProjection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(90), 1.0f, 0.1f, 10.0f);

        private Matrix4[] CaptureViews = new Matrix4[]
        {
            Matrix4.LookAt(new Vector3(0,0,0), Vector3.UnitX, -Vector3.UnitY ), // positive X
            Matrix4.LookAt(new Vector3(0,0,0), -Vector3.UnitX, -Vector3.UnitY ),// negative X
            Matrix4.LookAt(new Vector3(0,0,0), Vector3.UnitY, Vector3.UnitZ), // positive Y
            Matrix4.LookAt(new Vector3(0,0,0), -Vector3.UnitY, -Vector3.UnitZ ),// negative Y
            Matrix4.LookAt(new Vector3(0,0,0), Vector3.UnitZ, -Vector3.UnitY ),// positive Z
            Matrix4.LookAt(new Vector3(0,0,0), -Vector3.UnitZ, -Vector3.UnitY),// negative Z
        };

        protected Cube cubeMesh = null;

        protected MaterialBase material = null;
    }
}
