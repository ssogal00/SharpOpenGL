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
using Core.MaterialBase;
using Core.Primitive;
using Core.Texture;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SharpOpenGL.Asset;
using SixLabors.Primitives;


namespace SharpOpenGL.Font
{
    public class FontManager : Singleton<FontManager>
    {
        public void Initialize()
        {
            FontAtlas = new Texture2D();
            VB = new DynamicVertexBuffer<PT_VertexAttribute>();
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

                textureDimension = (squareSize) / (float) realTextureSize;

                using (Image<Rgba32> img = new Image<Rgba32>(realTextureSize, realTextureSize))
                {
                    img.Mutate(x => x.Fill(Rgba32.White));

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
                            bounds.Left, bounds.Top, 
                            bounds.Width, bounds.Height));

                        IPathCollection newGlyph = glyph.Item1.Transform(transform);
                        img.Mutate(x => x.Fill(Rgba32.Black, newGlyph));
                    }

                    using (FileStream fontAtlas = File.Create("fontatlas.png"))
                    {
                        img.SaveAsPng(fontAtlas);
                    }
                }
            }
            
            FontAtlas.Load("fontatlas.png");
        }

        public void RenderText(float x, float y, string text)
        {
            VertexList.Clear();

            using (var dummy = new ScopedDisable(EnableCap.DepthTest))
            {
                var glyphList = TextBuilder.GenerateGlyphsWithBox(text, PointF.Empty, new RendererOptions(currentFont, 72));

                int index = 0;
                foreach (var box in glyphList.boxes)
                {
                    var v1 = new OpenTK.Vector3( box.Bounds.Left, box.Bounds.Top , 0);
                    var v2 = new OpenTK.Vector3(box.Bounds.Right, box.Bounds.Top, 0);
                    var v3 = new OpenTK.Vector3(box.Bounds.Left, box.Bounds.Bottom, 0);
                    var v4 = new OpenTK.Vector3(box.Bounds.Right, box.Bounds.Bottom, 0);

                    var texCoordX0 = GlyphDictionary[text[index]].AtlasX;
                    var texCoordX1 = GlyphDictionary[text[index]].AtlasX + box.Bounds.Width / (float) realTextureSize;
                    var texCoordY0 = GlyphDictionary[text[index]].AtlasY;
                    var texCoordY1 = GlyphDictionary[text[index]].AtlasY + box.Bounds.Height / (float)realTextureSize;

                    var texcoord1 = new OpenTK.Vector2(texCoordX0 , texCoordY0);
                    var texcoord2 = new OpenTK.Vector2(texCoordX1, texCoordY0);
                    var texcoord3 = new OpenTK.Vector2(texCoordX0, texCoordY1);
                    var texcoord4 = new OpenTK.Vector2(texCoordX1, texCoordY1);

                    index++;

                    VertexList.Add(new PT_VertexAttribute(v1, texcoord1));
                    VertexList.Add(new PT_VertexAttribute(v2, texcoord2));
                    VertexList.Add(new PT_VertexAttribute(v3, texcoord3));
                    VertexList.Add(new PT_VertexAttribute(v4, texcoord4));
                }

                VB.Bind();
                VB.BindVertexAttribute();
                var vertexArray = VertexList.ToArray();
                VB.BufferData<PT_VertexAttribute>(ref vertexArray);
                
                GL.DrawArrays(PrimitiveType.TriangleStrip, 0, VertexList.Count);
            }
        }

        // 
        private DynamicVertexBuffer<PT_VertexAttribute> VB = null;
        private List<PT_VertexAttribute> VertexList = new List<PT_VertexAttribute>();
        private Texture2D FontAtlas = null;
        private MaterialBase FontRenderMaterial = null;
        //
        
        protected Dictionary<char, GlyphInfo> GlyphDictionary = new Dictionary<char, GlyphInfo>();

        // texture atlas info
        private float textureDimension = 0;
        private int squareSize = 72;
        private int realTextureSize = 512;
        private bool bInitialized = false;

        // 
        private SixLabors.Fonts.Font currentFont = null;
    }
}
