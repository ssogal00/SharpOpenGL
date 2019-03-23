using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Texture;

namespace SharpOpenGL.Transform
{
    public class CubemapConvolutionTransform : TransformBase
    {

        public override void OnGLContextCreated(object sender, EventArgs e)
        {
            base.OnGLContextCreated(sender, e);

        }



        private RenderTarget PositiveX = new RenderTarget(32, 32, 1, true);
        private RenderTarget NegativeX = new RenderTarget(32, 32, 1, true);
        private RenderTarget PositiveY = new RenderTarget(32, 32, 1, true);
        private RenderTarget NegativeY = new RenderTarget(32, 32, 1, true);
        private RenderTarget PositiveZ = new RenderTarget(32, 32, 1, true);
        private RenderTarget NegativeZ = new RenderTarget(32, 32, 1, true);
    }
}
