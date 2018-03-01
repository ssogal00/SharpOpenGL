using Core.Primitive;
using System.Collections.Generic;
using Core;
using Core.Buffer;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using Core.Texture;

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



        public void Blit(Texture2D texture)
        {
            Material.Use();
            VB.Bind();
            IB.Bind();
            PT_VertexAttribute.VertexAttributeBinding();
            Material.SetColorTex2D(texture);
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }

        public void Blit(Texture2D texture, int rowIndex, int colIndex)
        {
            
        }

        public void Blit(int textureObject)
        {
            Material.Use();            
            VB.Bind();
            IB.Bind();
            PT_VertexAttribute.VertexAttributeBinding();
            Material.SetColorTex2D(textureObject, Sampler.DefaultLinearSampler);
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }

        public void SetGridSize(int newGridRow, int newGridCol)
        {
            GridRowSize = newGridRow;
            GridColSize = newGridCol;
        }

        

        protected void UpdateVertexBuffer(int gridRow, int gridCol)
        {
            List<PT_VertexAttribute> VertexList = new List<PT_VertexAttribute>();

            float fRowGridLength = 2.0f / (float)GridRowSize;
            float fColGridLength = 2.0f / (float)GridColSize;

            float fRowOffset = (float)gridRow * fRowGridLength;
            float fColOffset = (float)gridCol * fColGridLength;

            float fOriginX = -1 + fRowOffset;
            float fOriginY =  1 - fColOffset;

            // upper left
            var EachVertex = new PT_VertexAttribute();
            EachVertex.VertexPosition = new OpenTK.Vector3(fOriginX, fOriginY , 0);
            EachVertex.TexCoord = new OpenTK.Vector2(0, 1);
            VertexList.Add(EachVertex);

            // lower right
            EachVertex.VertexPosition = new OpenTK.Vector3(fOriginX + fRowGridLength, fOriginY - fColGridLength, 0);
            EachVertex.TexCoord = new OpenTK.Vector2(1, 0);
            VertexList.Add(EachVertex);

            // lower left
            EachVertex.VertexPosition = new OpenTK.Vector3(fOriginX, fOriginY - fColGridLength, 0);
            EachVertex.TexCoord = new OpenTK.Vector2(0, 0);
            VertexList.Add(EachVertex);

            // upper left
            EachVertex.VertexPosition = new OpenTK.Vector3(fOriginX, fOriginY, 0);
            EachVertex.TexCoord = new OpenTK.Vector2(0, 1);
            VertexList.Add(EachVertex);

            // upper right
            EachVertex.VertexPosition = new OpenTK.Vector3(fOriginX + fRowGridLength, fOriginY, 0);
            EachVertex.TexCoord = new OpenTK.Vector2(1, 1);
            VertexList.Add(EachVertex);

            // lower right
            EachVertex.VertexPosition = new OpenTK.Vector3(fOriginX + fRowGridLength, fOriginY - fColGridLength, 0);
            EachVertex.TexCoord = new OpenTK.Vector2(1, 0);
            VertexList.Add(EachVertex);

            // create render resource
            VB = new StaticVertexBuffer<PT_VertexAttribute>();
            IB = new IndexBuffer();
            Material = new ScreenSpaceDraw.ScreenSpaceDraw();

            // feed vertex buffer
            var VertexArray = VertexList.ToArray();
            VB.BufferData<PT_VertexAttribute>(ref VertexArray);
        }

        protected SharpOpenGL.ScreenSpaceDraw.ScreenSpaceDraw Material = null;
        protected List<uint> Indices = new List<uint>();
        protected StaticVertexBuffer<PT_VertexAttribute> VB = null;
        protected IndexBuffer IB = null;

        // 
        protected int GridRowSize = 1;
        protected int GridColSize = 1;
        protected int GridIndex = 0;
    }
}
