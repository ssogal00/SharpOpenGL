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

        protected int mOrder = 0;
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

    public class BlendingOnCommand : RenderCommand
    {
        public override void Execute()
        {
            GL.Enable(EnableCap.Blend);
        }
    }

    public class BlendingOffCommand : RenderCommand
    {
        public override void Execute()
        {
            GL.Disable(EnableCap.Blend);
        }
    }

    public class BlendingFuncCommand : RenderCommand
    {
        public BlendingFuncCommand(BlendingFactor src, BlendingFactor dst)
        {
            mSrc = src;
            mDst = dst;
        }
        public override void Execute()
        {
            GL.BlendFunc(mSrc, mDst);
        }

        protected BlendingFactor mSrc;
        protected BlendingFactor mDst;
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
