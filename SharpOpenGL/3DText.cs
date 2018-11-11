using System;
using System.Collections.Generic;
using Core.MaterialBase;
using Core.Primitive;
using OpenTK;
using Core;
using Core.Buffer;
using Core.Texture;
using OpenTK.Graphics.OpenGL;
using SharpOpenGL.Font;

namespace SharpOpenGL
{
    public class ThreeDText : RenderResource, ISceneObject
    {
        public string TextContent = "";

        public ThreeDText(string textConent)
        {
            TextContent = textConent;
            GenerateVertices();
        }

        // @ISceneObject interface
        public Vector3 Translation { get; set; } = new Vector3(0,0,0);


        public float Scale { get; set; } = 1.0f;


        public float Yaw { get; set; } = 0.0f;


        public float Pitch { get; set; } = 0.0f;


        public float Roll { get; set; } = 0.0f;

        public OpenTK.Matrix4 ParentMatrix
        {
            get;
            set;
        } = Matrix4.Identity;

        public OpenTK.Matrix4 LocalMatrix
        {
            get
            {
                return Matrix4.CreateScale(Scale) * Matrix4.CreateRotationY(Yaw) * Matrix4.CreateRotationX(Pitch) *
                       Matrix4.CreateTranslation(Translation);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void Draw(MaterialBase material)
        {
            using (var blend = new ScopedEnable(EnableCap.Blend))
            using (var blendFunc = new ScopedBlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha))
            {
                material.BindAndExecute(vertexBuffer, () =>
                {
                    vertexBuffer.BindVertexAttribute();
                    material.SetUniformVarData("Model", LocalMatrix);
                    material.SetUniformVarData("View", CameraManager.Get().CurrentCameraView);
                    material.SetUniformVarData("Proj", CameraManager.Get().CurrentCameraProj);
                    material.SetTexture("FontTexture", FontManager.Get().FontAtlas);
                    GL.DrawArrays(PrimitiveType.Quads, 0, vertexCount);
                });
            }
        }
        // @ISceneObject interface

        protected void GenerateVertices()
        {
            vertexBuffer = new DynamicVertexBuffer<PT_VertexAttribute>();

            int index = 0;
            ulong previous = 0;

            var squareSize = FontManager.Get().SquareSize;
            var textureDimension = FontManager.Get().TextureDimension;

            float fXBasePosition = 0;
            float posY = 0;
            float fakeDepth = 0;

            foreach (var ch in TextContent)
            {
                if (FontManager.Get().GlyphDictionary.ContainsKey(ch) == false)
                {
                    continue;
                }

                long kerning = 0;
                if (index > 0)
                {
                    kerning = FontManager.Get().GetKerning(previous, ch);
                }

                var fScale = 1.0f;
                var glyph = FontManager.Get().GlyphDictionary[ch];

                fXBasePosition += ((glyph.AdvanceHorizontal) / (2.0f * 64.0f)) * fScale;
                float X = fXBasePosition - (glyph.Width / (2.0f * 64.0f)) * fScale;
                float Y = posY + ((glyph.HoriBearingY) / (2.0f * 64.0f)) * fScale;

                var halfSquare = (squareSize / 2.0f) * fScale;
                
                var leftX = -0.5f * halfSquare + X;
                var rightX = 0.5f * halfSquare + X;
                var topY = 0.5f * halfSquare + Y;
                var bottomY = -0.5f * halfSquare + Y;

                var charvertex1 = new OpenTK.Vector3(leftX, topY, 0);
                var charvertex2 = new OpenTK.Vector3(rightX, topY, 0);
                var charvertex3 = new OpenTK.Vector3(rightX, bottomY, 0);
                var charvertex4 = new OpenTK.Vector3(leftX, bottomY, 0);

                var texcoord1 = new OpenTK.Vector2(glyph.AtlasX, glyph.AtlasY);
                var texcoord2 = new OpenTK.Vector2(glyph.AtlasX + textureDimension, glyph.AtlasY);
                var texcoord3 = new OpenTK.Vector2(glyph.AtlasX + textureDimension, glyph.AtlasY + textureDimension);
                var texcoord4 = new OpenTK.Vector2(glyph.AtlasX, glyph.AtlasY + textureDimension);

                vertexList.Add(new PT_VertexAttribute(charvertex1, texcoord1));
                vertexList.Add(new PT_VertexAttribute(charvertex2, texcoord2));
                vertexList.Add(new PT_VertexAttribute(charvertex3, texcoord3));
                vertexList.Add(new PT_VertexAttribute(charvertex4, texcoord4));

                index++;
                previous = ch;
            }

            var vertexArray = vertexList.ToArray();
            vertexBuffer.BufferData<PT_VertexAttribute>(ref vertexArray);
            vertexCount = vertexArray.Length;
            vertexList.Clear();
        }

        protected DynamicVertexBuffer<PT_VertexAttribute> vertexBuffer = null;
        protected List<PT_VertexAttribute> vertexList = new List<PT_VertexAttribute>();
        private int vertexCount = 0;
    }
}
