using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Core
{
    public class UniformBufferParameter
    {
        public UniformBufferParameter(string name, int bufferSize, )
        {
            BufferName = name;
            BufferSize = bufferSize;
        }

        public void SetParameter()
        {
        }

        protected int BufferSize = 0;
        private string BufferName = "";
    }
}
