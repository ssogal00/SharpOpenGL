using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Buffer;
using OpenTK.Graphics.OpenGL;

namespace Core
{
    public class UniformBufferParameter<T>
    {
        public UniformBufferParameter(DynamicUniformBuffer bufferObject, string name, int programObject)
        {
            this.BufferName = name;
            this.ProgramObject = programObject;
            this.BufferObject = bufferObject;
        }

        public void SetParameter()
        {
            BufferObject.Bind();
        }

        protected DynamicUniformBuffer BufferObject = null;
        private string BufferName = "";
        protected int ProgramObject = 0;
    }
}
