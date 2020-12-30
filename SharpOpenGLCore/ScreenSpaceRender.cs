using Core;
using Core.Buffer;
using Core.CustomAttribute;
using Core.Primitive;
using Core.Texture;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace SharpOpenGLCore
{
    public abstract class ScreenSpaceRender : RenderingThreadObject
    {
        public ScreenSpaceRender()
        {
            Initialize();
        }

        [ExposeUI]
        public string Name { get; set; }

        public override void Initialize()
        {
            // create vertex, index buffer for screen space drawing
            VB = new AOSVertexBuffer<PT_VertexAttribute>();
            IB = new IndexBuffer();
            VA = new VertexArray();

            VA.Bind();
            UpdateVertexBuffer();
            UpdateIndexBuffer();
        }

        protected void BlitToScreenSpace()
        {
            using (var s = new ScopedBind(VA,VB,IB))
            {
                GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
            }
        }

        
        public virtual void Render()
        {

        }

        public virtual void Render(TextureBase Input0)
        {

        }

        public virtual void Render(TextureBase Input0, TextureBase Input1)
        {
        }

        

        // indices and vertices to draw in screen space
        protected List<uint> Indices = new List<uint>();
        protected AOSVertexBuffer<PT_VertexAttribute> VB = null;
        protected IndexBuffer IB = null;
        protected VertexArray VA = null;

        List<PT_VertexAttribute> VertexList = new List<PT_VertexAttribute>();

        protected void UpdateIndexBuffer()
        {
            // feed index buffer
            uint[] IndexArray = { 0, 1, 2, 3, 4, 5 };
            IB.Bind();
            IB.BufferData<uint>(ref IndexArray);
        }

        protected void UpdateVertexBuffer()
        {
            VertexList.Clear();

            var EachVertex = new PT_VertexAttribute();

            // uppper left
            EachVertex.VertexPosition = new Vector3(-1, 1, 0);
            EachVertex.TexCoord = new Vector2(0, 1);
            VertexList.Add(EachVertex);

            // lower right
            EachVertex.VertexPosition = new Vector3(1, -1, 0);
            EachVertex.TexCoord = new Vector2(1, 0);
            VertexList.Add(EachVertex);

            // lower left
            EachVertex.VertexPosition = new Vector3(-1, -1, 0);
            EachVertex.TexCoord = new Vector2(0, 0);
            VertexList.Add(EachVertex);

            // upper left
            EachVertex.VertexPosition = new Vector3(-1, 1, 0);
            EachVertex.TexCoord = new Vector2(0, 1);
            VertexList.Add(EachVertex);

            // upper right
            EachVertex.VertexPosition = new Vector3(1, 1, 0);
            EachVertex.TexCoord = new Vector2(1, 1);
            VertexList.Add(EachVertex);

            // lower right
            EachVertex.VertexPosition = new Vector3(1, -1, 0);
            EachVertex.TexCoord = new Vector2(1, 0);
            VertexList.Add(EachVertex);

            // feed vertex buffer
            VB.Bind();
            var VertexArray = VertexList.ToArray();
            VB.BufferData<PT_VertexAttribute>(ref VertexArray);
        }
    }
}
