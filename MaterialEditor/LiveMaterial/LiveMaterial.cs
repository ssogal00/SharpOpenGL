using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CompiledMaterial;
using Core.OpenGLShader;
using Core.MaterialBase;
using System.IO;
using Core;
using Core.Texture;

namespace MaterialEditor
{
    public class LiveMaterial : MaterialBase
    {
        protected bool lastCompileSuccess = false;

        protected string diffuseColorCode = string.Empty;

        protected Dictionary<string, Core.Texture.Texture2D> textureMap = new Dictionary<string, Core.Texture.Texture2D>();
        
        public LiveMaterial()
        {
            vertexShader = new VertexShader();
            fragmentShader = new FragmentShader();
            tesselControlShader = new TesselControlShader();
            tesselEvaluationShader = new TesselEvalShader();
        }

        public bool IsValid()
        {
            return lastCompileSuccess;
        }

        public override void Setup()
        {
            base.Setup();

            foreach (var texture in textureMap)
            {
                this.SetTexture(texture.Key, texture.Value);
            }
        }

        protected void BuildTextureMap(NetworkViewModel shaderNetwork)
        {
            foreach(var item in shaderNetwork.Nodes)
            {
                if(item is TextureParamNode)
                {
                    var textureNode = (TextureParamNode)item;

                    var texturePath = textureNode.ImageSource.UriSource.AbsolutePath;

                    if(File.Exists(texturePath))
                    {
                        Texture2D newTexture = new Texture2D();
                        newTexture.Load(texturePath);
                        textureMap.Add(textureNode.UniformName, newTexture);
                    }                    
                }
            }
        }

        protected void CleanUpTextureMap()
        {
            foreach(var item in textureMap)
            {
                item.Value.Dispose();
            }

            textureMap.Clear();
        }

        public void Compile(NetworkViewModel shaderNetwork)
        {
            var uniformVarCode = GetUniformVariableCode(shaderNetwork);
            var sampler2DCode = GetTextureUniformVariableCode(shaderNetwork);

            var fsTemplate = GetFragmentShaderCode();
            var vsTemplate = GetVertexShaderCode();
            
            fsTemplate = fsTemplate.Replace("{uniformVariableDeclaration}", uniformVarCode);

            if(sampler2DCode.Length > 0)
            {
                fsTemplate = fsTemplate.Replace("{sampler2DVariableDeclaration}", sampler2DCode);
            }
            else
            {
                fsTemplate = fsTemplate.Replace("{sampler2DVariableDeclaration}", "");
            }

            var resultNode = (ResultNode)shaderNetwork.CurrentSelectedNode;

            var diffuseColorCode = resultNode.GetDiffuseColorCode();

            if(diffuseColorCode.Length > 0)
            {
                fsTemplate = fsTemplate.Replace("{diffuseColorCode}", diffuseColorCode);
            }

            var normalColorCode = resultNode.GetNormalColorCode();

            if(normalColorCode.Length > 0)
            {
                fsTemplate = fsTemplate.Replace("{normalColorCode}", normalColorCode);
            }

            var metallicCode = resultNode.GetMetallicCode();

            if (metallicCode.Length > 0)
            {
                fsTemplate = fsTemplate.Replace("{metallicCode}", metallicCode);
            }

            var roughnessCode = resultNode.GetRoughnessCode();

            if (roughnessCode.Length > 0)
            {
                fsTemplate = fsTemplate.Replace("{roughnessCode}", roughnessCode);
            }

            // if compile succeeds
            if(Compile(vsTemplate, fsTemplate))
            {
                //
                CleanUpUniformBufferMap();

                CleanUpTextureMap();

                BuildTextureMap(shaderNetwork);

                // 
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

        protected string GetTextureUniformVariableCode(NetworkViewModel shaderNetwork)
        {
            string result = string.Empty;

            foreach(var item in shaderNetwork.Nodes)
            {
                if(item is TextureParamNode)
                {
                    TextureParamNode node = (TextureParamNode)item;
                    result += string.Format("uniform sampler2D {0};\n", node.UniformName);
                }
            }

            return result;
        }

        protected bool Compile(string vsCode, string fsCode)
        {
            string errorlog = "";            

            bool bVSCompileSuccess = vertexShader.CompileShader(vsCode, out errorlog);
            bool bFSCompileSuccess = fragmentShader.CompileShader(fsCode, out errorlog);

            string tcCode = File.ReadAllText("./Resources/Shader/TesselControl.shader");
            bool bTCCompileSuccess = tesselControlShader.CompileShader(tcCode, out errorlog);

            string teCode = File.ReadAllText("./Resources/Shader/TesselEvaluation.shader");
            bool bTECompileSuccess = tesselEvaluationShader.CompileShader(teCode, out errorlog);
            

            MaterialProgram = new Core.OpenGLShader.ShaderProgram();

            MaterialProgram.AttachShader(vertexShader);
            MaterialProgram.AttachShader(fragmentShader);

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
