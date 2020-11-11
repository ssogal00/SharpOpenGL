using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Core.OpenGLShader
{
    public class TesselEvalShader : Shader
    {
        public TesselEvalShader()
        {
            ShaderObject = GL.CreateShader(ShaderType.TessEvaluationShader);
        }
    }
}
