using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core
{    
    public class RenderingThreadObject : IDisposable
    {
        // 
        public static EventHandler<EventArgs> OnOpenGLContextCreated;

        public RenderingThreadObject()
        {
            OnOpenGLContextCreated += this.OnGLContextCreated;
        }

        public virtual void ReleaseResource()
        {
            bReleased = true;
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

        public virtual void OnGLContextCreated()
        {
            Initialize();
        }

        public virtual void Initialize()
        {
            bInit = true;
        }

        public virtual bool IsInitialized()
        {
            return bInit;
        }

        public virtual bool IsReleased()
        {
            return bReleased;
        }
        
        protected bool bInit = false;

        protected bool bReleased = false;
    }
}
