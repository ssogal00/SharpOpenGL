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