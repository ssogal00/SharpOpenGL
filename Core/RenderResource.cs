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

        public RenderResource()
        {
            OnOpenGLContextCreated += this.OnGLContextCreated;
        }

        public virtual void Dispose()
        {
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

        public virtual void Load(string assetPath)
        {
        }
        

        protected bool bInit = false;
    }
}
