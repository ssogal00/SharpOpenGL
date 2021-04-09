using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using CompiledMaterial.FontRenderMaterial;
using CompiledMaterial.ScreenSpaceDraw;
using CompiledMaterial.SignedDistanceField;
using Core;
using Core.Texture;
using Engine;

namespace Engine
{
    public class FontRender : ScreenSpaceRender
    {
        public FontRender()
        : base()
        {
            mFontRenderMaterial = ShaderManager.Instance.GetMaterial<SignedDistanceField>();
            //mFontRenderMaterial = ShaderManager.Instance.GetMaterial<ScreenSpaceDraw>();
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

