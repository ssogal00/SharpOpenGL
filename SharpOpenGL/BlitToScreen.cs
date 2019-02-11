using Core.Primitive;
using System.Collections.Generic;
using Core;
using Core.Buffer;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using Core.Texture;
using System.Diagnostics;

namespace SharpOpenGL
{
    public class BlitToScreen : RenderResource
    {
        public BlitToScreen()
        {
            //            
        }

        public void Create()
        {
            Material = new ScreenSpaceDraw.ScreenSpaceDraw();

            VB = new StaticVertexBuffer<PT_VertexAttribute>();
            IB = new IndexBuffer();
            UpdateVertexBuffer(0, 0, 1, 1, 1, 1);
            // feed index buffer
            uint[] IndexArray = { 0, 1, 2, 3, 4, 5 };
            IB.BufferData<uint>(ref IndexArray);
        }

        public override void OnGLContextCreated(object sender, System.EventArgs eventArgs)
        {            
            Create();
        }
        
        public void Blit(TextureBase texture)
        {
            using (var depthDisable = new ScopedDisable(EnableCap.DepthTest))
            {
                Material.BindAndExecute(VB, IB, () =>
                {
                    Material.ColorTex2D = texture;
                    GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
                });
            }
        }

        public void Blit(TextureBase texture, int rowIndex, int colIndex, int gridRowSpan, int gridColSpan)
        {
            using (var depthDisable = new ScopedDisable(EnableCap.DepthTest))
            {
                Material.BindAndExecute(VB,IB,() =>
                {
                    UpdateVertexBuffer(rowIndex, colIndex, GridRowSize, GridColSize, gridRowSpan, gridColSpan);
                    Material.ColorTex2D = texture;
                    GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
                });
            }
        }

        public void Blit(int textureObject)
        {
            using (var depthDisable = new ScopedDisable(EnableCap.DepthTest))
            {
                Material.BindAndExecute(VB, IB, () =>
                {
                    Material.SetColorTex2D(textureObject, Sampler.DefaultLinearSampler);
                    GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
                });
            }
        }

        public void Blit(int textureObject, int rowIndex, int colIndex, int gridRowSpan, int gridColSpan)
        {
            using (var depthDisable = new ScopedDisable(EnableCap.DepthTest))
            {
                Material.BindAndExecute(VB,IB, () =>
                {
                    UpdateVertexBuffer(rowIndex, colIndex, GridRowSize, GridColSize, gridRowSpan, gridColSpan);
                    Material.SetColorTex2D(textureObject, Sampler.DefaultLinearSampler);
                    GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
                });
            }
        }

        public void SetGridSize(int newGridRow, int newGridCol)
        {
            Debug.Assert(newGridRow > 0 && newGridCol > 0);

            GridRowSize = newGridRow;
            GridColSize = newGridCol;            
        }        

        protected void UpdateVertexBuffer(int gridRowIndex, int gridColIndex, int gridRowSize, int gridColSize, int gridRowSpan, int gridColSpan)
        {            
            GridRowIndex = gridRowIndex;
            GridColIndex = gridColIndex;
            GridColSize = gridColSize;
            GridRowSize = gridRowSize;
            GridRowSpan = gridRowSpan;
            GridColSpan = gridColSpan;

            float fRowGridLength = 2.0f / (float)GridRowSize * (float)GridRowSpan;
            float fColGridLength = 2.0f / (float)GridColSize * (float)GridColSpan;

            float fRowOffset = (float)gridRowIndex * fRowGridLength;
            float fColOffset = (float)gridColIndex * fColGridLength;

            float fOriginX = -1 + fRowOffset;
            float fOriginY =  1 - fColOffset;

            VertexList.Clear();

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

            // feed vertex buffer
            VB.Bind();
            var VertexArray = VertexList.ToArray();
            VB.BufferData<PT_VertexAttribute>(ref VertexArray);
        }

        protected SharpOpenGL.ScreenSpaceDraw.ScreenSpaceDraw Material = null;
        protected List<uint> Indices = new List<uint>();
        protected StaticVertexBuffer<PT_VertexAttribute> VB = null;
        protected IndexBuffer IB = null;
        List<PT_VertexAttribute> VertexList = new List<PT_VertexAttribute>();
        
        // 
        protected int GridRowSize = 1;
        protected int GridColSize = 1;

        protected int GridRowIndex = 0;
        protected int GridColIndex = 0;

        protected int GridRowSpan = 1;
        protected int GridColSpan = 1;
    }
}
