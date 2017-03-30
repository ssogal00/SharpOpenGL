using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core;

using ShaderProgram = Core.OpenGLShader.ShaderProgram;

namespace ShaderCompiler
{
    public class FragmentShaderCodeGenerator : CodeGenerator
    {   
        public FragmentShaderCodeGenerator(Core.OpenGLShader.ShaderProgram ProgramObject, string Name)
        {
            Program = ProgramObject;
            NameSpace = string.Format("CompiledShader.{0}", Name);
        }

        protected override string GetCodeContents()
        {
            var template = new FragmentShaderTemplate(Program, "");
            return template.TransformText();
        }

        ShaderProgram Program;        
    }
}
