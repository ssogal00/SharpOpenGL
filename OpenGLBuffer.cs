using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SharpOpenGL
{
    public class OpenGLBuffer
    {
        public OpenGLBuffer(BufferTarget Target, BufferUsageHint Hint)
        {
            m_BufferTarget = Target;
            m_BufferObject = GL.GenBuffer();            
        }

        public void Bind()
        {
            GL.BindBuffer(m_BufferTarget, m_BufferObject);
        }

        public void Unbind()
        {
            GL.BindBuffer(m_BufferTarget, 0);
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
    }
}
