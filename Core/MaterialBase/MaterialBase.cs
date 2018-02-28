using Core.Buffer;
using Core.OpenGLShader;
using System.Collections.Generic;
using System.Diagnostics;
using OpenTK;

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
        }

        protected void Initialize()
        {
            var names = MaterialProgram.GetActiveUniformBlockNames();
            foreach(var name in names)
            {
                var uniformBuffer = new DynamicUniformBuffer(MaterialProgram, name);                
                UniformBufferMap.Add(name, uniformBuffer);
            }
        }

        public void Setup()
        {
            MaterialProgram.UseProgram();

            int index = 0;
            foreach(var uniformBuffer in UniformBufferMap)
            {
                uniformBuffer.Value.Bind();
                uniformBuffer.Value.BindBufferBase(index++);
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
            MaterialProgram.SetUniformVarData(varName, data);
        }

        public void SetUniformVarData(string varName, OpenTK.Vector2 data)
        {
            MaterialProgram.SetUniformVarData(varName, data);
        }

        public void SetUniformVarData(string varName, OpenTK.Vector3 data)
        {
            MaterialProgram.SetUniformVarData(varName, data);
        }

        public void SetUniformVarData(string varName, OpenTK.Vector4 data)
        {
            MaterialProgram.SetUniformVarData(varName, data);
        }

        public void SetUniformVarData(string varName, OpenTK.Matrix3 data)
        {
            MaterialProgram.SetUniformVarData(varName, data);
        }

        public void SetUniformVarData(string varName, OpenTK.Matrix4 data)
        {
            MaterialProgram.SetUniformVarData(varName, data);
        }
        
        protected Dictionary<string, DynamicUniformBuffer> UniformBufferMap = null;

    }
}
