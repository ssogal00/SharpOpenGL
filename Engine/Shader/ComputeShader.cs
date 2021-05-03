using OpenTK.Graphics.OpenGL;

namespace Engine.Rendering
{
    //
    public class ComputeShader : Core.OpenGLShader.Shader
    {
        public ComputeShader()
        {
            ShaderObject = GL.CreateShader(ShaderType.ComputeShader);
        }
    }
}