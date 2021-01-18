using System;
using System.Collections.Generic;
using System.Text;
using Core.MaterialBase;
using Core.Texture;

namespace Engine
{
    public class PBRMaterial : MaterialBase
    {
        public void SetBaseColorTex(Texture2D texture)
        {
        }

        public TextureBase DiffuseTex
        {
            set => SetTexture(@"DiffuseTex", value);
        }

        public Texture2D NormalTex
        {
            set => SetTexture(@"NormalTex", value);
        }
    }
}
