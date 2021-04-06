using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using GetIndexedPName = OpenTK.Graphics.ES30.GetIndexedPName;
using Core.Buffer;
using Core.Texture;

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

    public class ScopedDepthFunc : IDisposable
    {
        public ScopedDepthFunc(DepthFunction func)
        {
            GL.GetInteger(GetPName.DepthFunc, out prevFunc);
            GL.DepthFunc(func);
        }

        public void Dispose()
        {
            GL.DepthFunc((DepthFunction)prevFunc);
        }

        private int prevFunc;
    }

    public class ScopedBlendFunc : IDisposable
    {
        public ScopedBlendFunc(BlendingFactor srcFactor, BlendingFactor dstFactor)
        {
            // get prev blend mode
            GL.GetInteger(GetPName.BlendSrcAlpha, out prevSrcAlpha);
            GL.GetInteger(GetPName.BlendDstAlpha, out prevDstAlpha);

            GL.BlendFunc(srcFactor, dstFactor);
        }

        public void Dispose()
        {
            GL.BlendFunc((BlendingFactor)prevSrcAlpha,  (BlendingFactor)prevDstAlpha);
        }

        private int prevSrcAlpha;
        private int prevDstAlpha;
    }

    public class ScopedRenderTargetBound : IDisposable
    {
        public ScopedRenderTargetBound(RenderTarget target)
        {
            // get original
            GL.GetInteger(GetPName.DrawFramebufferBinding, out originalBounding);

            // bind new buffer
            target.Bind();
        }

        public void Dispose()
        {
            // restore original
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, originalBounding);
        }
        
        protected int originalBounding = 0;
    }

    public class ScopedFrameBufferBound : IDisposable
    {
        public ScopedFrameBufferBound(FrameBuffer buffer)
        {
            // get original
            GL.GetInteger(GetPName.DrawFramebufferBinding, out originalBounding);

            // bind new buffer
            buffer.Bind();
        }

        public void Dispose()
        {
            // restore original
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, originalBounding);
        }

        protected int originalBounding = 0;
    }

    public class ScopedEnable : IDisposable
    {
        public ScopedEnable(EnableCap enable)
        {
            enabled = enable;
            GL.Enable(enable);
        }

        public void Dispose()
        {
            GL.Disable(enabled);
        }

        private EnableCap enabled;
    }

    public class ScopedDisable : IDisposable
    {
        public ScopedDisable(EnableCap disable)
        {
            disabled = disable;
            GL.Disable(disabled);
        }

        public void Dispose()
        {
            GL.Enable(disabled);
        }

        private EnableCap disabled;
    }
    public class ScopedCullFace : IDisposable
    {
        public ScopedCullFace(CullFaceMode NewMode)
        {
            GL.GetInteger(GetPName.CullFaceMode, out OriginalMode);
            GL.CullFace(NewMode);
        }

        public void Dispose()
        {
            GL.CullFace((CullFaceMode) OriginalMode);
        }

        private int OriginalMode = (int)CullFaceMode.Back;
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
