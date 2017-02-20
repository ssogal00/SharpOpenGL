using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpOpenGL;

namespace ShaderCompiler
{
    public partial class FragmentShaderTemplate
    {
        public FragmentShaderTemplate(ShaderProgram Program, string ShaderName)
        {
            ProgramObject = Program;
            FSShaderName = ShaderName;
        }

        ShaderProgram ProgramObject;
        string FSShaderName = "";
    }
}
