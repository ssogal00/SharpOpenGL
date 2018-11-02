using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Fonts;
using SixLabors.Shapes;
using Core;
using Core.Buffer;
using Core.MaterialBase;
using Core.Primitive;
using Core.Texture;
using FreeTypeLibWrapper;
using OpenTK.Graphics.OpenGL;
using SharpOpenGL.Asset;
using SixLabors.Primitives;
using System.Diagnostics;



namespace SharpOpenGL.Font
{
    public class FontManager : Singleton<FontManager>
    {
        public void Initialize()
        {
            fontAtlas = new Texture2D();
            FontRenderMaterial = AssetManager.LoadAssetSync<MaterialBase>("FontRenderMaterial");
            freeTypeLib = new FreeType();
            BuildFontTextureAtlas();
        }

        public void BuildFontTextureAtlas()
        {
            bool bSuccess = freeTypeLib.Initialize("./Resources/Font/test.ttf");
            if (bSuccess)
            {
                GlyphDictionary = freeTypeLib.GetGlyphInfoDictionary();
                realTextureSize = freeTypeLib.GetRealTextureSize();
                textureDimension = freeTypeLib.GetTextureDimension();
                squareSize = freeTypeLib.GetSquareSize();
            }
            else
            {
                Debug.Assert(false);
            }
            var textureData = freeTypeLib.GetTextureData();
            
            fontAtlas.Load(textureData.ToArray(), realTextureSize, realTextureSize, PixelInternalFormat.LuminanceAlpha, PixelFormat.LuminanceAlpha);
            
        }

        public void RenderText(float x, float y, string text, int fontSize)
        {
            if (renderTextDicationary.ContainsKey(text) == false)
            {
                var newInstance = new TextInstance(text,x,y,fontSize);
                renderTextDicationary.Add(text, newInstance);
            }
            
            renderTextDicationary[text].Render(FontRenderMaterial);
        }

        
        // 
        private Texture2D fontAtlas = null;
        private MaterialBase FontRenderMaterial = null;
        //

        public static Dictionary<ulong, FreeTypeLibWrapper.GlyphInfo> GlyphDictionary = null;

        public int FontSize => fontSize;

        public int DPI => dpi;

        // texture atlas info
        private float textureDimension = 0;
        private float squareSize = 0;
        private int dpi = 72;
        private int fontSize = 18;
        private int realTextureSize = 512;
        private bool bInitialized = false;

        public float SquareSize => squareSize;
        public float TextureDimension => textureDimension;

        // 
        private SixLabors.Fonts.Font currentFont = null;

        public SixLabors.Fonts.Font CurrentFont
        {
            get { return currentFont; }
        }

        public Texture2D FontAtlas
        {
            get { return fontAtlas; }
        }

        Dictionary<string, TextInstance> renderTextDicationary = new Dictionary<string, TextInstance>();
        private FreeTypeLibWrapper.FreeType freeTypeLib = null;
    }
}
