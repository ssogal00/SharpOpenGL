using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core;
using Core.OpenGLShader;

namespace ShaderCompiler
{
    public partial class FragmentShaderTemplate
    {
        public FragmentShaderTemplate(ShaderProgram Program, string ShaderName,string ShaderSource)
        {
            ProgramObject = Program;
            FSShaderName = ShaderName;
            SourceCode = ShaderSource;
        }

        ShaderProgram ProgramObject;
        string FSShaderName = "";
        string SourceCode = "";
    }
}
