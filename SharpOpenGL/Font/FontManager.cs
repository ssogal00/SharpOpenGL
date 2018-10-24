using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace SharpOpenGL.Font
{
    public class FontManager : Singleton<FontManager>
    {


        protected void BuildFontTextureAtlas()
        {
            //
        }

        public void RenderText(float x, float y, string text)
        {
        }

        
        protected Dictionary<char, GlyphInfo> GlyphDictionary = new Dictionary<char, GlyphInfo>();
    }
}
