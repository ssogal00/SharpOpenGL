using Core;
using Core.Buffer;
using Core.CustomAttribute;
using Core.Primitive;
using Core.Texture;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace SharpOpenGL.PostProcess
{
    public abstract class PostProcessBase : RenderResource
    {   
        public PostProcessBase()
        {
        }

        [ExposeUI]
        public string Name { get; set; }
        
        public override void Initialize()
        {
            // create vertex, index buffer for screen space drawing
            VB = new StaticVertexBuffer<PT_VertexAttribute>();
            IB = new IndexBuffer();
            UpdateVertexBuffer();
            UpdateIndexBuffer();

            // 
            if (bOwnItsRenderTarget)
            {
                // create render target
                if (bCreateCustomRenderTarget)
                {
                    CreateCustomRenderTarget();
                }
                else
                {
                    CreateDefaultRenderTarget();
                }
            }
        }

        protected void BlitToScreenSpace()
        {
            using (var s = new ScopedBind(VB, IB))
            {
                GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
            }
        }

        protected virtual void CreateDefaultRenderTarget()
        {
            Output = new RenderTarget(1024,768,1, false);
            Output.Initialize();
        }

        protected virtual void CreateCustomRenderTarget()
        {
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

        public virtual void Render(TextureBase Input0, TextureBase Input1, TextureBase Input3, TextureBase Input4)
        {
        }

        public virtual void Render(TextureBase Input0, TextureBase Input1, TextureBase Input3, TextureBase Input4, TextureBase Input5)
        {
        }

        public virtual void Render(TextureBase Input0, TextureBase Input1, TextureBase Input3, TextureBase Input4, TextureBase Input5, TextureBase Input6)
        {
        }

        public RenderTarget GetOutputRenderTarget()
        {
            return Output;
        }

        public RenderTarget OutputRenderTarget => Output;

        public TextureBase OutputColorTex0 => Output.ColorAttachment0;
        public TextureBase OutputColorTex1 => Output.ColorAttachment1;
        public TextureBase OutputColorTex2 => Output.ColorAttachment2;

        // this could be null
        protected RenderTarget Output;

        protected Core.MaterialBase.MaterialBase PostProcessMaterial = null;
                
        // indices and vertices to draw in screen space
        protected List<uint> Indices = new List<uint>();
        protected StaticVertexBuffer<PT_VertexAttribute> VB = null;
        protected IndexBuffer IB = null;
        List<PT_VertexAttribute> VertexList = new List<PT_VertexAttribute>();

        protected bool bCreateCustomRenderTarget = false;

        protected bool bOwnItsRenderTarget = true;

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
