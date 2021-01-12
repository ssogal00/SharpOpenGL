using Core.Buffer;
using Core.OpenGLShader;
using Core.Texture;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace Core.MaterialBase
{
    public class MaterialBase : Core.AssetBase, IBindable
    {
        protected ShaderProgram mMaterialProgram = null;
        protected Core.OpenGLShader.VertexShader mVertexShader = null;
        protected Core.OpenGLShader.FragmentShader mFragmentShader = null;
        protected Core.OpenGLShader.TesselControlShader tesselControlShader = null;
        protected Core.OpenGLShader.TesselEvalShader tesselEvaluationShader = null;

        protected string VertexShaderCode = "";
        protected string FragmentShaderCode = "";

        public static readonly string StageOutputName = "stage_out";
        public static readonly string StageInputName = "stage_in";

        protected string CompileResult = "";



        public MaterialBase(ShaderProgram program)
        {
            mMaterialProgram = program;
            Initialize();
        }

        public MaterialBase(string vertexShaderCode, string fragmentShaderCode, 
            List<Tuple<string, string>> vsDefines, List<Tuple<string, string>> fsDefines) 
        {
            mVertexShader = new VertexShader();
            mFragmentShader = new FragmentShader();

            mMaterialProgram = new Core.OpenGLShader.ShaderProgram();

            mVertexShader.CompileShader(vertexShaderCode, vsDefines);
            mFragmentShader.CompileShader(fragmentShaderCode, fsDefines);

            mMaterialProgram.AttachShader(mVertexShader);
            mMaterialProgram.AttachShader(mFragmentShader);

            bool bSuccess = mMaterialProgram.LinkProgram(out CompileResult);
            if (bSuccess == false)
            {
                Console.Write("{0}", CompileResult);
            }
            Debug.Assert(bSuccess == true);

            Initialize();
        }

        public MaterialBase(string vertexShaderCode, string fragmentShaderCode)
        {
            mVertexShader = new VertexShader();
            mFragmentShader = new FragmentShader();

            mMaterialProgram = new Core.OpenGLShader.ShaderProgram();
            
            mVertexShader.CompileShader(vertexShaderCode);
            mFragmentShader.CompileShader(fragmentShaderCode);

            mMaterialProgram.AttachShader(mVertexShader);
            mMaterialProgram.AttachShader(mFragmentShader);

            bool bSuccess = mMaterialProgram.LinkProgram(out CompileResult);

            if (bSuccess == false)
            {
                Console.Write("{0}", CompileResult);
            }

            Debug.Assert(bSuccess == true);

            Initialize();
        }

        public MaterialBase(string vertexShaderCode, string fragmentShaderCode, string tesselControlShaderCode, string tesselEvalShaderCode)
        {
            mVertexShader = new VertexShader();
            mFragmentShader = new FragmentShader();
            tesselControlShader = new TesselControlShader();
            tesselEvaluationShader = new TesselEvalShader();

            mMaterialProgram = new Core.OpenGLShader.ShaderProgram();

            mVertexShader.CompileShader(vertexShaderCode);
            mFragmentShader.CompileShader(fragmentShaderCode);
            tesselControlShader.CompileShader(tesselControlShaderCode);
            tesselEvaluationShader.CompileShader(tesselEvalShaderCode);

            mMaterialProgram.AttachShader(mVertexShader);
            mMaterialProgram.AttachShader(mFragmentShader);
            mMaterialProgram.AttachShader(tesselControlShader);
            mMaterialProgram.AttachShader(tesselEvaluationShader);

            bool bSuccess = mMaterialProgram.LinkProgram(out CompileResult);

            Debug.Assert(bSuccess == true);

            Initialize();
        }


        public MaterialBase()
        {
        }

        protected virtual void CleanUpUniformBufferMap()
        {
            if(mUniformBufferMap != null)
            {
                foreach(var buffer in mUniformBufferMap)
                {
                    buffer.Value.Dispose();
                }
                mUniformBufferMap.Clear();
            }
        }

        protected virtual void BuildUniformBufferMap()
        {
            var names = mMaterialProgram.GetActiveUniformBlockNames();

            if (names.Count > 0)
            {
                mUniformBufferMap = new Dictionary<string, DynamicUniformBuffer>();
            }

            foreach (var name in names)
            {
                var uniformBuffer = new DynamicUniformBuffer(mMaterialProgram, name);
                mUniformBufferMap.Add(name, uniformBuffer);
            }
        }

        protected virtual void BuildSamplerMap()
        {
            var samplerNames = mMaterialProgram.GetSampler2DNames();

            if (samplerNames.Count > 0)
            {
                mSamplerMap = new Dictionary<string, TextureUnit>();
                
            }

            for (int i = 0; i < samplerNames.Count; ++i)
            {
                mSamplerMap.Add(samplerNames[i], TextureUnit.Texture0 + i);
            }
        }

        protected virtual void Initialize()
        {
            BuildUniformBufferMap();

            BuildSamplerMap();

            var attrList = mMaterialProgram.GetActiveVertexAttributeList();

            mUniformVariableNames = mMaterialProgram.GetActiveUniformNames();
        }

        public void SetTexture(string name, Core.Texture.TextureBase texture)
        {
            SetTexture(name, texture, Sampler.DefaultLinearSampler);
        }

        public void SetTexture(string name, Core.Texture.TextureBase texture, Sampler sampler)
        {
            if (mSamplerMap == null)
            {
                return;
            }

            if (mSamplerMap.ContainsKey(name) == false)
            {
                return;
            }

            var Loc = mMaterialProgram.GetSampler2DUniformLocation(name);
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
            mMaterialProgram.UseProgram();

            if (mUniformBufferMap != null)
            {
                foreach (var uniformBuffer in mUniformBufferMap)
                {
                    mMaterialProgram.BindUniformBlock(uniformBuffer.Key);
                }
            }
        }

        public string GetCompileResult()
        {
            return CompileResult;
        }

        public bool HasUniformBuffer(string uniformBufferName)
        {
            if(mUniformBufferMap.ContainsKey(uniformBufferName))
            {
                return true;
            }

            return false;
        }
        
        public void SetUniformBufferValue<T>(string bufferName, ref T data) where T : struct
        {
            if(HasUniformBuffer(bufferName))
            {   
                mUniformBufferMap[bufferName].BufferData(ref data);
            }
        }

        public void SetUniformBufferMemberValue<TMember>(string bufferName, ref TMember data, int offset) where TMember : struct
        {
            if (HasUniformBuffer(bufferName))
            {   
                mUniformBufferMap[bufferName].BufferSubData<TMember>(ref data, offset);
            }
        }

        public void SetUniformVarData(string varName, float data, bool bChecked = false)
        {
            if (bChecked)
            {
                CheckUniformVariableExist(varName);
            }

            if (mUniformVariableNames.Contains(varName))
            {
                mMaterialProgram.SetUniformVarData(varName, data);
            }
        }

        public void SetUniformVarData(string varName, bool data, bool bChecked = false)
        {
            if (bChecked)
            {
                CheckUniformVariableExist(varName);
            }

            if (mUniformVariableNames.Contains(varName))
            {
                mMaterialProgram.SetUniformVarData(varName, data);
            }
        }

        public void SetUniformVarData(string varName, int data, bool bChecked=false)
        {
            if (bChecked)
            {
                CheckUniformVariableExist(varName);
            }

            if (mUniformVariableNames.Contains(varName))
            {
                mMaterialProgram.SetUniformVarData(varName, data);
            }
        }

        public void SetUniformVarData(string varName, float[] data, bool bChecked = false)
        {
            if (mUniformVariableNames.Contains(varName))
            {
                mMaterialProgram.SetUniformFloatArrayData(varName , ref data);
            }
        }

        public void SetUniformVarData(string varName, ref float[] data, bool bChecked = false)
        {
            if (mUniformVariableNames.Contains(varName))
            {
                mMaterialProgram.SetUniformFloatArrayData(varName, ref data);
            }
        }

        public void SetUniformVarData(string varName, double[] data, bool bChecked = false)
        {
            if (mUniformVariableNames.Contains(varName))
            {
                mMaterialProgram.SetUniformDoubleArrayData(varName, ref data);
            }
        }

        public void SetUniformVarData(string varname, ref Vector2[] data)
        {
            if (mUniformVariableNames.Contains(varname))
            {
                mMaterialProgram.SetUniformVector2ArrayData(varname, ref data);
            }
        }

        public void SetUniformVarData(string varname, ref Vector3[] data)
        {
            if (mUniformVariableNames.Contains(varname))
            {
                mMaterialProgram.SetUniformVector3ArrayData(varname, ref data);
            }
        }

        public void SetUniformVarData(string varname, ref Vector4[] data)
        {
            if (mUniformVariableNames.Contains(varname))
            {
                mMaterialProgram.SetUniformVector4ArrayData(varname, ref data);
            }
        }

        public void SetUniformVarData(string varName, Vector2 data, bool bChecked = false)
        {
            if (bChecked)
            {
                CheckUniformVariableExist(varName);
            }

            if (mUniformVariableNames.Contains(varName))
            {
                mMaterialProgram.SetUniformVarData(varName, data);
            }
        }

        public void SetUniformVarData(string varName, ref Vector2 data, bool bChecked = false)
        {
            if (bChecked)
            {
                CheckUniformVariableExist(varName);
            }

            if (mUniformVariableNames.Contains(varName))
            {
                mMaterialProgram.SetUniformVarData(varName, data);
            }
        }        

        public void SetUniformVarData(string varName, Vector3 data, bool bChecked = false)
        {
            if (bChecked)
            {
                CheckUniformVariableExist(varName);
            }

            if (mUniformVariableNames.Contains(varName))
            {
                mMaterialProgram.SetUniformVarData(varName, data);
            }
        }

        public void SetUniformVarData(string varName, ref Vector3 data, bool bChecked = false)
        {
            if (bChecked)
            {
                CheckUniformVariableExist(varName);
            }

            if (mUniformVariableNames.Contains(varName))
            {
                mMaterialProgram.SetUniformVarData(varName, ref data);
            }
        }

        public void SetUniformVarData(string varName, Vector4 data, bool bChecked=false)
        {
            if (bChecked)
            {
                CheckUniformVariableExist(varName);
            }

            if (mUniformVariableNames.Contains(varName))
            {
                mMaterialProgram.SetUniformVarData(varName, data);
            }
        }

        public void SetUniformVarData(string varName, Matrix3 data, bool bChecked=false)
        {
            if (bChecked)
            {
                CheckUniformVariableExist(varName);
            }

            if (mUniformVariableNames.Contains(varName))
            {
                mMaterialProgram.SetUniformVarData(varName, data);
            }
        }

        private void CheckUniformVariableExist(string variablename)
        {
            Debug.Assert(mUniformVariableNames.Contains(variablename));
        }

        public void SetUniformVarData(string varName, Matrix4 data, bool bChecked = false)
        {
            if (bChecked)
            {
                CheckUniformVariableExist(varName);
            }

            if (mUniformVariableNames.Contains(varName))
            {
                mMaterialProgram.SetUniformVarData(varName, data);
            }
        }

        public void SetUniformVarData(string varName, ref Matrix4 data, bool bChecked = false)
        {
            if (bChecked)
            {
                CheckUniformVariableExist(varName);
            }

            if (mUniformVariableNames.Contains(varName))
            {
                mMaterialProgram.SetUniformVarData(varName, ref data);
            }
        }

        public void SetUniformVector2ArrayData(string varName, ref float[] data)
        {
            Debug.Assert(mUniformVariableNames.Contains(varName));
            mMaterialProgram.SetUniformVector2ArrayData(varName, ref data);
        }

        public void SetUniformVector3ArrayData(string varName, ref float[] data)
        {
            Debug.Assert(mUniformVariableNames.Contains(varName));
            mMaterialProgram.SetUniformVector3ArrayData(varName, ref data);
        }

        public void SetUniformVector4ArrayData(string varName, ref float[] data)
        {
            Debug.Assert(mUniformVariableNames.Contains(varName));
            mMaterialProgram.SetUniformVector4ArrayData(varName, ref data);
        }

        public List<VertexAttribute> GetVertexAttributes()
        {
            if (mMaterialProgram.ProgramLinked)
            {
                return mMaterialProgram.GetActiveVertexAttributeList();
            }

            return new List<VertexAttribute>();
        }
        

        protected Dictionary<string, DynamicUniformBuffer> mUniformBufferMap = null;
        protected Dictionary<string, TextureUnit> mSamplerMap = null;
        protected List<string> mUniformVariableNames = null;
    }
}
