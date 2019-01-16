using Core.Buffer;
using Core.OpenGLShader;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Channels;
using OpenTK.Graphics.OpenGL;

namespace Core.MaterialBase
{
    public class MaterialBase : Core.AssetBase, IBindable
    {
        protected ShaderProgram MaterialProgram = null;
        protected Core.OpenGLShader.VertexShader vertexShader = null;
        protected Core.OpenGLShader.FragmentShader fragmentShader = null;
        protected Core.OpenGLShader.TesselControlShader tesselControlShader = null;
        protected Core.OpenGLShader.TesselEvalShader tesselEvaluationShader = null;

        protected string VertexShaderCode = "";
        protected string FragmentShaderCode = "";

        public static readonly string StageOutputName = "stage_out";
        public static readonly string StageInputName = "stage_in";

        protected string CompileResult = "";

        public MaterialBase(ShaderProgram program)
        {
            MaterialProgram = program;
            Initialize();
        }

        public MaterialBase(string vertexShaderCode, string fragmentShaderCode)
        {
            vertexShader = new VertexShader();
            fragmentShader = new FragmentShader();

            MaterialProgram = new Core.OpenGLShader.ShaderProgram();

            vertexShader.CompileShader(vertexShaderCode);
            fragmentShader.CompileShader(fragmentShaderCode);

            MaterialProgram.AttachShader(vertexShader);
            MaterialProgram.AttachShader(fragmentShader);

            bool bSuccess = MaterialProgram.LinkProgram(out CompileResult);

            Debug.Assert(bSuccess == true);

            Initialize();
        }

        public MaterialBase(string vertexShaderCode, string fragmentShaderCode, string tesselControlShaderCode, string tesselEvalShaderCode)
        {
            vertexShader = new VertexShader();
            fragmentShader = new FragmentShader();
            tesselControlShader = new TesselControlShader();
            tesselEvaluationShader = new TesselEvalShader();

            MaterialProgram = new Core.OpenGLShader.ShaderProgram();

            vertexShader.CompileShader(vertexShaderCode);
            fragmentShader.CompileShader(fragmentShaderCode);
            tesselControlShader.CompileShader(tesselControlShaderCode);
            tesselEvaluationShader.CompileShader(tesselEvalShaderCode);

            MaterialProgram.AttachShader(vertexShader);
            MaterialProgram.AttachShader(fragmentShader);
            MaterialProgram.AttachShader(tesselControlShader);
            MaterialProgram.AttachShader(tesselEvaluationShader);

            bool bSuccess = MaterialProgram.LinkProgram(out CompileResult);

            Debug.Assert(bSuccess == true);

            Initialize();
        }


        public MaterialBase()
        {
        }

        protected virtual void CleanUpUniformBufferMap()
        {
            if(UniformBufferMap != null)
            {
                foreach(var buffer in UniformBufferMap)
                {
                    buffer.Value.Dispose();
                }
                UniformBufferMap.Clear();
            }
        }

        protected virtual void BuildUniformBufferMap()
        {
            var names = MaterialProgram.GetActiveUniformBlockNames();

            if (names.Count > 0)
            {
                UniformBufferMap = new Dictionary<string, DynamicUniformBuffer>();
            }

            foreach (var name in names)
            {
                var uniformBuffer = new DynamicUniformBuffer(MaterialProgram, name);
                UniformBufferMap.Add(name, uniformBuffer);
            }
        }

        protected virtual void BuildSamplerMap()
        {
            var samplerNames = MaterialProgram.GetSampler2DNames();

            if (samplerNames.Count > 0)
            {
                SamplerMap = new Dictionary<string, TextureUnit>();
                
            }

            for (int i = 0; i < samplerNames.Count; ++i)
            {
                SamplerMap.Add(samplerNames[i], TextureUnit.Texture0 + i);
            }
        }

        protected virtual void Initialize()
        {
            BuildUniformBufferMap();

            BuildSamplerMap();

            var attrList = MaterialProgram.GetActiveVertexAttributeList();

            UniformVariableNames = MaterialProgram.GetActiveUniformNames();
        }

        public void SetTexture(string name, Core.Texture.TextureBase texture)
        {
            if (SamplerMap == null)
            {
                return;
            }

            if (SamplerMap.ContainsKey(name) == false)
            {
                return;
            }
            
            var Loc = MaterialProgram.GetSampler2DUniformLocation(name);
            GL.ActiveTexture(TextureUnit.Texture0 + Loc);
            texture.Bind();
            texture.BindShader(TextureUnit.Texture0 + Loc, Loc);
        }

        public void SetTexture(string name, int TextureObject)
        {
            if(SamplerMap.ContainsKey(name) == false)
            {
                return;
            }
            
            var textureUnitToBind = SamplerMap[name];

            GL.ActiveTexture(textureUnitToBind);
            GL.BindTexture(TextureTarget.Texture2D, TextureObject);
            Core.Texture.Sampler.DefaultLinearSampler.BindSampler(textureUnitToBind);
            var SamplerLoc = MaterialProgram.GetSampler2DUniformLocation(name);
            GL.ProgramUniform1(MaterialProgram.ProgramObject, SamplerLoc, 0);
        }

        public void Bind()
        {
            Setup();
        }

        public void Unbind()
        {
            GL.UseProgram(0);
        }

        public virtual void Setup()
        {
            MaterialProgram.UseProgram();

            if (UniformBufferMap != null)
            {
                foreach (var uniformBuffer in UniformBufferMap)
                {
                    MaterialProgram.BindUniformBlock(uniformBuffer.Key);
                }
            }
        }

        public string GetCompileResult()
        {
            return CompileResult;
        }

        public bool HasUniformBuffer(string uniformBufferName)
        {
            if(UniformBufferMap.ContainsKey(uniformBufferName))
            {
                return true;
            }

            return false;
        }
        
        public void SetUniformBufferValue<T>(string bufferName, ref T data) where T : struct
        {
            if(HasUniformBuffer(bufferName))
            {   
                UniformBufferMap[bufferName].Bind();
                UniformBufferMap[bufferName].BufferData(ref data);
            }
        }

        public void SetUniformVarData(string varName, float data, bool bChecked = false)
        {
            if (bChecked)
            {
                CheckUniformVariableExist(varName);
            }

            if (UniformVariableNames.Contains(varName))
            {
                MaterialProgram.SetUniformVarData(varName, data);
            }
        }

        public void SetUniformVarData(string varName, int data, bool bChecked=false)
        {
            if (bChecked)
            {
                CheckUniformVariableExist(varName);
            }

            if (UniformVariableNames.Contains(varName))
            {
                MaterialProgram.SetUniformVarData(varName, data);
            }
        }

        public void SetUniformVarData(string varName, OpenTK.Vector2 data, bool bChecked = false)
        {
            if (bChecked)
            {
                CheckUniformVariableExist(varName);
            }

            if (UniformVariableNames.Contains(varName))
            {
                MaterialProgram.SetUniformVarData(varName, data);
            }
        }

        public void SetUniformVarData(string varName, ref OpenTK.Vector2 data, bool bChecked = false)
        {
            if (bChecked)
            {
                CheckUniformVariableExist(varName);
            }

            if (UniformVariableNames.Contains(varName))
            {
                MaterialProgram.SetUniformVarData(varName, data);
            }
        }        

        public void SetUniformVarData(string varName, OpenTK.Vector3 data, bool bChecked = false)
        {
            if (bChecked)
            {
                CheckUniformVariableExist(varName);
            }

            if (UniformVariableNames.Contains(varName))
            {
                MaterialProgram.SetUniformVarData(varName, data);
            }
        }

        public void SetUniformVarData(string varName, ref OpenTK.Vector3 data, bool bChecked = false)
        {
            if (bChecked)
            {
                CheckUniformVariableExist(varName);
            }

            if (UniformVariableNames.Contains(varName))
            {
                MaterialProgram.SetUniformVarData(varName, ref data);
            }
        }

        public void SetUniformVarData(string varName, OpenTK.Vector4 data, bool bChecked=false)
        {
            if (bChecked)
            {
                CheckUniformVariableExist(varName);
            }

            if (UniformVariableNames.Contains(varName))
            {
                MaterialProgram.SetUniformVarData(varName, data);
            }
        }

        public void SetUniformVarData(string varName, OpenTK.Matrix3 data, bool bChecked=false)
        {
            if (bChecked)
            {
                CheckUniformVariableExist(varName);
            }

            if (UniformVariableNames.Contains(varName))
            {
                MaterialProgram.SetUniformVarData(varName, data);
            }
        }

        private void CheckUniformVariableExist(string variablename)
        {
            Debug.Assert(UniformVariableNames.Contains(variablename));
        }

        public void SetUniformVarData(string varName, OpenTK.Matrix4 data, bool bChecked = false)
        {
            if (bChecked)
            {
                CheckUniformVariableExist(varName);
            }

            if (UniformVariableNames.Contains(varName))
            {
                MaterialProgram.SetUniformVarData(varName, data);
            }
        }

        public void SetUniformVarData(string varName, ref OpenTK.Matrix4 data, bool bChecked = false)
        {
            if (bChecked)
            {
                CheckUniformVariableExist(varName);
            }

            if (UniformVariableNames.Contains(varName))
            {
                MaterialProgram.SetUniformVarData(varName, ref data);
            }
        }

        public void SetUniformVector3ArrayData(string varName, ref float[] data)
        {
            Debug.Assert(UniformVariableNames.Contains(varName));
            MaterialProgram.SetUniformVector3ArrayData(varName, ref data);
        }

        public void SetUniformVector4ArrayData(string varName, ref float[] data)
        {
            Debug.Assert(UniformVariableNames.Contains(varName));
            MaterialProgram.SetUniformVector4ArrayData(varName, ref data);
        }


        protected Dictionary<string, DynamicUniformBuffer> UniformBufferMap = null;
        protected Dictionary<string, TextureUnit> SamplerMap = null;
        protected List<string> UniformVariableNames = null;
    }
}
