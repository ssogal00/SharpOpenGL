using System;
using System.Collections.Generic;
using System.Text;
using Core.Buffer;
using OpenTK.Graphics.OpenGL;

namespace Engine
{
    public class AtomicCountBuffer : OpenGLBuffer
    {
        public AtomicCountBuffer()
        {
            mBufferTarget = BufferTarget.AtomicCounterBuffer;
        }
    }
}
