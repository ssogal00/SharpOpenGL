using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpOpenGL.Font
{
    public class GlyphInfo
    {
        public GlyphInfo(char code, float atlasX, float atlasY, float advanceHorizontal, float adavanceVertical)
        {
            CharCode = code;
            AtlasX = atlasX;
            AtlasY = atlasY;
            AdvanceHorizontal = advanceHorizontal;
            AdvanceVertical = adavanceVertical;
        }

        public char CharCode = 'a';
        public float AtlasX = 0;
        public float AtlasY = 0;
        public float AdvanceHorizontal = 0;
        public float AdvanceVertical = 0;

    }
}
