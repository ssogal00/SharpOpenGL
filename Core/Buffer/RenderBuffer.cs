using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        public int GetBufferHandle()
        {
            return renderBufferObject;
        }

        public void Dispose()
        {
            DeleteRenderBuffer();
        }

        private void DeleteRenderBuffer()
        {
            GL.DeleteRenderbuffer(renderBufferObject);
            renderBufferObject = 0;
        }

        public void Bind()
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, renderBufferObject);
        }

        public void Unbind()
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
        }

        public void AllocStorage(RenderbufferStorage storage, int width, int height)
        {
            Bind();
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, storage, width, height);
        }
        
        private int renderBufferObject = 0;
    }
}
