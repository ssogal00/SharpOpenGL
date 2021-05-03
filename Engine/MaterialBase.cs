using Core.Buffer;
using Core.OpenGLShader;
using Core.Texture;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Core.CustomAttribute;
using Engine;
using Engine.Rendering;


namespace Core.MaterialBase
{
    [StructLayout(LayoutKind.Explicit, Size = 128)]
    public struct CameraTransform
    {
        [FieldOffset(0), ExposeUI]
        public OpenTK.Mathematics.Matrix4 View;
        [FieldOffset(64), ExposeUI]
        public OpenTK.Mathematics.Matrix4 Proj;
    }

    [StructLayout(LayoutKind.Explicit, Size = 64)]
    public struct ModelTransform
    {
        [FieldOffset(0), ExposeUI]
        public OpenTK.Mathematics.Matrix4 Model;
    }

    public class MaterialBase : IBindable
    {
        protected ShaderProgram mMaterialProgram = null;
        
        protected VertexShader mVertexShader = null;
        protected FragmentShader mFragmentShader = null;
        protected GeometryShader mGeometryShader = null;
        protected ComputeShader mComputeShader = null;
        protected TesselControlShader mTesselControlShader = null;
        protected TesselEvalShader mTesselEvalShader = null;

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
            CompileAndLink(new Dictionary<ShaderType, string>
            {
                {ShaderType.VertexShader, vertexShaderCode},
                {ShaderType.FragmentShader, fragmentShaderCode}
            });

            Initialize();
        }

        // geometry shader
        public MaterialBase(string vertexShaderCode,  string fragmentShaderCode, string geometryShaderCode)
        {
            CompileAndLink(new Dictionary<ShaderType, string>
            {
                {ShaderType.VertexShader, vertexShaderCode},
                {ShaderType.FragmentShader, fragmentShaderCode},
                {ShaderType.GeometryShader, geometryShaderCode}
            });
            Initialize();
        }
        
        protected bool CompileAndLink(Dictionary<ShaderType, string> shaderTypeCodeMap)
        {
            if (shaderTypeCodeMap.ContainsKey(ShaderType.VertexShader))
            {
                mVertexShader = new VertexShader();

                if (mVertexShader.CompileShader(shaderTypeCodeMap[ShaderType.VertexShader]))
                {
                    mMaterialProgram.AttachShader(mVertexShader);
                }
            }

            if (shaderTypeCodeMap.ContainsKey(ShaderType.FragmentShader))
            {
                mFragmentShader = new FragmentShader();

                if (mFragmentShader.CompileShader(shaderTypeCodeMap[ShaderType.FragmentShader]))
                {
                    mMaterialProgram.AttachShader(mFragmentShader);
                }
            }

            if (shaderTypeCodeMap.ContainsKey(ShaderType.ComputeShader))
            {
                mComputeShader = new ComputeShader();
                if (mComputeShader.CompileShader(shaderTypeCodeMap[ShaderType.ComputeShader]))
                {
                    mMaterialProgram.AttachShader(mComputeShader);
                }
            }

            if (shaderTypeCodeMap.ContainsKey(ShaderType.GeometryShader))
            {
                mGeometryShader = new GeometryShader();
                if (mGeometryShader.CompileShader(shaderTypeCodeMap[ShaderType.GeometryShader]))
                {
                    mMaterialProgram.AttachShader(mGeometryShader);
                }
            }

            if (shaderTypeCodeMap.ContainsKey(ShaderType.TessControlShader))
            {
                mTesselControlShader = new TesselControlShader();
                if (mTesselControlShader.CompileShader(shaderTypeCodeMap[ShaderType.TessControlShader]))
                {
                    mMaterialProgram.AttachShader(mTesselControlShader);
                }
            }

            if (shaderTypeCodeMap.ContainsKey(ShaderType.TessEvaluationShader))
            {
                mTesselEvalShader = new TesselEvalShader();
                if (mTesselEvalShader.CompileShader(shaderTypeCodeMap[ShaderType.TessEvaluationShader]))
                {
                    mMaterialProgram.AttachShader(mTesselEvalShader);
                }
            }

            bool bSuccess = mMaterialProgram.LinkProgram(out CompileResult);

            if (bSuccess == false)
            {
                Console.Write("{0}", CompileResult);
            }

            Debug.Assert(bSuccess);

            return bSuccess;
        }

        public MaterialBase(Dictionary<ShaderType, string> shaderTypeCodeMap)
        {
            CompileAndLink(shaderTypeCodeMap);
            Initialize();
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
            var blocks = mMaterialProgram.GetActiveUnformBlockNameAndSizes();

            foreach (var (name,size) in blocks)
            {
                var uniformBuffer = new DynamicUniformBuffer(mMaterialProgram, name, size);
                mUniformBufferMap.Add(name, uniformBuffer);

                var bufferMembers = mMaterialProgram
                    .GetUniformVariableMetaDataListInBlock(uniformBuffer.UniformBufferBlockIndex)
                    .OrderBy(x => x.VariableOffset);

                foreach (var member in bufferMembers)
                {
                    mUniformBufferMembersMap.Add(member.VariableName, member);
                }
            }
        }

        protected virtual void BuildSamplerMap()
        {
            var samplerNames = mMaterialProgram.GetSampler2DNames();

            for (int i = 0; i < samplerNames.Count; ++i)
            {
                int index = mMaterialProgram.GetSampler2DUniformLocation(samplerNames[i]);
                mSamplerMap.Add(samplerNames[i], index);
            }
        }


        protected virtual void Initialize()
        {
            Debug.Assert(RenderingThread.IsInRenderingThread());

            BuildUniformBufferMap();

            BuildSamplerMap();

            mUniformVariableNames = mMaterialProgram.GetActiveUniformNames();

            foreach (var name in mUniformVariableNames)
            {
                var loc= mMaterialProgram.GetUniformLocation(name);
                mUniformVariableMap.Add(name, loc);
            }
        }

        public void SetTexture(int location, Core.Texture.TextureBase texture)
        {
            SetTextureByIndex(location, texture, Sampler.DefaultLinearSampler);
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

            var location = mSamplerMap[name];
            SetTextureByIndex(location, texture, sampler);
        }

        // set texture by index
        public void SetTextureByIndex(int index, Core.Texture.TextureBase texture, Sampler sampler)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + index);
            texture.Bind();
            sampler.BindSampler(TextureUnit.Texture0 + index);
            texture.BindShader(TextureUnit.Texture0 + index, index);
        }

        public void Bind()
        {
            mMaterialProgram.UseProgram();

            if (mUniformBufferMap != null)
            {
                foreach (var uniformBuffer in mUniformBufferMap)
                {
                    mMaterialProgram.BindUniformBlock(uniformBuffer.Key);
                    uniformBuffer.Value.Bind();
                }
            }
        }

        public void Unbind()
        {
            GL.UseProgram(0);

            if (mUniformBufferMap != null)
            {
                foreach (var uniformBuffer in mUniformBufferMap)
                {
                    uniformBuffer.Value.Unbind();
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
        
        public void SetUniformBufferValue<T>(string bufferName, T data) where T : struct
        {
            if(HasUniformBuffer(bufferName))
            {   
                mUniformBufferMap[bufferName].BufferData(data);
            }
        }


        public void SetUniformBufferMemberValue<TMember>(string bufferName, TMember data, int offset) where TMember : struct
        {
            if (HasUniformBuffer(bufferName))
            {   
                mUniformBufferMap[bufferName].BufferSubData<TMember>(data, offset);
            }
        }

        public void SetUniformVariable(string varName, float data, bool bCompareAndUpdateWhenDifferent = false)
        {
            // 
            if (mUniformVariableNames.Contains(varName))
            {
                mMaterialProgram.SetUniformVariable(varName, data);
            }

            InternalSetUnfiormBufferMember(varName,data);
        }

        public void SetUniformVariable(string varName, bool data, bool bChecked = false)
        {
            if (bChecked)
            {
                CheckUniformVariableExist(varName);
            }

            if (mUniformVariableNames.Contains(varName))
            {
                mMaterialProgram.SetUniformVariable(varName, data);
            }

            InternalSetUnfiormBufferMember(varName, data);
        }

        public void SetUniformVariable(string varName, int data, bool bChecked=false)
        {
            if (bChecked)
            {
                CheckUniformVariableExist(varName);
            }

            if (mUniformVariableNames.Contains(varName))
            {
                mMaterialProgram.SetUniformVariable(varName, data);
            }

            InternalSetUnfiormBufferMember(varName, data);
        }

        public void SetUniformVariable(string varName, float[] data, bool bChecked = false)
        {
            if (mUniformVariableNames.Contains(varName))
            {
                mMaterialProgram.SetUniformFloatArrayData(varName , data);
            }
        }

        public void SetUniformVariable(string varname, Vector3[] data)
        {
            if (mUniformVariableNames.Contains(varname))
            {
                mMaterialProgram.SetUniformVector3ArrayData(varname, data);
            }
        }

        public void SetUniformVariable(string varname, Vector4[] data)
        {
            if (mUniformVariableNames.Contains(varname))
            {
                mMaterialProgram.SetUniformVector4ArrayData(varname, data);
            }
        }

        public void SetUniformVariable(string varName, Vector2 data, bool bChecked = false)
        {
            if (bChecked)
            {
                CheckUniformVariableExist(varName);
            }

            if (mUniformVariableNames.Contains(varName))
            {
                mMaterialProgram.SetUniformVariable(varName, data);
            }

            InternalSetUnfiormBufferMember(varName, data);
        }

        private void InternalSetUnfiormBufferMember<T>(string variableName, T data) where T : struct
        {
            if (mUniformBufferMembersMap.ContainsKey(variableName))
            {
                var member = mUniformBufferMembersMap[variableName];
                mUniformBufferMap[member.UniformBlockName].BufferSubData(data, member.VariableOffset);
            }
        }
        
        public void SetUniformVariable(string varName, Vector3 data, bool bChecked = false)
        {
            if (bChecked)
            {
                CheckUniformVariableExist(varName);
            }

            if (mUniformVariableNames.Contains(varName))
            {
                mMaterialProgram.SetUniformVariable(varName, data);
            }

            InternalSetUnfiormBufferMember(varName, data);
        }


        public void SetUniformVariable(string varName, Vector4 data, bool bChecked=false)
        {
            if (bChecked)
            {
                CheckUniformVariableExist(varName);
            }

            if (mUniformVariableNames.Contains(varName))
            {
                mMaterialProgram.SetUniformVariable(varName, data);
            }

            InternalSetUnfiormBufferMember(varName, data);
        }

        public void SetUniformVariable(string varName, Matrix3 data, bool bChecked=false)
        {
            if (bChecked)
            {
                CheckUniformVariableExist(varName);
            }

            if (mUniformVariableNames.Contains(varName))
            {
                mMaterialProgram.SetUniformVariable(varName, data);
            }

            InternalSetUnfiormBufferMember(varName, data);
        }

        public void SetUniformVariable(string varName, Matrix4 data, bool bChecked = false)
        {
            if (bChecked)
            {
                CheckUniformVariableExist(varName);
            }

            if (mUniformVariableNames.Contains(varName))
            {
                mMaterialProgram.SetUniformVariable(varName, data);
            }
            InternalSetUnfiormBufferMember(varName, data);
        }

        private void CheckUniformVariableExist(string variablename)
        {
            Debug.Assert(mUniformVariableNames.Contains(variablename));
        }

        public void SetUniformVector2ArrayData(string varName, float[] data)
        {
            Debug.Assert(mUniformVariableNames.Contains(varName));
            mMaterialProgram.SetUniformVector2ArrayData(varName, data);
        }

        public void SetUniformVector3ArrayData(string varName, float[] data)
        {
            Debug.Assert(mUniformVariableNames.Contains(varName));
            mMaterialProgram.SetUniformVector3ArrayData(varName, data);
        }

        public void SetUniformVector4ArrayData(string varName, float[] data)
        {
            Debug.Assert(mUniformVariableNames.Contains(varName));
            mMaterialProgram.SetUniformVector4ArrayData(varName, data);
        }

        public List<VertexAttribute> GetVertexAttributes()
        {
            if (mMaterialProgram.ProgramLinked)
            {
                return mMaterialProgram.GetActiveVertexAttributeList();
            }

            return new List<VertexAttribute>();
        }

        public List<ActiveAttribType> GetVertexAttributeTypeList()
        {
            if (mMaterialProgram.ProgramLinked)
            {
                return mMaterialProgram.GetActiveVertexAttributeList().Select(x=>x.AttributeType).ToList();
            }
            return new List<ActiveAttribType>();
        }

        protected CameraTransform mCameraTransform = new CameraTransform();
        protected ModelTransform mModelTransform = new ModelTransform();

        protected Dictionary<string, DynamicUniformBuffer> mUniformBufferMap = new Dictionary<string, DynamicUniformBuffer>(StringComparer.InvariantCultureIgnoreCase);
        protected Dictionary<string, UniformVariableMetaData> mUniformBufferMembersMap = new Dictionary<string, UniformVariableMetaData>(StringComparer.InvariantCultureIgnoreCase);
        protected Dictionary<string, int> mUniformVariableMap = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
        protected Dictionary<string, int> mSamplerMap = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

        protected List<string> mUniformVariableNames = null;
    }
}
