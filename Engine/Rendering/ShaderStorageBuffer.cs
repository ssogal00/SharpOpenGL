using System;
using System.Collections.Generic;
using System.Text;
using Core.Buffer;
using OpenTK.Graphics.OpenGL;

namespace Engine.Rendering
{
    public class ShaderStorageBuffer : OpenGLBuffer
    {
        public ShaderStorageBuffer()
        {
            mBufferTarget = BufferTarget.ShaderStorageBuffer;
            mHint = BufferUsageHint.DynamicCopy;
        }
    }
}
