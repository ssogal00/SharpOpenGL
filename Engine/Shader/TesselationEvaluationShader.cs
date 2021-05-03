using System;
using System.Collections.Generic;
using System.Text;
using Core.OpenGLShader;
using OpenTK.Graphics.OpenGL;

namespace Engine.Shader
{
    public class TesselationEvaluationShader : Core.OpenGLShader.Shader
    {
        public TesselationEvaluationShader()
        {
            ShaderObject = GL.CreateShader(ShaderType.TessEvaluationShader);
        }
    }
}
