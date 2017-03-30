using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Core.Buffer
{
    public class IndexBuffer : OpenGLBuffer
    {
        public IndexBuffer()
        {
            m_BufferTarget = BufferTarget.ElementArrayBuffer;
            m_Hint = BufferUsageHint.DynamicDraw;
        }
    }
}
