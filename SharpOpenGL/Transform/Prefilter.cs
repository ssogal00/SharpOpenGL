using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Texture;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using MathHelper = OpenTK.MathHelper;

namespace SharpOpenGL.Transform
{
    public class Prefilter : TransformBase
    {

        public override void OnGLContextCreated(object sender, EventArgs e)
        {
            base.OnGLContextCreated(sender, e);

            prefilterMaterial = ShaderManager.Get().GetMaterial<PrefilterMaterial.PrefilterMaterial>();

            cubemesh = new Cube();
            cubemesh.SetVisible(false);

            cubemapRenderTarget = new CubemapRenderTarget(SizeX, SizeY, true);

        }

        public override void Transform()
        {
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            using (var dummy = new ScopedBind(prefilterMaterial))
            {
                cubemapRenderTarget.BindForRendering();
                // 
                prefilterMaterial.Roughness = 0;
                prefilterMaterial.Projection = CaptureProjection;
                prefilterMaterial.EnvironmentMap2D = environmentMap;
                
                cubemapRenderTarget.BindFaceForRendering(TextureTarget.TextureCubeMapPositiveX);
                prefilterMaterial.View = CaptureViews[0];
                cubemesh.JustDraw();

                cubemapRenderTarget.BindFaceForRendering(TextureTarget.TextureCubeMapNegativeX);
                prefilterMaterial.View = CaptureViews[1];
                cubemesh.JustDraw();

                cubemapRenderTarget.BindFaceForRendering(TextureTarget.TextureCubeMapPositiveY);
                prefilterMaterial.View = CaptureViews[2];
                cubemesh.JustDraw();

                cubemapRenderTarget.BindFaceForRendering(TextureTarget.TextureCubeMapNegativeY);
                prefilterMaterial.View = CaptureViews[3];
                cubemesh.JustDraw();

                cubemapRenderTarget.BindFaceForRendering(TextureTarget.TextureCubeMapPositiveZ);
                prefilterMaterial.View = CaptureViews[4];
                cubemesh.JustDraw();

                cubemapRenderTarget.BindFaceForRendering(TextureTarget.TextureCubeMapNegativeZ);
                prefilterMaterial.View = CaptureViews[5];
                cubemesh.JustDraw();

                cubemapRenderTarget.UnbindForRendering();
            }
        }

        public void SetEnvMap(TextureBase envmap)
        {
            this.environmentMap = envmap;
        }

        public void Save()
        {
            /*var colorDataX = PositiveX.ColorAttachment0.GetTexImageAsByte();
            FreeImageHelper.SaveAsBmp(ref colorDataX, 512, 512, "PrefilterPosX.bmp");

            var colorData = NegativeX.ColorAttachment0.GetTexImageAsByte();
            FreeImageHelper.SaveAsBmp(ref colorData, 512, 512, "PrefilterNegX.bmp");*/
        }
       
        private OpenTK.Matrix4 CaptureProjection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(90), 1.0f, 0.1f, 10.0f);

        private OpenTK.Matrix4[] CaptureViews = new Matrix4[]
        {
            Matrix4.LookAt(new Vector3(0,0,0), Vector3.UnitX, -Vector3.UnitY ), // positive X
            Matrix4.LookAt(new Vector3(0,0,0), -Vector3.UnitX, -Vector3.UnitY ),// negative X
            Matrix4.LookAt(new Vector3(0,0,0), Vector3.UnitY, Vector3.UnitZ), // positive Y
            Matrix4.LookAt(new Vector3(0,0,0), -Vector3.UnitY, -Vector3.UnitZ ),// negative Y
            Matrix4.LookAt(new Vector3(0,0,0), Vector3.UnitZ, -Vector3.UnitY ),// positive Z
            Matrix4.LookAt(new Vector3(0,0,0), -Vector3.UnitZ, -Vector3.UnitY) // negative Z
        };

        private static readonly int SizeX = 1024;
        private static readonly int SizeY = 1024;

        private PrefilterMaterial.PrefilterMaterial prefilterMaterial = null;

        private Cube cubemesh = null;

        public CubemapRenderTarget ResultCubemap => cubemapRenderTarget;

        private CubemapRenderTarget cubemapRenderTarget = null;

        private TextureBase environmentMap = null;
    }

}
