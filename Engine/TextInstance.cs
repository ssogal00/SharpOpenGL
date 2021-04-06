using Core;
using Core.Buffer;
using Core.MaterialBase;
using Core.Primitive;
using Core.Texture;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace SharpOpenGL.Font
{
    public class TextInstance : IDisposable
    {
        public TextInstance(string textContent, float x, float y)
        {
            TextContent = textContent;
            originX = x;
            originY = y;
            GenerateVertices(x,y);
        }

        public void SetTextContent(string newTextContent)
        {
            if (TextContent != newTextContent)
            {
                TextContent = newTextContent;
                GenerateVertices(originX, originY);
            }
        }

        public void Dispose()
        {
            if (vb != null)
            {
                vb.Dispose();
            }
        }

        public void RenderWithBox(MaterialBase fontRenderMaterial, MaterialBase fontBoxRenderMaterial)
        {
            RenderBox(fontBoxRenderMaterial);
            Render(fontRenderMaterial);
        }

        private void RenderBox(MaterialBase fontBoxRenderMaterial)
        {
            using (var blend = new ScopedEnable(EnableCap.Blend))
            using (var dummy = new ScopedDisable(EnableCap.DepthTest))
            using (var blendFunc = new ScopedBlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha))
            {
                fontBoxRenderMaterial.BindAndExecute(boxVB, () =>
                {
                    fontBoxRenderMaterial.SetUniformVarData("ScreenSize",
                        new Vector2(OpenGLContext.Get().WindowWidth, OpenGLContext.Get().WindowHeight));
                    fontBoxRenderMaterial.SetUniformVarData("BoxColor", new Vector3(0,0,0));
                    fontBoxRenderMaterial.SetUniformVarData("BoxAlpha", 0.70f);
                    GL.DrawArrays(PrimitiveType.Quads, 0, 4);
                });
            }
        }

        public void Render(MaterialBase fontRenderMaterial)
        {
            
        }



        protected void GenerateVertices(float posX, float posY)
        {
            
        }

        public string TextContent = "";

        private DynamicVertexBuffer<PT_VertexAttribute> vb = null;
        private DynamicVertexBuffer<PT_VertexAttribute> boxVB = null;

        private List<PT_VertexAttribute> vertexList = new List<PT_VertexAttribute>();
        private List<PT_VertexAttribute> boxVertexList = new List<PT_VertexAttribute>();

        public float Left => left;
        public float Right => right;
        public float Top => top;
        public float Bottom => bottom;

        private float left = float.MaxValue;
        private float right = float.MinValue;
        private float top = float.MinValue;
        private float bottom = float.MaxValue;

        private float originX = 0;
        private float originY = 0;
    }
}
