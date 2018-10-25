using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpOpenGL.Font
{
    public class GlyphInfo
    {
        public GlyphInfo(char code, float atlasX, float atlasY, float left, float top, float width, float height)
        {
            CharCode = code;
            AtlasX = atlasX;
            AtlasY = atlasY;
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }

        public char CharCode = 'a';
        public float AtlasX = 0;
        public float AtlasY = 0;
        public float Left = 0;
        public float Top = 0;
        public float Width = 0;
        public float Height = 0;

    }
}
