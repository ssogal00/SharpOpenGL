using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SharpOpenGL.Buffer 
{
    public class StaticVertexBuffer<T> : OpenGLBuffer where T : struct
    {
        public StaticVertexBuffer()
        {
            m_BufferTarget = BufferTarget.ArrayBuffer;
            m_Hint = BufferUsageHint.StaticDraw;
        }
        
        

    }
}
