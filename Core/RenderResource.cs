using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core
{    
    public class RenderResource : IDisposable
    {
        public static EventHandler<EventArgs> OnOpenGLContextCreated;
        public static EventHandler<CustomEvent.ScreenResizeEventArgs> OnWindowResized;

        public RenderResource()
        {
            OnOpenGLContextCreated += this.OnGLContextCreated;
            OnWindowResized += this.OnWindowResize;
        }

        public virtual void Dispose()
        {
            OnOpenGLContextCreated -= this.OnGLContextCreated;
            OnWindowResized -= this.OnWindowResize;
        }

        public virtual void OnGLContextCreated(object sender, EventArgs e)
        {
            Initialize();
        }

        public virtual void Initialize()
        {
            bInit = true;
        }

        public virtual void OnWindowResize(object sender, CustomEvent.ScreenResizeEventArgs e)
        {
        }

        protected bool bInit = false;
    }
}
