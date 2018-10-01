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

    public class ScopedDepthDisable : IDisposable
    {
        public ScopedDepthDisable()
        {
            GL.Disable(EnableCap.DepthTest);
        }

        public void Dispose()
        {
            GL.Enable(EnableCap.DepthTest);
        }
    }

    public class ScopedBind : IDisposable
    {
        public ScopedBind(params IBindable[] bindableList)
        {
            foreach(var each in bindableList)
            {
                each.Bind();
            }

            BindableList = bindableList;
        }

        public ScopedBind(IEnumerable<IBindable> bindableList)
        {
            foreach(var each in bindableList)
            {
                each.Bind();
            }
            BindableList = bindableList.ToArray();
        }

        public void Dispose()
        {
            foreach(var each in BindableList)
            {
                each.Unbind();
            }
        }

        private IBindable[] BindableList = null;
    }
    
}
