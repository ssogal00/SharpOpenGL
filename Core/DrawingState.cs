using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Core
{
    public class WireFrameMode : IDisposable
    {
        public WireFrameMode()
        {
            GL.GetInteger(GetPName.PolygonMode, out ePrevPolygonMode);
            
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
        }

        public void Dispose()
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, (PolygonMode) ePrevPolygonMode);
        }
        
        private int ePrevPolygonMode = (int)PolygonMode.Fill;
    }

    public class ScopedBind : IDisposable
    {
        public ScopedBind(IBindable bindable)
        {
            this.bindable = bindable;
            this.bindable.Bind();
        }

        public void Dispose()
        {
            this.bindable.Unbind();
        }

        private IBindable bindable = null;
    }
    
}
