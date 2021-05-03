using System;
using System.Collections.Generic;
using System.Text;
using Core.OpenGLShader;
using OpenTK.Graphics.OpenGL;

namespace Engine.Rendering
{
    //
    public class TesslationControlShader : Core.OpenGLShader.Shader
    {
        public TesslationControlShader()
        {
            ShaderObject = GL.CreateShader(ShaderType.TessControlShader);
        }
    }
}