using System;
using System.Collections.Generic;
using Core.MaterialBase;
using Core.Primitive;
using OpenTK;
using Core;
using Core.Buffer;
using Core.Texture;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Engine.Font;

namespace Engine
{
    public class ThreeDText : GameObject
    {
        public string TextContent = "";

        public ThreeDText(string textConent)
        {
            TextContent = textConent;
            GenerateVertices();
        }

        protected override void PrepareRenderingData()
        {
        }

        public override Matrix4 LocalMatrix
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

        public override void Render()
        {
            
        }
        // @ISceneObject interface

        protected void GenerateVertices()
        {
            
        }

        protected DynamicVertexBuffer<PT_VertexAttribute> vertexBuffer = null;
        protected List<PT_VertexAttribute> vertexList = new List<PT_VertexAttribute>();
        private int vertexCount = 0;
    }
}
