using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace Core.OpenGLShader
{
    public class ShaderProgram : ShaderProgramBase
    {
        public ShaderProgram()
            : base()
        {
        }

        public ShaderProgram(VertexShader VS, FragmentShader FS)
        {
            AttachShader(VS);
            AttachShader(FS);
            string Result;            
            Debug.Assert(LinkProgram(out Result));

            CacheShaderProgramInfo();
        }
      
        public ShaderProgram(VertexShader vs, FragmentShader fs, TesselControlShader tc, TesselEvalShader te)
        {
            AttachShader(vs);
            AttachShader(fs);
            AttachShader(tc);
            AttachShader(te);

            string Result;
            Debug.Assert(LinkProgram(out Result));

            CacheShaderProgramInfo();
        }
    }    
}
