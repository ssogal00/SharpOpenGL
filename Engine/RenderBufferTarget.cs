using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Buffer;
using Core.CustomEvent;

namespace Core.Texture
{
    public class RenderBufferTarget : RenderResource , IBindable, IResizable
    {
        public RenderBufferTarget()
        {

        }

        public void OnResize(int width , int height)
        {

        }

        public override void Initialize()
        {
            
        }

        public void Bind()
        {

        }

        public void Unbind()
        {

        }

        protected FrameBuffer frameBuffer = null;
        protected RenderBuffer renderBuffer = null;
    }
}
