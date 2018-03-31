using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpOpenGL;
using Core.OpenGLShader;
using Core.MaterialBase;
using System.IO;
using Core;

namespace MaterialEditor
{
    public class LiveMaterial : MaterialBase
    {
        protected bool lastCompileSuccess = false;

        protected string diffuseColorCode = string.Empty;
        
        public LiveMaterial()
        {
        }

        public bool IsValid()
        {
            return lastCompileSuccess;
        }
        
        public void Compile()
        {
            string errorlog;            

            bool bVSCompileSuccess = VSShader.CompileShader(GetVertexShaderCode(), out errorlog);
            bool bFSCompileSuccess = FSShader.CompileShader(GetFragmentShaderCode(), out errorlog);

            MaterialProgram = new Core.OpenGLShader.ShaderProgram();

            MaterialProgram.AttachShader(VSShader);
            MaterialProgram.AttachShader(FSShader);

            string linkResult;

            bool bLinkResult = MaterialProgram.LinkProgram(out linkResult);

            lastCompileSuccess = bVSCompileSuccess && bFSCompileSuccess && bLinkResult;

            Initialize();
        }

        public void Use()
        {
            if(MaterialProgram != null && MaterialProgram.IsProgramLinked())
            {
                MaterialProgram.UseProgram();
            }
        }

        public void SetDiffuseColorCode(string code)
        {
            diffuseColorCode = code;
        }

        protected string GetVertexShaderCode()
        {
            string vsTemplate = File.ReadAllText("./Resources/Shader/MaterialTemplate.vs");

            return vsTemplate;
        }

        protected string GetFragmentShaderCode()
        {
            string fsTemplate = File.ReadAllText("./Resources/Shader/MaterialTemplate.fs");

            fsTemplate = fsTemplate.Replace("{0}", diffuseColorCode);
            
            return fsTemplate;
        }
    }
}
