using System;
using System.Collections.Generic;
using Core;
using Core.Buffer;
using Core.MaterialBase;
using Core.Primitive;
using Core.Texture;
using SixLabors.Fonts;
using SixLabors.Primitives;
using SharpOpenGL;
using OpenTK.Graphics.OpenGL;


namespace SharpOpenGL.Font
{
    public class TextInstance : IDisposable
    {
        public TextInstance(string textContent, float x, float y, int fontSize)
        {
            this.fontSize = fontSize;
            TextContent = textContent;
            GenerateVertices(x,y);
        }

        public void Dispose()
        {
            if (vb != null)
            {
                vb.Dispose();
            }
        }

        public void Render(MaterialBase fontRenderMaterial)
        {
            using (var blend = new ScopedEnable(EnableCap.Blend))
            using (var dummy = new ScopedDisable(EnableCap.DepthTest))
            using (var sampler = new Sampler())
            using (var blendFunc = new ScopedBlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha))
            {
                fontRenderMaterial.BindAndExecute(vb, () =>
                {
                    
                    vb.Bind();
                    vb.BindVertexAttribute();
                    fontRenderMaterial.SetTexture("FontTexture", FontManager.Get().FontAtlas);
                    fontRenderMaterial.SetUniformVarData("ScreenSize",
                        new OpenTK.Vector2(OpenGLContext.Get().WindowWidth, OpenGLContext.Get().WindowHeight));
                    GL.DrawArrays(PrimitiveType.Quads, 0, vertexList.Count);
                });
            }
        }

        protected void GenerateVertices(float originX, float originY)
        {
            vertexList.Clear();

            var fXBasePosition = originX;

            var squareSize = FontManager.Get().SquareSize;
            var textureDimension = FontManager.Get().TextureDimension;

            foreach (var ch in TextContent)
            {
                if (FontManager.GlyphDictionary.ContainsKey(ch) == false)
                {
                    continue;
                }

                var fScale = 0.5f;
                var glyph = FontManager.GlyphDictionary[ch];

                fXBasePosition += (glyph.AdvanceHorizontal / (2.0f * 64.0f)) * fScale;
                float X = fXBasePosition - (glyph.Width / (2.0f * 64.0f)) * fScale;
                float Y = originY + ((glyph.HoriBearingY) / (2.0f * 64.0f)) * fScale;

                var halfSquare = (squareSize / 2.0f) * fScale;

                var v1 = new OpenTK.Vector3(-0.5f * halfSquare + X, 0.5f * halfSquare + Y, 0);
                var v2 = new OpenTK.Vector3( 0.5f * halfSquare + X, 0.5f * halfSquare + Y, 0);
                var v3 = new OpenTK.Vector3( 0.5f * halfSquare + X, -0.5f * halfSquare + Y, 0);
                var v4 = new OpenTK.Vector3(-0.5f * halfSquare + X, -0.5f * halfSquare + Y, 0);
                
                var texcoord1 = new OpenTK.Vector2(glyph.AtlasX, glyph.AtlasY);
                var texcoord2 = new OpenTK.Vector2(glyph.AtlasX + textureDimension, glyph.AtlasY);
                var texcoord3 = new OpenTK.Vector2(glyph.AtlasX + textureDimension, glyph.AtlasY + textureDimension);
                var texcoord4 = new OpenTK.Vector2(glyph.AtlasX, glyph.AtlasY + textureDimension);

                vertexList.Add(new PT_VertexAttribute(v1, texcoord1));
                vertexList.Add(new PT_VertexAttribute(v2, texcoord2));
                vertexList.Add(new PT_VertexAttribute(v4, texcoord4));
                vertexList.Add(new PT_VertexAttribute(v3, texcoord3));
            }

            vb = new DynamicVertexBuffer<PT_VertexAttribute>();
            var vertexArray = vertexList.ToArray();
            vb.BufferData<PT_VertexAttribute>(ref vertexArray);
        }

        public string TextContent = "";

        private bool bDrawBackground = false;

        private int fontSize = 36;

        private DynamicVertexBuffer<PT_VertexAttribute> vb = null;

        private List<PT_VertexAttribute> vertexList = new List<PT_VertexAttribute>();
    }
}
