using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SharpOpenGL.Buffer
{
    public class StaticUniformBuffer : OpenGLBuffer
    {
        public StaticUniformBuffer()
        {
            m_BufferTarget = BufferTarget.UniformBuffer;
            m_Hint = BufferUsageHint.StaticDraw;
        }
    }
}
