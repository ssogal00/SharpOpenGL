using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpOpenGL;

namespace ShaderCompiler
{
    public class FragmentShaderCodeGenerator : CodeGenerator
    {   
        public FragmentShaderCodeGenerator(SharpOpenGL.ShaderProgram ProgramObject, string Name)
        {
            Program = ProgramObject;
            NameSpace = string.Format("SharpOpenGL.{0}", Name);
        }

        protected override string GetCodeContents()
        {
            var template = new FragmentShaderTemplate(Program, "");
            return template.TransformText();
        }

        ShaderProgram Program;        
    }
}
