using OpenTK.Graphics.OpenGL;


namespace Core.OpenGLShader
{
    public class VertexShader : Shader
    {
        public VertexShader()
        {
            ShaderObject = GL.CreateShader(ShaderType.VertexShader);
        }
    }
}
