using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using OpenTK;
using OpenTK.Graphics.OpenGL;
using Core.OpenGLShader;

namespace ShaderCompilerCore
{
    public partial class SamplerGenerator
    {
        public SamplerGenerator(ShaderProgram program, string _StructName)
        {
            var locations = program.GetSampler2DUniformLocations();

            foreach(var loc in locations)
            {
                var name = program.GetUniformName(loc);

                SamplerNameIndexMap.Add(name, loc);
            }

            StructName = _StructName;
        }

        protected string StructName = "";

        protected Dictionary<string, int> SamplerNameIndexMap = new Dictionary<string, int>();
    }
}
