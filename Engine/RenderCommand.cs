using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using Core.MaterialBase;
using OpenTK.Graphics.OpenGL4;

namespace Core
{
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



    public class SetRenderTargetCommand : RenderCommand
    {

    }

}
