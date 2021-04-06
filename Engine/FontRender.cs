using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using CompiledMaterial.FontRenderMaterial;
using CompiledMaterial.ScreenSpaceDraw;
using CompiledMaterial.SignedDistanceField;
using Core;
using Core.Texture;
using SharpOpenGL;

namespace SharpOpenGLCore
{
    public class FontRender : ScreenSpaceRender
    {
        public FontRender()
        : base()
        {
            mFontRenderMaterial = ShaderManager.Get().GetMaterial<SignedDistanceField>();
            //mFontRenderMaterial = ShaderManager.Get().GetMaterial<ScreenSpaceDraw>();
            Debug.Assert(mFontRenderMaterial != null);
        }

        public override void Render(TextureBase input0)
        {
            mFontRenderMaterial.BindAndExecute(() =>
            {
                //mFontRenderMaterial.ColorTex2D = input0;
                mFontRenderMaterial.FontTexture2D = input0;
                BlitToScreenSpace();
            });
        }

        protected SignedDistanceField mFontRenderMaterial;
        //protected ScreenSpaceDraw mFontRenderMaterial;
    }
}

