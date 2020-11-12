using Core;
using Core.MaterialBase;
using Core.Texture;
using FreeTypeLibWrapper;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;



namespace SharpOpenGL.Font
{
    public class FontManager : Singleton<FontManager>
    {
        public void Initialize()
        {
            freeTypeLib = new FreeType();

            RenderingThread.Get().ExecuteImmediatelyIfRenderingThread
            (
            () =>
            {
                fontAtlas = new Texture2D();
                FontRenderMaterial = ShaderManager.Get().GetMaterial("FontRenderMaterial");
                FontBoxRenderMaterial = ShaderManager.Get().GetMaterial("FontBoxRenderMaterial");
                BuildFontTextureAtlas();
            }
            );
        }

        public long GetKerning(ulong previous, ulong current)
        {
            return freeTypeLib.GetKerning(previous, current);
        }

        private void BuildFontTextureAtlas()
        {
            var characters = Enumerable.Range(char.MinValue, 126).Select(c => (char)c).Where(c => !char.IsControl(c)).ToArray();
            var charString = new string (characters);
            bool bSuccess = freeTypeLib.Initialize("./Resources/Font/Test.ttf", 54, charString);
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

        public void RenderText(float x, float y, string text, bool bCache = false)
        {
            if (text.Length == 0)
            {
                return;
            }
            
            if (renderTextDicationary.ContainsKey(text) == false)
            {
                var newInstance = new TextInstance(text,x,y);
                renderTextDicationary.Add(text, newInstance);
            }

            renderTextDicationary[text].RenderWithBox(FontRenderMaterial, FontBoxRenderMaterial);
        }
        
        // 
        private Texture2D fontAtlas = null;
        private MaterialBase FontRenderMaterial = null;

        private MaterialBase FontBoxRenderMaterial = null;
        //

        public Dictionary<ulong, FreeTypeLibWrapper.GlyphInfo> GlyphDictionary = null;

        public int FontSize => fontSize;

        

        // texture atlas info
        private float textureDimension = 0;
        private float squareSize = 0;

        private int fontSize = 18;
        private int realTextureSize = 512;

        public float SquareSize => squareSize;
        public float TextureDimension => textureDimension;


        public Texture2D FontAtlas
        {
            get { return fontAtlas; }
        }

        Dictionary<string, TextInstance> renderTextDicationary = new Dictionary<string, TextInstance>();
        private FreeTypeLibWrapper.FreeType freeTypeLib = null;
    }
}
