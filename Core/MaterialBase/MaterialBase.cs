using System;
using Core.Buffer;
using Core.OpenGLShader;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Channels;
using Core.Texture;
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

            if (bSuccess == false)
            {
                Console.Write("{0}", CompileResult);
            }

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
            SetTexture(name, texture, Sampler.DefaultLinearSampler);
        }

        public void SetTexture(string name, Core.Texture.TextureBase texture, Sampler sampler)
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
            sampler.BindSampler(TextureUnit.Texture0 + Loc);
            texture.BindShader(TextureUnit.Texture0 + Loc, Loc);
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
                UniformBufferMap[bufferName].BufferData(ref data);
            }
        }

        public void SetUniformBufferMemberValue<TMember>(string bufferName, ref TMember data, int offset) where TMember : struct
        {
            if (HasUniformBuffer(bufferName))
            {   
                UniformBufferMap[bufferName].BufferSubData<TMember>(ref data, offset);
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

        public void SetUniformVarData(string varName, bool data, bool bChecked = false)
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

        public void SetUniformVarData(string varName, float[] data, bool bChecked = false)
        {
            if (UniformVariableNames.Contains(varName))
            {
                MaterialProgram.SetUniformFloatArrayData(varName , ref data);
            }
        }

        public void SetUniformVarData(string varName, ref float[] data, bool bChecked = false)
        {
            if (UniformVariableNames.Contains(varName))
            {
                MaterialProgram.SetUniformFloatArrayData(varName, ref data);
            }
        }

        public void SetUniformVarData(string varName, double[] data, bool bChecked = false)
        {
            if (UniformVariableNames.Contains(varName))
            {
                MaterialProgram.SetUniformDoubleArrayData(varName, ref data);
            }
        }

        public void SetUniformVarData(string varname, ref OpenTK.Vector2[] data)
        {
            if (UniformVariableNames.Contains(varname))
            {
                MaterialProgram.SetUniformVector2ArrayData(varname, ref data);
            }
        }

        public void SetUniformVarData(string varname, ref OpenTK.Vector3[] data)
        {
            if (UniformVariableNames.Contains(varname))
            {
                MaterialProgram.SetUniformVector3ArrayData(varname, ref data);
            }
        }

        public void SetUniformVarData(string varname, ref OpenTK.Vector4[] data)
        {
            if (UniformVariableNames.Contains(varname))
            {
                MaterialProgram.SetUniformVector4ArrayData(varname, ref data);
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

        public void SetUniformVector2ArrayData(string varName, ref float[] data)
        {
            Debug.Assert(UniformVariableNames.Contains(varName));
            MaterialProgram.SetUniformVector2ArrayData(varName, ref data);
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

        public List<VertexAttribute> GetVertexAttributes()
        {
            if (MaterialProgram.ProgramLinked)
            {
                return MaterialProgram.GetActiveVertexAttributeList();
            }

            return new List<VertexAttribute>();
        }
        

        protected Dictionary<string, DynamicUniformBuffer> UniformBufferMap = null;
        protected Dictionary<string, TextureUnit> SamplerMap = null;
        protected List<string> UniformVariableNames = null;
    }
}
