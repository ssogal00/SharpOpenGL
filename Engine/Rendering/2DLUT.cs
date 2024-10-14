using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using CompiledMaterial.LUTGenerateMaterial;
using Core;
using Core.MaterialBase;
using Core.Texture;
using Engine.PostProcess;
using OpenTK.Graphics.OpenGL;

namespace Engine.Transform
{
    public class LookUpTable2D : PostProcessBase
    {
        public LookUpTable2D()
        {
            material = ShaderManager.Instance.GetMaterial<LUTGenerateMaterial>();
        }

        protected override void CreateDefaultRenderTarget()
        {
            // we need fixed render target
            Output = new RenderTarget(512, 512, 1, true);
            Output.Initialize();
        }

        public bool IsCompleted { get; set; } = false;
        [SupportedOSPlatform("windows")]
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
