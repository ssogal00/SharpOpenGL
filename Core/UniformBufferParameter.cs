using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Buffer;
using OpenTK.Graphics.OpenGL;

namespace Core
{
    public class UniformBufferParameter<T> where T : struct
    {
        public UniformBufferParameter(int programObject, string name, ref T data)
        {
            this.ProgramObject = programObject;
            this.BufferObject = new DynamicUniformBuffer(programObject, name);
            this.Data = data;
        }

        public void SetParameter()
        {
            BufferObject.Bind();
            BufferObject.BufferData<T>(ref Data);
        }

        public void SetValue(ref T newValue)
        {
            Data = newValue;
        }

        protected DynamicUniformBuffer BufferObject = null;
        protected int ProgramObject = 0;
        protected T Data = default(T);
    }
}
