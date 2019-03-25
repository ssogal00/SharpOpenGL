using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Core.Buffer
{
    public class RenderBuffer : IDisposable, IBindable
    {
        static protected int RenderBufferCount = 0;

        public RenderBuffer()
        {
            renderBufferObject = GL.GenRenderbuffer();
#if DEBUG
            
#endif
        }

        public void Dispose()
        { }

        public void Bind()
        {

        }

        public void Unbind()
        {

        }

        private int renderBufferObject;
    }
}
