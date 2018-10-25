using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

using System.Numerics;
using SixLabors.Fonts;
using SixLabors.Shapes;



namespace SharpOpenGL.Font
{
    public static class FontHelper
    {
        public static void TestRender()
        {
            FontCollection fonts = new FontCollection();
            using (var fs = new FileStream(@"./Resources//Font/OpenSans-Regular.ttf", FileMode.Open))
            {
                FontFamily fontFamily = fonts.Install(fs);

                var font = new SixLabors.Fonts.Font(fontFamily, 64);

                var characters = Enumerable.Range(char.MinValue, 126).Select(c => (char) c).Where(c => !char.IsControl(c))
                    .ToArray();

                int newResolution = 0;
                int newMargin = 0;

                GetCorrectResolution(64, characters.Length, out newResolution, out newMargin);

                var squareSize = (newResolution + newMargin);

                int numGlyphsPerRow = (int) Math.Ceiling(Math.Sqrt((float) characters.Length));

                int texSize = (int) (numGlyphsPerRow) * squareSize;

                int realTexSize = GetNextPowerOf2(texSize);

                Dictionary<char, GlyphInfo> glyphMap = new Dictionary<char, GlyphInfo>();
                
                using (Image<Rgba32> img = new Image<Rgba32>(realTexSize, realTexSize))
                {
                    img.Mutate(x => x.Fill(Rgba32.White));

                    for (int i = 0; i < characters.Length; ++i)
                    {
                        int row = i / numGlyphsPerRow;
                        int col = i % numGlyphsPerRow;

                        var renderOption = new RendererOptions(font, 72);
                        var glyph = TextBuilder.GenerateGlyphsWithBox(new string(new char[] {characters[i]}),
                                new SixLabors.Primitives.PointF(0f, 0f), renderOption);

                        var atlasX = col * squareSize;
                        var atlasY = row * squareSize;
                        var transform = Matrix3x2.Identity;
                        transform.Translation = new Vector2( atlasX, atlasY);

                        var bounds = glyph.Item1.Bounds;
                        
                        glyphMap.Add(characters[i], new GlyphInfo(characters[i], 
                            atlasX , atlasY, 
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
        }

        

        public static void GetCorrectResolution(int resolution , int glyphCount, out int newResolution, out int newMargin)
        {
            int glyphMargin = 0;

            while (resolution > 0)
            {
                glyphMargin = (int) (Math.Ceiling(resolution * 0.1));
                var squareSize = resolution + glyphMargin;
                var numGlyphsPerRow = (int) (Math.Ceiling(Math.Sqrt(glyphCount)));
                var texSize = numGlyphsPerRow * squareSize;
                int realTexSize = GetNextPowerOf2(texSize);
                if (realTexSize <= 1024)
                {
                    break;
                }

                resolution -= 5;
            }

            if (resolution > 0)
            {
                newResolution = resolution;
                newMargin = glyphMargin;
            }
            else
            {
                newResolution = 0;
                newMargin = 0;
            }
        }

        public static int GetNextPowerOf2(float n)
        {
            int returnValue = 1;
            while (returnValue < n)
            {
                returnValue <<= 1;
            }

            return returnValue;
        }

        public static void RenderText(SixLabors.Fonts.Font font, string text, int width, int height)
        {
            string path = System.IO.Path.GetInvalidFileNameChars().Aggregate(text, (x, c) => x.Replace($"{c}", "-"));
            string fullPath = System.IO.Path.GetFullPath(System.IO.Path.Combine("Output", System.IO.Path.Combine(path)));

            var renderOption = new RendererOptions(font, 72);
            (IPathCollection, IPathCollection, IPath) glyph = TextBuilder.GenerateGlyphsWithBox(text, new SixLabors.Primitives.PointF(0f, 0f), renderOption);
            int heightPowerOf2 = GetNextPowerOf2(glyph.Item2.Bounds.Height);
            int widthPowerOf2 = GetNextPowerOf2(glyph.Item2.Bounds.Width);


            using (Image<Rgba32> img = new Image<Rgba32>(widthPowerOf2, heightPowerOf2))
            {
                img.Mutate(x => x.Fill(Rgba32.White));
                img.Mutate(x => x.Fill(Rgba32.Black, glyph.Item1));

                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fullPath));

                using (FileStream fs = File.Create(fullPath + ".png"))
                {
                    img.SaveAsPng(fs);
                }
            }
        }

        public static void RenderText(RendererOptions font, string text)
        {
            GlyphBuilder builder = new GlyphBuilder();
            TextRenderer renderer = new TextRenderer(builder);
            SixLabors.Primitives.SizeF size = TextMeasurer.Measure(text, font);
            var bounds = TextMeasurer.MeasureBounds(text, font);
            foreach (var box in builder.Boxes)
            {
                var bound = box.Bounds;
            }
            renderer.RenderText(text, font);
            builder.Paths
                .SaveImage((int)size.Width + 20, (int)size.Height + 20, font.Font.Name, text + ".png");
        }
        public static void RenderText(FontFamily font, string text, float pointSize = 12)
        {
            RenderText(new RendererOptions(new SixLabors.Fonts.Font(font, pointSize), 96) { ApplyKerning = true, WrappingWidth = 340 }, text);
        }

        public static void SaveImage(this IEnumerable<IPath> shapes, int width, int height, params string[] path)
        {
            path = path.Select(p => System.IO.Path.GetInvalidFileNameChars().Aggregate(p, (x, c) => x.Replace($"{c}", "-"))).ToArray();
            string fullPath = System.IO.Path.GetFullPath(System.IO.Path.Combine("Output", System.IO.Path.Combine(path)));

            using (Image<Rgba32> img = new Image<Rgba32>(width, height))
            {
                img.Mutate(x => x.Fill(Rgba32.White));

                foreach (IPath s in shapes)
                {
                    // In ImageSharp.Drawing.Paths there is an extension method that takes in an IShape directly.
                    //img.Mutate(x => x.Fill(Rgba32.Black, s.Translate(new Vector2(0, 0))));
                    img.Mutate(x => x.Fill(Rgba32.Black, s));
                }
                // img.Draw(Color.LawnGreen, 1, shape);

                // Ensure directory exists
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fullPath));

                using (FileStream fs = File.Create(fullPath))
                {
                    img.SaveAsPng(fs);
                }
            }
        }

        public static void SaveImage(this IEnumerable<IPath> shapes, params string[] path)
        {
            IPath shape = new ComplexPolygon(shapes.ToArray());
            shape = shape.Translate(shape.Bounds.Location * -1) // touch top left
                    .Translate(new Vector2(10)); // move in from top left

            StringBuilder sb = new StringBuilder();
            IEnumerable<ISimplePath> converted = shape.Flatten();
            converted.Aggregate(sb, (s, p) =>
            {
                foreach (Vector2 point in p.Points)
                {
                    sb.Append(point.X);
                    sb.Append('x');
                    sb.Append(point.Y);
                    sb.Append(' ');
                }
                s.Append('\n');
                return s;
            });
            string str = sb.ToString();
            shape = new ComplexPolygon(converted.Select(x => new Polygon(new LinearLineSegment(x.Points))).ToArray());

            path = path.Select(p => System.IO.Path.GetInvalidFileNameChars().Aggregate(p, (x, c) => x.Replace($"{c}", "-"))).ToArray();
            string fullPath = System.IO.Path.GetFullPath(System.IO.Path.Combine("Output", System.IO.Path.Combine(path)));
            // pad even amount around shape
            int width = (int)(shape.Bounds.Left + shape.Bounds.Right);
            int height = (int)(shape.Bounds.Top + shape.Bounds.Bottom);
            if (width < 1)
            {
                width = 1;
            }
            if (height < 1)
            {
                height = 1;
            }
            using (Image<Rgba32> img = new Image<Rgba32>(width, height))
            {
                img.Mutate(x => x.Fill(Rgba32.DarkBlue));

                // In ImageSharp.Drawing.Paths there is an extension method that takes in an IShape directly.
                img.Mutate(x => x.Fill(Rgba32.HotPink, shape));
                // img.Draw(Color.LawnGreen, 1, shape);

                // Ensure directory exists
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fullPath));

                using (FileStream fs = File.Create(fullPath))
                {
                    img.SaveAsPng(fs);
                }
            }
        }
    }
}
