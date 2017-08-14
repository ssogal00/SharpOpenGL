using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using System.Reflection;
using System.Runtime.InteropServices;

namespace Core.Buffer
{
    public abstract class OpenGLBuffer : IDisposable
    {
        public OpenGLBuffer()
        {
            m_BufferObject = GL.GenBuffer();
        }

        public void Dispose()
        {
            Unbind();
            GL.DeleteBuffer(m_BufferObject);
        }

        public void Bind()
        {
            GL.BindBuffer(m_BufferTarget, m_BufferObject);
            bBind = true;
        }

        public bool IsBind
        {
            get { return bBind; }
        }

        public void Unbind()
        {
            GL.BindBuffer(m_BufferTarget, 0);
            bBind = false;
        }

        public void BufferData(IntPtr Size, IntPtr Data)
        {
            if(!IsBind)
            {
                Bind();
            }         
            
            GL.BufferData(m_BufferTarget, Size, Data, m_Hint);            
        }

        public void BufferData<T>(ref T Data) where T : struct
        {
            if (!IsBind)
            {
                Bind();
            }
            
            var Size = new IntPtr(Marshal.SizeOf(Data));
            GL.BufferData<T>(m_BufferTarget, Size, ref Data, m_Hint);            
        }
     
        public void BufferWholeData<T>(ref T Data) where T: struct
        {
            if (!IsBind)
            {
                Bind();
            }
            GL.BufferSubData<T>(m_BufferTarget, new IntPtr(0), Marshal.SizeOf(Data), ref Data);
	    }

    public void BufferData<T>(ref T[] Data) where T: struct
        {
            if (!IsBind)
            {
                Bind();
            }
            if (Data != null)
            {
                var Size = new IntPtr(Marshal.SizeOf(Data[0]) * Data.Length);
                GL.BufferData<T>(m_BufferTarget, Size, Data, m_Hint);
            }
        }

        public void BindBufferBase(int BindingPoint)
        {
            if (!IsBind)
            {
                Bind();
            }            
            
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, BindingPoint, m_BufferObject);            
        }
        
        public BufferUsageHint UsageHint
        {
            get { return m_Hint; }
            set { m_Hint = value; }
        }

        public BufferTarget Target 
        { 
            get { return m_BufferTarget; }
            protected set { m_BufferTarget = value; }
        }

        public int BufferObject
        {
            get { return m_BufferObject; }
            protected set { m_BufferObject = value; }
        }

        protected BufferTarget m_BufferTarget;

        protected BufferUsageHint m_Hint;

        protected int m_BufferObject = 0;

        protected bool bBind = false;
    }
}
