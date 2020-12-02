using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompiledMaterial.PrefilterMaterial;
using Core;
using Core.Texture;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using MathHelper = OpenTK.Mathematics.MathHelper;

namespace SharpOpenGL.Transform
{
    public class Prefilter : TransformBase
    {
        public Prefilter()
        {
            prefilterMaterial = ShaderManager.Get().GetMaterial<PrefilterMaterial>();

            cubemesh = new Cube();
            cubemesh.SetVisible(false);

            cubemapRenderTarget = new CubemapRenderTarget(SizeX, SizeY, true);
        }

        public override void Dispose()
        {
            base.Dispose();
            cubemapRenderTarget.Dispose();
        }

        public override void Transform()
        {
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            using (var dummy = new ScopedBind(prefilterMaterial))
            {
                prefilterMaterial.EnvironmentMap2D = environmentMap;
                prefilterMaterial.Projection = CaptureProjection;

                int maxMipLevels = 5;

                cubemapRenderTarget.BindForRendering();

                for (int miplevel = 0; miplevel < maxMipLevels; miplevel++)
                {   
                    int mipWidth = (int) (SizeX * Math.Pow(0.5, (double)miplevel));
                    int mipHeight = (int) (SizeY * Math.Pow(0.5, (double)miplevel));

                    //prefilterMaterial.Roughness = (float)miplevel / (float)(maxMipLevels - 1);
                    prefilterMaterial.Roughness = 1.0f;

                    cubemapRenderTarget.BindFaceForRendering(TextureTarget.TextureCubeMapPositiveX, miplevel);
                    prefilterMaterial.View = CaptureViews[0];
                    cubemesh.JustDraw();

                    cubemapRenderTarget.BindFaceForRendering(TextureTarget.TextureCubeMapNegativeX, miplevel);
                    prefilterMaterial.View = CaptureViews[1];
                    cubemesh.JustDraw();

                    cubemapRenderTarget.BindFaceForRendering(TextureTarget.TextureCubeMapPositiveY, miplevel);
                    prefilterMaterial.View = CaptureViews[2];
                    cubemesh.JustDraw();

                    cubemapRenderTarget.BindFaceForRendering(TextureTarget.TextureCubeMapNegativeY, miplevel);
                    prefilterMaterial.View = CaptureViews[3];
                    cubemesh.JustDraw();

                    cubemapRenderTarget.BindFaceForRendering(TextureTarget.TextureCubeMapPositiveZ, miplevel);
                    prefilterMaterial.View = CaptureViews[4];
                    cubemesh.JustDraw();

                    cubemapRenderTarget.BindFaceForRendering(TextureTarget.TextureCubeMapNegativeZ, miplevel);
                    prefilterMaterial.View = CaptureViews[5];
                    cubemesh.JustDraw();
                }

                cubemapRenderTarget.UnbindForRendering();
            }
        }

        public void SetEnvMap(TextureBase envmap)
        {
            this.environmentMap = envmap;
        }

        public void Save()
        {
            var colorDataX = cubemapRenderTarget.GetCubemapTexImageAsByte(TextureTarget.TextureCubeMapPositiveX, 0);
            FreeImageHelper.SaveAsBmp(ref colorDataX, cubemapRenderTarget.Width, cubemapRenderTarget.Height, "PrefilterPosX.bmp");

            var colorDataNegX = cubemapRenderTarget.GetCubemapTexImageAsByte(TextureTarget.TextureCubeMapNegativeX, 0);
            FreeImageHelper.SaveAsBmp(ref colorDataNegX, cubemapRenderTarget.Width, cubemapRenderTarget.Height, "PrefilterNegPosX.bmp");

            var colorDataZ = cubemapRenderTarget.GetCubemapTexImageAsByte(TextureTarget.TextureCubeMapPositiveZ, 0);
            FreeImageHelper.SaveAsBmp(ref colorDataZ, cubemapRenderTarget.Width, cubemapRenderTarget.Height, "PrefilterPosZ.bmp");

            var colorDataNegZ = cubemapRenderTarget.GetCubemapTexImageAsByte(TextureTarget.TextureCubeMapNegativeZ, 0);
            FreeImageHelper.SaveAsBmp(ref colorDataNegZ, cubemapRenderTarget.Width, cubemapRenderTarget.Height, "PrefilterNegPosZ.bmp");
        }
       
        private Matrix4 CaptureProjection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(90), 1.0f, 0.1f, 10.0f);

        private Matrix4[] CaptureViews = new Matrix4[]
        {
            Matrix4.LookAt(new Vector3(0,0,0), Vector3.UnitX, -Vector3.UnitY ), // positive X
            Matrix4.LookAt(new Vector3(0,0,0), -Vector3.UnitX, -Vector3.UnitY ),// negative X
            Matrix4.LookAt(new Vector3(0,0,0), Vector3.UnitY, Vector3.UnitZ), // positive Y
            Matrix4.LookAt(new Vector3(0,0,0), -Vector3.UnitY, -Vector3.UnitZ ),// negative Y
            Matrix4.LookAt(new Vector3(0,0,0), Vector3.UnitZ, -Vector3.UnitY ),// positive Z
            Matrix4.LookAt(new Vector3(0,0,0), -Vector3.UnitZ, -Vector3.UnitY) // negative Z
        };

        private static readonly int SizeX = 512;
        private static readonly int SizeY = 512;

        private PrefilterMaterial prefilterMaterial = null;

        private Cube cubemesh = null;

        public CubemapRenderTarget ResultCubemap => cubemapRenderTarget;

        private CubemapRenderTarget cubemapRenderTarget = null;

        private TextureBase environmentMap = null;
    }

}
