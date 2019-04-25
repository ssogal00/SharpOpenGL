using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompiledMaterial.LUTGenerateMaterial;
using Core;
using Core.MaterialBase;
using Core.Texture;
using OpenTK.Graphics.OpenGL;
using SharpOpenGL.PostProcess;

namespace SharpOpenGL.Transform
{
    public class LookUpTable2D : PostProcessBase
    {
       
        public override void OnGLContextCreated(object sender, EventArgs e)
        {
            base.OnGLContextCreated(sender, e);

            material = ShaderManager.Get().GetMaterial<LUTGenerateMaterial>();
        }

        protected override void CreateDefaultRenderTarget()
        {
            // we need fixed render target
            Output = new RenderTarget(512, 512, 1, true);
            Output.Initialize();
        }

        public bool IsCompleted { get; set; } = false;

        public void Save()
        {
            if (Output != null)
            {
                var colorDataX = Output.ColorAttachment0.GetTexImageAsByte();
                FreeImageHelper.SaveAsBmp(ref colorDataX, 512, 512, "LUT.bmp");
            }
        }

        public override void Render()
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);

            Output.BindAndExecute(material, 
                () =>
                {
                    BlitToScreenSpace();
                    // once draw we completed
                    IsCompleted = true;
                }
            );
        }

        protected LUTGenerateMaterial material = null;
    }
}
