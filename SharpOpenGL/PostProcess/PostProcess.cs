using System;
using Core.Texture;
using Core.MaterialBase;
using System.Collections.Generic;
using Core.Buffer;
using Core.VertexCustomAttribute;
using Core.Primitive;
using OpenTK.Graphics.OpenGL;
using Core.CustomEvent;
using Core;

namespace SharpOpenGL.PostProcess
{
    public abstract class PostProcessBase : RenderResource
    {   
        public PostProcessBase()
        {   
        }

        public override void Initialize()
        {
            VB = new StaticVertexBuffer<PT_VertexAttribute>();
            IB = new IndexBuffer();
            UpdateVertexBuffer();
            UpdateIndexBuffer();
        }

        public override void OnWindowResize(object sender, ScreenResizeEventArgs e)
        {
            Output.OnWindowResize(sender, e);
        }

        protected void BlitToScreenSpace()
        {
            using (var s = new ScopedBind(VB, IB))
            {
                PT_VertexAttribute.VertexAttributeBinding();
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

        public virtual void Render(TextureBase Input0, TextureBase Input1, TextureBase Input2)
        {
        }        

        public RenderTarget GetOutputTextureObject()
        {
            return Output;
        }

        protected RenderTarget Output = new RenderTarget(1024, 768, 1);

        protected Core.MaterialBase.MaterialBase PostProcessMaterial = null;
                
        // indices and vertices to draw in screen space
        protected List<uint> Indices = new List<uint>();
        protected StaticVertexBuffer<PT_VertexAttribute> VB = null;
        protected IndexBuffer IB = null;
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
            EachVertex.VertexPosition = new OpenTK.Vector3(-1, 1, 0);
            EachVertex.TexCoord = new OpenTK.Vector2(0, 1);
            VertexList.Add(EachVertex);

            // lower right
            EachVertex.VertexPosition = new OpenTK.Vector3(1, -1, 0);
            EachVertex.TexCoord = new OpenTK.Vector2(1, 0);
            VertexList.Add(EachVertex);

            // lower left
            EachVertex.VertexPosition = new OpenTK.Vector3(-1, -1, 0);
            EachVertex.TexCoord = new OpenTK.Vector2(0, 0);
            VertexList.Add(EachVertex);

            // upper left
            EachVertex.VertexPosition = new OpenTK.Vector3(-1, 1, 0);
            EachVertex.TexCoord = new OpenTK.Vector2(0, 1);
            VertexList.Add(EachVertex);

            // upper right
            EachVertex.VertexPosition = new OpenTK.Vector3(1, 1, 0);
            EachVertex.TexCoord = new OpenTK.Vector2(1, 1);
            VertexList.Add(EachVertex);

            // lower right
            EachVertex.VertexPosition = new OpenTK.Vector3(1, -1, 0);
            EachVertex.TexCoord = new OpenTK.Vector2(1, 0);
            VertexList.Add(EachVertex);

            // feed vertex buffer
            VB.Bind();
            var VertexArray = VertexList.ToArray();
            VB.BufferData<PT_VertexAttribute>(ref VertexArray);
        }
        
    }
}
