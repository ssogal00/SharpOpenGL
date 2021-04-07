using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Buffer;
using OpenTK.Graphics.OpenGL;

namespace Core
{
    public class UniformBufferSet
    {
        protected Dictionary<string, UniformBufferParameterBase> UniformBufferDictionary = new Dictionary<string, UniformBufferParameterBase>();

        public UniformBufferSet(MaterialBase.MaterialBase material)
        {

        }
    }

    public abstract class UniformBufferParameterBase
    {
        public virtual void SetParameter() { }
    }


    public class UniformBufferParameter<T> : UniformBufferParameterBase where T : struct
    {
        public UniformBufferParameter(int programObject, string name, ref T data)
        {
            this.ProgramObject = programObject;
            this.BufferObject = new DynamicUniformBuffer(programObject, name);
            this.Data = data;
        }

        public override void SetParameter()
        {
            BufferObject.Bind();
            BufferObject.BufferData<T>(Data);
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
