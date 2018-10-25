using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

using System.Numerics;
using SixLabors.Fonts;
using SixLabors.Shapes;
using Core;
using Core.Buffer;
using Core.Primitive;
using Core.Texture;
using OpenTK.Graphics.OpenGL;

namespace SharpOpenGL.Font
{
    public class FontManager : Singleton<FontManager>
    {

        public void Initialize()
        {
            BuildFontTextureAtlas();

        }

        public void BuildFontTextureAtlas()
        {
            //
            FontCollection fonts = new FontCollection();
            using (var fs = new FileStream(@"./Resources//Font/OpenSans-Regular.ttf", FileMode.Open))
            {
                FontFamily fontFamily = fonts.Install(fs);
                var font = new SixLabors.Fonts.Font(fontFamily, 64);

                var characters = Enumerable.Range(char.MinValue, 126).Select(c => (char)c).Where(c => !char.IsControl(c)).ToArray();

                int newResolution = 0;
                int newMargin = 0;

                FontHelper.GetCorrectResolution(64, characters.Length, out newResolution, out newMargin);

                var squareSize = (newResolution + newMargin);

                int numGlyphsPerRow = (int)Math.Ceiling(Math.Sqrt((float)characters.Length));

                int texSize = (int)(numGlyphsPerRow) * squareSize;

                realTextureSizeX = realTextureSizeY = FontHelper.GetNextPowerOf2(texSize);

                using (Image<Rgba32> img = new Image<Rgba32>(realTextureSizeX, realTextureSizeY))
                {
                    img.Mutate(x => x.Fill(Rgba32.White));

                    for (int i = 0; i < characters.Length; ++i)
                    {
                        int row = i / numGlyphsPerRow;
                        int col = i % numGlyphsPerRow;

                        var renderOption = new RendererOptions(font, 72);
                        (IPathCollection, IPathCollection, IPath) glyph =
                            TextBuilder.GenerateGlyphsWithBox(new string(new char[] { characters[i] }),
                                new SixLabors.Primitives.PointF(0f, 0f), renderOption);

                        var atlasX = col * squareSize;
                        var atlasY = row * squareSize;
                        var transform = Matrix3x2.Identity;
                        transform.Translation = new Vector2(atlasX, atlasY);

                        var bounds = glyph.Item1.Bounds;

                        GlyphDictionary.Add(characters[i], new GlyphInfo(characters[i], 
                            atlasX, atlasY, 
                            bounds.Left, bounds.Top, 
                            bounds.Width, bounds.Height));

                        IPathCollection newGlyph = glyph.Item1.Transform(transform);
                        img.Mutate(x => x.Fill(Rgba32.Black, newGlyph));
                    }

                    using (FileStream fontAtlas = File.Create("fontatlas.bmp"))
                    {
                        img.SaveAsBmp(fontAtlas);
                    }
                }
            }

            FontAtlas = new Texture2D();
            FontAtlas.Load("fontatlas.bmp");
        }

        public void RenderText(float x, float y, string text)
        {
            using (var dummy = new ScopedDisable(EnableCap.DepthTest))
            {
                VB.Bind();
                VB.BindVertexAttribute();

                foreach (var ch in text)
                {
                    var glyphInfo = GlyphDictionary[ch];
                    var texCoordLeft = glyphInfo.Left / (float) realTextureSizeX;
                    var texCoordTop = glyphInfo.Top / (float) realTextureSizeY;
                    var texCoordRight = (glyphInfo.Left + glyphInfo.Width) / (float) realTextureSizeX;
                    var texCoordBottom = (glyphInfo.Left + glyphInfo.Width) / (float)realTextureSizeX;


                }
            }
        }

        // 
        private DynamicVertexBuffer<PT_VertexAttribute> VB = null;
        private Texture2D FontAtlas = null;
        //
        
        protected Dictionary<char, GlyphInfo> GlyphDictionary = new Dictionary<char, GlyphInfo>();


        private int realTextureSizeX = 512;
        private int realTextureSizeY = 512;
    }
}
