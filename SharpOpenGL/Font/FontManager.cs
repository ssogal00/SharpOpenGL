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
using OpenTK.Graphics.OpenGL;
using SharpOpenGL.Asset;
using SixLabors.Primitives;


namespace SharpOpenGL.Font
{
    public class FontManager : Singleton<FontManager>
    {
        public void Initialize()
        {
            fontAtlas = new Texture2D();
            FontRenderMaterial = AssetManager.LoadAssetSync<MaterialBase>("FontRenderMaterial");
            BuildFontTextureAtlas();
        }

        public void BuildFontTextureAtlas()
        {
            //
            FontCollection fonts = new FontCollection();
            using (var fs = new FileStream(@"./Resources//Font/OpenSans-Regular.ttf", FileMode.Open))
            {
                FontFamily fontFamily = fonts.Install(fs);
                currentFont = new SixLabors.Fonts.Font(fontFamily, 64);

                var characters = Enumerable.Range(char.MinValue, 126).Select(c => (char)c).Where(c => !char.IsControl(c)).ToArray();

                int newResolution = 0;
                int newMargin = 0;

                FontHelper.GetCorrectResolution(64, characters.Length, out newResolution, out newMargin);

                squareSize = (newResolution + newMargin);

                int numGlyphsPerRow = (int)Math.Ceiling(Math.Sqrt((float)characters.Length));

                int texSize = (int)(numGlyphsPerRow) * squareSize;

                realTextureSize = FontHelper.GetNextPowerOf2(texSize);

                byte[] textureData = new byte[realTextureSize * realTextureSize * 2];

                textureDimension = (squareSize) / (float) realTextureSize;

                using (Image<Rgba32> img = new Image<Rgba32>(realTextureSize, realTextureSize))
                {
                    img.Mutate(x => x.Fill(Rgba32.Black));

                    for (int i = 0; i < characters.Length; ++i)
                    {
                        int row = i / numGlyphsPerRow;
                        int col = i % numGlyphsPerRow;

                        var renderOption = new RendererOptions(currentFont, 72);
                        (IPathCollection, IPathCollection, IPath) glyph =
                            TextBuilder.GenerateGlyphsWithBox(new string(new char[] { characters[i] }),
                                new SixLabors.Primitives.PointF(0f, 0f), renderOption);

                        var atlasX = col * squareSize;
                        var atlasY = row * squareSize;
                        var transform = System.Numerics.Matrix3x2.Identity;
                        transform.Translation = new System.Numerics.Vector2(atlasX, atlasY);

                        var bounds = glyph.Item1.Bounds;

                        GlyphDictionary.Add(characters[i], new GlyphInfo(characters[i], 
                            atlasX / (float)realTextureSize, atlasY / (float) realTextureSize, 
                            bounds.Left / (float)realTextureSize, bounds.Top / (float)realTextureSize, 
                            bounds.Width/ (float)realTextureSize, bounds.Height/ (float)realTextureSize));

                        IPathCollection newGlyph = glyph.Item1.Transform(transform);
                        img.Mutate(x => x.Fill(Rgba32.White, newGlyph));
                    }

                    for (int y = 0; y < realTextureSize; ++y)
                    {
                        for (int x = 0; x < realTextureSize; ++x)
                        {
                            textureData[(y * realTextureSize + x)*2] = textureData[(y* realTextureSize + x)*2 + 1] = img[x, realTextureSize -1 - y].R;
                        }
                    }
                    
                    using (FileStream fontAtlas = File.Create("fontatlas.png"))
                    {
                        img.SaveAsBmp(fontAtlas);
                    }
                }

                fontAtlas.BindAtUnit(TextureUnit.Texture0);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.LuminanceAlpha, realTextureSize, realTextureSize,
                0, PixelFormat.LuminanceAlpha, PixelType.UnsignedByte, textureData);
            }
        }

        public void RenderText(float x, float y, string text)
        {
            
            if (renderTextDicationary.ContainsKey(text) == false)
            {
                var newInstance = new TextInstance(text,x,y);
                renderTextDicationary.Add(text, newInstance);
            }
            
            renderTextDicationary[text].Render(FontRenderMaterial);
        }

        
        // 
        private Texture2D fontAtlas = null;
        private MaterialBase FontRenderMaterial = null;
        //
        
        public static Dictionary<char, GlyphInfo> GlyphDictionary =  new Dictionary<char, GlyphInfo>();

        // texture atlas info
        private float textureDimension = 0;
        private int squareSize = 72;
        private int realTextureSize = 512;
        private bool bInitialized = false;

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
    }
}
