using OpenTK.Graphics.OpenGL;

namespace Engine.Rendering
{
    //
    public class GeometryShader : Core.OpenGLShader.Shader
    {
        public GeometryShader()
        {
            ShaderObject = GL.CreateShader(ShaderType.GeometryShader);
        }
    }
}
