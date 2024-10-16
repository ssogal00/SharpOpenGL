﻿using System;
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
            mBufferTarget = BufferTarget.ElementArrayBuffer;
            mHint = BufferUsageHint.DynamicDraw;
        }
    }
}
