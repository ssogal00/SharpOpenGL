using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;

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
        public DrawCommand(IBindable vertexBuffer, IBindable indexBuffer)
        {
            VertexBuffer = vertexBuffer;
            IndexBuffer = indexBuffer;
        }

        public override void Execute()
        {
            VertexBuffer.Bind();
            IndexBuffer.Bind();

            

            VertexBuffer.Unbind();
            IndexBuffer.Unbind();
        }

        private IBindable VertexBuffer = null;
        private IBindable IndexBuffer = null;
    }



    public class SetRenderTargetCommand : RenderCommand
    {

    }

}
