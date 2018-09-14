using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;

namespace Core.OpenGLShader
{
    public class SeparableShaderProgram : ShaderProgramBase
    {
        public SeparableShaderProgram()
            : base()
        {
        }

        public SeparableShaderProgram(VertexShader vs)
        {
            GL.ProgramParameter(ProgramObject, ProgramParameterName.ProgramSeparable, (int) 1);
            AttachShader(vs);
            string Result;
            Debug.Assert(LinkProgram(out Result));
            CacheShaderProgramInfo();
        }

        public SeparableShaderProgram(FragmentShader fs)
        {
            GL.ProgramParameter(ProgramObject, ProgramParameterName.ProgramSeparable, (int)1);
            AttachShader(fs);
            string Result;
            Debug.Assert(LinkProgram(out Result));
            CacheShaderProgramInfo();
        }
    }
}
