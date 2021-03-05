using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace Core
{
    // change render state or draw 
    public class RenderCommand
    {
        public RenderCommand()
        {

        }

        //
        protected virtual void Setup()
        {
        }

        public virtual void Execute()
        {

        }
    }

    public class ClearRenderCommand : RenderCommand
    {
        public ClearRenderCommand(Color clearColor, ClearBufferMask mask=ClearBufferMask.ColorBufferBit|ClearBufferMask.DepthBufferBit)
        {
        }

        public override void Execute()
        {
            GL.ClearColor(mClearColor);
            GL.Clear(mBufferMask);
        }

        private Color mClearColor = Color.White;
        private ClearBufferMask mBufferMask = ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit;
    }

    public class DrawCommand : RenderCommand
    {
        public DrawCommand(MaterialBase.MaterialBase material, IBindable vertexBuffer, IBindable indexBuffer)
        {
            MaterialToUse = material;
            VertexBuffer = vertexBuffer;
            IndexBuffer = indexBuffer;
        }

        public override void Execute()
        {
            MaterialToUse.Bind();
            VertexBuffer.Bind();
            IndexBuffer.Bind();

            
            
            MaterialToUse.Unbind();
            VertexBuffer.Unbind();
            IndexBuffer.Unbind();
        }

        private MaterialBase.MaterialBase MaterialToUse = null;
        private IBindable VertexBuffer = null;
        private IBindable IndexBuffer = null;
    }



    

}
