using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using CompiledMaterial.ScreenSpaceDraw;
using Core.Buffer;
using Core.OpenGLShader;
using Core.Primitive;
using Core.Texture;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using SharpOpenGL;

namespace SharpOpenGL
{
    public class MyGameWindow : GameWindow
    {
        public MyGameWindow()
        : base(new GameWindowSettings{ IsMultiThreaded = false, UpdateFrequency = 500, RenderFrequency = 500},
            new NativeWindowSettings{APIVersion = new Version(4,6), API = ContextAPI.OpenGL, AutoLoadBindings = true})
        {
        }

        public IObservable<Unit> GLInit = new Subject<Unit>();

        private StaticVertexBuffer<PT_VertexAttribute> VB;
        private IndexBuffer IB;

        private ScreenSpaceDraw Material = null;

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.ClearColor(Color.Crimson);
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            var texture = TextureManager.Get()
                .LoadTexture2D("./Imported/Resources/Texture/bamboo/bamboo-wood-semigloss-roughness.imported");

            VB.Bind();
            IB.Bind();
            Material.Bind();
            Material.ColorTex2D = texture;
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
        }

        private BlitToScreen blit=new BlitToScreen();

        private Sphere mSphere = null;

        protected override void OnLoad()
        {
            ShaderManager.Get().CompileShaders();
            Sampler.OnResourceCreate(this,null);

            Material = new ScreenSpaceDraw();

            VB = new StaticVertexBuffer<PT_VertexAttribute>();
            IB = new IndexBuffer();

            // feed index buffer
            uint[] IndexArray = { 0, 1, 2, 3, 4, 5 };
            IB.BufferData<uint>(ref IndexArray);
            UpdateVertexBuffer(0,0,1,1,1,1);
            
        }

        protected void UpdateVertexBuffer(int gridRowIndex, int gridColIndex, int gridRowSize, int gridColSize, int gridRowSpan, int gridColSpan)
        {
            List<PT_VertexAttribute> VertexList = new List<PT_VertexAttribute>();
            
            float fRowGridLength = 2.0f / (float)gridRowSize * (float)gridRowSpan;
            float fColGridLength = 2.0f / (float)gridColSize * (float)gridColSpan;

            float fRowOffset = (float)gridRowIndex * fRowGridLength;
            float fColOffset = (float)gridColIndex * fColGridLength;

            float fOriginX = -1 + fRowOffset;
            float fOriginY = 1 - fColOffset;

            VertexList.Clear();

            // upper left
            var EachVertex = new PT_VertexAttribute();
            EachVertex.VertexPosition = new Vector3(fOriginX, fOriginY, 0);
            EachVertex.TexCoord = new Vector2(0, 1);
            VertexList.Add(EachVertex);

            // lower right
            EachVertex.VertexPosition = new Vector3(fOriginX + fRowGridLength, fOriginY - fColGridLength, 0);
            EachVertex.TexCoord = new Vector2(1, 0);
            VertexList.Add(EachVertex);

            // lower left
            EachVertex.VertexPosition = new Vector3(fOriginX, fOriginY - fColGridLength, 0);
            EachVertex.TexCoord = new Vector2(0, 0);
            VertexList.Add(EachVertex);

            // upper left
            EachVertex.VertexPosition = new Vector3(fOriginX, fOriginY, 0);
            EachVertex.TexCoord = new Vector2(0, 1);
            VertexList.Add(EachVertex);

            // upper right
            EachVertex.VertexPosition = new Vector3(fOriginX + fRowGridLength, fOriginY, 0);
            EachVertex.TexCoord = new Vector2(1, 1);
            VertexList.Add(EachVertex);

            // lower right
            EachVertex.VertexPosition = new Vector3(fOriginX + fRowGridLength, fOriginY - fColGridLength, 0);
            EachVertex.TexCoord = new Vector2(1, 0);
            VertexList.Add(EachVertex);

            // feed vertex buffer
            var VertexArray = VertexList.ToArray();
            VB.BufferData<PT_VertexAttribute>(ref VertexArray);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            SharpOpenGL.Engine.Get().RequestExit();
        }

        protected override void OnClosed()
        {
            base.OnClosed();
        }
    }
}
