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

        public void Compile(NetworkViewModel shaderNetwork)
        {
            var uniformVarCode = GetUniformVariableCode(shaderNetwork);

            var fsTemplate = GetFragmentShaderCode();
            var vsTemplate = GetVertexShaderCode();

            if (uniformVarCode.Length > 0)
            {
                fsTemplate = fsTemplate.Replace("{uniformVariableDeclaration}", uniformVarCode);
            }
            else
            {
                fsTemplate = fsTemplate.Replace("{uniformVariableDeclaration}", "");
            }

            var diffuseColorCode = shaderNetwork.CurrentSelectedNode.ToExpression();

            if(diffuseColorCode.Length > 0)
            {
                fsTemplate = fsTemplate.Replace("{diffuseColorCode}", diffuseColorCode);
            }            

            if(Compile(vsTemplate, fsTemplate))
            {
                Initialize();
            }
        }
        
        protected string GetUniformVariableCode(NetworkViewModel shaderNetwork)
        {
            string result = string.Empty;

            foreach(var item in shaderNetwork.Nodes)
            {
                if (item is FloatUniformVariableNode)
                {
                    FloatUniformVariableNode node = (FloatUniformVariableNode)item;                    
                    result += string.Format("uniform float {0};\n", node.UniformVariableName);
                }                
            }

            return result;
        }

        protected bool Compile(string vsCode, string fsCode)
        {
            string errorlog;            

            bool bVSCompileSuccess = VSShader.CompileShader(vsCode, out errorlog);
            bool bFSCompileSuccess = FSShader.CompileShader(fsCode, out errorlog);

            MaterialProgram = new Core.OpenGLShader.ShaderProgram();

            MaterialProgram.AttachShader(VSShader);
            MaterialProgram.AttachShader(FSShader);

            string linkResult;

            bool bLinkResult = MaterialProgram.LinkProgram(out linkResult);

            lastCompileSuccess = bVSCompileSuccess && bFSCompileSuccess && bLinkResult;

            return lastCompileSuccess;
        }

        public void Use()
        {
            if(MaterialProgram != null && MaterialProgram.IsProgramLinked())
            {
                MaterialProgram.UseProgram();
            }
        }

        protected string GetVertexShaderCode()
        {
            string vsTemplate = File.ReadAllText("./Resources/Shader/MaterialTemplate.vs");

            return vsTemplate;
        }

        protected string GetFragmentShaderCode()
        {
            string fsTemplate = File.ReadAllText("./Resources/Shader/MaterialTemplate.fs");                        
            
            return fsTemplate;
        }
    }
}
