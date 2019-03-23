using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.CustomAttribute;
using Core.Texture;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SharpOpenGL.PostProcess;
using MathHelper = OpenTK.MathHelper;

namespace SharpOpenGL
{
    public class EquirectangleToCubemap : PostProcessBase
    {
        
        public override void Initialize()
        {
            base.Initialize();

        }

        public override void OnGLContextCreated(object sender, EventArgs e)
        {
            base.OnGLContextCreated(sender, e);

            PostProcessMaterial = ShaderManager.Get().GetMaterial<EquirectangleToCube.EquirectangleToCube>();

            // create hdr texture
            var hdr = new HDRTexture();
            hdr.Load("./Resources/Texture/HDR/Alexs_Apt_2k.hdr");
            cubemapTex = hdr;

            //
            cubeMesh = new Cube();
            cubeMesh.SetVisible(false);
        }

        public void Save()
        {
            if (bSaved == false)
            {
                if (PositiveX != null && PositiveY != null && PositiveZ != null)
                {
                    var colorDataX = PositiveX.ColorAttachment0.GetTexImageAsByte();
                    FreeImageHelper.SaveAsBmp(ref colorDataX, SizeX, SizeY, "PositiveX.bmp");

                    var colorDataY = PositiveY.ColorAttachment0.GetTexImageAsByte();
                    FreeImageHelper.SaveAsBmp(ref colorDataY, SizeX, SizeY, "PositiveY.bmp");

                    var colorDataZ = PositiveZ.ColorAttachment0.GetTexImageAsByte();
                    FreeImageHelper.SaveAsBmp(ref colorDataZ, SizeX, SizeY, "PositiveZ.bmp");
                }

                if (NegativeX != null && NegativeY != null && NegativeZ != null)
                {
                    var colorDataX = NegativeX.ColorAttachment0.GetTexImageAsByte();
                    FreeImageHelper.SaveAsBmp(ref colorDataX, SizeX, SizeY, "NegativeX.bmp");

                    var colorDataY = NegativeY.ColorAttachment0.GetTexImageAsByte();
                    FreeImageHelper.SaveAsBmp(ref colorDataY, SizeX, SizeY, "NegativeY.bmp");

                    var colorDataZ = NegativeZ.ColorAttachment0.GetTexImageAsByte();
                    FreeImageHelper.SaveAsBmp(ref colorDataZ, SizeX, SizeY, "NegativeZ.bmp");
                }

                bSaved = true;
            }
        }

        public bool IsSaveCompleted => bSaved;

        private bool bSaved = false;

        public override void Render()
        {
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            var equirectangleToCube = (EquirectangleToCube.EquirectangleToCube) PostProcessMaterial;

            using (var mtl =  new ScopedBind(equirectangleToCube))
            {
                //
                equirectangleToCube.EquirectangularMap2D = cubemapTex;
                equirectangleToCube.Projection = CaptureProjection;

                using (var dummy = new ScopedBind(PositiveX))
                {
                    equirectangleToCube.View = CaptureViews[0];
                    cubeMesh.JustDraw();
                }

                using (var dummy = new ScopedBind(NegativeX))
                {
                    equirectangleToCube.View = CaptureViews[1];
                    cubeMesh.JustDraw();
                }

                using (var dummy = new ScopedBind(PositiveY))
                {
                    equirectangleToCube.View = CaptureViews[2];
                    cubeMesh.JustDraw();
                }

                using (var dummy = new ScopedBind(NegativeY))
                {
                    equirectangleToCube.View = CaptureViews[3];
                    cubeMesh.JustDraw();
                }

                using (var dummy = new ScopedBind(PositiveZ))
                {
                    equirectangleToCube.View = CaptureViews[4];
                    cubeMesh.JustDraw();
                }

                using (var dummy = new ScopedBind(NegativeZ))
                {
                    equirectangleToCube.View = CaptureViews[5];
                    cubeMesh.JustDraw();
                }

                
            }
        }


        private RenderTarget PositiveX = new RenderTarget(512, 512, 1 ,true);
        private RenderTarget NegativeX = new RenderTarget(512, 512, 1, true);
        private RenderTarget PositiveY = new RenderTarget(512, 512, 1, true);
        private RenderTarget NegativeY = new RenderTarget(512, 512, 1, true);
        private RenderTarget PositiveZ = new RenderTarget(512, 512, 1, true);
        private RenderTarget NegativeZ = new RenderTarget(512, 512, 1, true);

        private readonly int SizeX = 512;
        private readonly int SizeY = 512;

        
        private OpenTK.Matrix4 CaptureProjection = Matrix4.CreatePerspectiveFieldOfView( MathHelper.DegreesToRadians(90), 1.0f, 0.1f, 10.0f);

        private OpenTK.Matrix4[] CaptureViews = new Matrix4[]
        {
            Matrix4.LookAt(new Vector3(0,0,0), Vector3.UnitX, -Vector3.UnitY ), // positive X
            Matrix4.LookAt(new Vector3(0,0,0), -Vector3.UnitX, -Vector3.UnitY ),// negative X
            Matrix4.LookAt(new Vector3(0,0,0), Vector3.UnitY, Vector3.UnitZ), // positive Y
            Matrix4.LookAt(new Vector3(0,0,0), -Vector3.UnitY, -Vector3.UnitZ ),// negative Y
            Matrix4.LookAt(new Vector3(0,0,0), Vector3.UnitZ, -Vector3.UnitY ),// positive Z
            Matrix4.LookAt(new Vector3(0,0,0), -Vector3.UnitZ, -Vector3.UnitY),// negative Z
        };

        protected TextureBase cubemapTex = null;

        protected Cube cubeMesh = null;
    }
}
