using OpenTK.Graphics.OpenGL;

namespace Core.OpenGLShader
{
    public class FragmentShader : Shader
    {
        public FragmentShader()
        {
            ShaderObject = GL.CreateShader(ShaderType.FragmentShader);
        }
    }
}
