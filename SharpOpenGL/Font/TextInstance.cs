using System;
using System.Collections.Generic;
using Core;
using Core.Buffer;
using Core.MaterialBase;
using Core.Primitive;
using SixLabors.Fonts;
using SixLabors.Primitives;
using SharpOpenGL;
using OpenTK.Graphics.OpenGL;


namespace SharpOpenGL.Font
{
    public class TextInstance : IDisposable
    {
        public TextInstance(string textContent, float x, float y)
        {
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

            var glyphList = TextBuilder.GenerateGlyphsWithBox(TextContent, PointF.Empty, new RendererOptions(FontManager.Get().CurrentFont, 72) {ApplyKerning = true});
            int index = 0;

            foreach (var box in glyphList.boxes)
            {
                if (FontManager.GlyphDictionary.ContainsKey(TextContent[index]) == false)
                {
                    continue;
                }

                var v1 = new OpenTK.Vector3(originX + box.Bounds.Left, originY - box.Bounds.Top, 0);
                var v2 = new OpenTK.Vector3(originX + box.Bounds.Right, originY - box.Bounds.Top, 0);
                var v3 = new OpenTK.Vector3(originX + box.Bounds.Left, originY - box.Bounds.Bottom, 0);
                var v4 = new OpenTK.Vector3(originX + box.Bounds.Right, originY - box.Bounds.Bottom, 0);

                var left = FontManager.GlyphDictionary[TextContent[index]].Left;
                var top = FontManager.GlyphDictionary[TextContent[index]].Top;
                var textureWidth = FontManager.GlyphDictionary[TextContent[index]].Width;
                var textureHeight = FontManager.GlyphDictionary[TextContent[index]].Height;

                var texCoordX0 = FontManager.GlyphDictionary[TextContent[index]].AtlasX + left;
                var texCoordX1 = FontManager.GlyphDictionary[TextContent[index]].AtlasX + left + textureWidth;
                var texCoordY0 = 1 - (FontManager.GlyphDictionary[TextContent[index]].AtlasY + top);
                var texCoordY1 = 1 - (FontManager.GlyphDictionary[TextContent[index]].AtlasY + top + textureHeight);

                var texcoord1 = new OpenTK.Vector2(texCoordX0, texCoordY0);
                var texcoord2 = new OpenTK.Vector2(texCoordX1, texCoordY0);
                var texcoord3 = new OpenTK.Vector2(texCoordX0, texCoordY1);
                var texcoord4 = new OpenTK.Vector2(texCoordX1, texCoordY1);

                index++;

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

        private DynamicVertexBuffer<PT_VertexAttribute> vb = null;

        private List<PT_VertexAttribute> vertexList = new List<PT_VertexAttribute>();
    }
}
