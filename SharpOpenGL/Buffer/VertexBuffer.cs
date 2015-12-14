using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SharpOpenGL.Buffer 
{
    public class VertexBuffer : OpenGLBuffer
    {
        public VertexBuffer()
        {
            m_BufferTarget = BufferTarget.ArrayBuffer;
            m_Hint = BufferUsageHint.DynamicDraw;
        }


    }
}
