using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SharpOpenGL
{
    public class FragmentShader : Shader
    {
        public FragmentShader()
        {
            ShaderObject = GL.CreateShader(ShaderType.FragmentShader);
        }
    }
}
