using Core.Buffer;
using Core.OpenGLShader;
using System.Collections.Generic;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Core.MaterialBase
{
    public class MaterialBase
    {
        protected ShaderProgram MaterialProgram = null;
        protected Core.OpenGLShader.VertexShader VSShader = new Core.OpenGLShader.VertexShader();
        protected Core.OpenGLShader.FragmentShader FSShader = new Core.OpenGLShader.FragmentShader();
        protected string CompileResult = "";

        public MaterialBase(string vertexShaderCode, string fragmentShaderCode)
        {
            MaterialProgram = new Core.OpenGLShader.ShaderProgram();

            VSShader.CompileShader(vertexShaderCode);
            FSShader.CompileShader(fragmentShaderCode);

            MaterialProgram.AttachShader(VSShader);
            MaterialProgram.AttachShader(FSShader);

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

            var textureUnitToBind = SamplerMap[name];
            GL.ActiveTexture(textureUnitToBind);
            texture.Bind();
            var Loc = MaterialProgram.GetSampler2DUniformLocation(name);
            texture.BindShader(textureUnitToBind, Loc);
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

        public virtual void Setup()
        {
            MaterialProgram.UseProgram();

            if (UniformBufferMap != null)
            {
                int index = 0;
                foreach (var uniformBuffer in UniformBufferMap)
                {
                    uniformBuffer.Value.BindBufferBase(index++);
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

        public void SetUniformVarData(string varName, float data)
        {
            if (UniformVariableNames.Contains(varName))
            {
                MaterialProgram.SetUniformVarData(varName, data);
            }
        }

        public void SetUniformVarData(string varName, int data)
        {
            if (UniformVariableNames.Contains(varName))
            {
                MaterialProgram.SetUniformVarData(varName, data);
            }
        }

        public void SetUniformVarData(string varName, OpenTK.Vector2 data)
        {
            if (UniformVariableNames.Contains(varName))
            {
                MaterialProgram.SetUniformVarData(varName, data);
            }
        }

        public void SetUniformVarData(string varName, ref OpenTK.Vector2 data)
        {
            if (UniformVariableNames.Contains(varName))
            {
                MaterialProgram.SetUniformVarData(varName, data);
            }
        }        

        public void SetUniformVarData(string varName, OpenTK.Vector3 data)
        {
            if (UniformVariableNames.Contains(varName))
            {
                MaterialProgram.SetUniformVarData(varName, data);
            }
        }

        public void SetUniformVarData(string varName, OpenTK.Vector4 data)
        {
            if (UniformVariableNames.Contains(varName))
            {
                MaterialProgram.SetUniformVarData(varName, data);
            }
        }

        public void SetUniformVarData(string varName, OpenTK.Matrix3 data)
        {
            if (UniformVariableNames.Contains(varName))
            {
                MaterialProgram.SetUniformVarData(varName, data);
            }
        }

        public void SetUniformVarData(string varName, OpenTK.Matrix4 data)
        {
            if (UniformVariableNames.Contains(varName))
            {
                MaterialProgram.SetUniformVarData(varName, data);
            }
        }

        public void SetUniformVector2ArrayData(string varName, ref float[] data )
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


        protected Dictionary<string, DynamicUniformBuffer> UniformBufferMap = null;
        protected Dictionary<string, TextureUnit> SamplerMap = null;
        protected List<string> UniformVariableNames = null;
    }
}
