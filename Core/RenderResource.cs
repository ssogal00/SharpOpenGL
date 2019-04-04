using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core
{    
    public class RenderResource : IDisposable
    {
        // 
        public static EventHandler<EventArgs> OnOpenGLContextCreated;

        //
        public static EventHandler<EventArgs> OnRenderingThreadTick;

        public RenderResource()
        {
            OnOpenGLContextCreated += this.OnGLContextCreated;
        }

        public virtual void ReleaseResource()
        {
        }

        public virtual void Dispose()
        {
            ReleaseResource();
            OnOpenGLContextCreated -= this.OnGLContextCreated;
        }

        public virtual void OnGLContextCreated(object sender, EventArgs e)
        {
            Initialize();
        }

        public virtual void Initialize()
        {
            bInit = true;
        }
        
        protected bool bInit = false;
    }
}
