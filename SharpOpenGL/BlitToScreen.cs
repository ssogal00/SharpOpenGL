using Core.Primitive;
using System.Collections.Generic;
using Core;
using Core.Buffer;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace SharpOpenGL
{
    public class BlitToScreen
    {
        public BlitToScreen()
        {
            //
            
        }

        public void OnResourceCreate(object sender, System.EventArgs eventArgs)
        {
            //
            List<PT_VertexAttribute> VertexList = new List<PT_VertexAttribute>();

            // upper left
            var EachVertex = new PT_VertexAttribute();
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

            // create render resource
            VB = new StaticVertexBuffer<PT_VertexAttribute>();
            IB = new IndexBuffer();
            Material = new ScreenSpaceDraw.ScreenSpaceDraw();

            // feed vertex buffer
            var VertexArray = VertexList.ToArray();
            VB.BufferData<PT_VertexAttribute>(ref VertexArray);

            // feed index buffer
            uint[] IndexArray = { 0, 1, 2, 3, 4, 5 };
            IB.BufferData<uint>(ref IndexArray);
        }

        
        public void Draw()
        {
            Material.Use();
            VB.Bind();
            IB.Bind();
            PT_VertexAttribute.VertexAttributeBinding();
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }

        protected SharpOpenGL.ScreenSpaceDraw.ScreenSpaceDraw Material = null;
        protected List<uint> Indices = new List<uint>();
        protected StaticVertexBuffer<PT_VertexAttribute> VB = null;
        protected IndexBuffer IB = null;
    }
}
