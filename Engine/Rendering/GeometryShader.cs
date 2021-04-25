using System;
using System.Collections.Generic;
using System.Text;
using Core.OpenGLShader;
using OpenTK.Graphics.OpenGL;

namespace Engine.Rendering
{
    //
    public class GeometryShader : Shader
    {
        public GeometryShader()
        {
            ShaderObject = GL.CreateShader(ShaderType.GeometryShader);
        }
    }
}
