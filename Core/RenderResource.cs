using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core
{    
    public class RenderResource : IDisposable
    {
        public static EventHandler<EventArgs> OnResourceCreated;
        public static EventHandler<CustomEvent.ScreenResizeEventArgs> OnWindowResized;

        public RenderResource()
        {
            OnResourceCreated += this.OnCreateResource;
            OnWindowResized += this.OnWindowResize;
        }

        public virtual void Dispose()
        {
            OnResourceCreated -= this.OnCreateResource;
            OnWindowResized -= this.OnWindowResize;
        }

        public virtual void OnCreateResource(object sender, EventArgs e)
        {
        }

        public virtual void OnWindowResize(object sender, CustomEvent.ScreenResizeEventArgs e)
        {
        }
    }
}
