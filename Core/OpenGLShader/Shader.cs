using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Core.OpenGLShader
{
    public class Shader
    {
        public Shader()
        {
        }        


        public void CompileShader(string ShaderSourceCode)
        {
            GL.ShaderSource(m_ShaderObject, ShaderSourceCode);
            GL.CompileShader(m_ShaderObject);            

            int nStatus;
            GL.GetShader(m_ShaderObject, ShaderParameter.CompileStatus, out nStatus);

            if (nStatus != 1)
            {
                string ShaderErrLog;
                GL.GetShaderInfoLog(m_ShaderObject, out ShaderErrLog);

                Console.WriteLine(ShaderErrLog);
            }
        }

        public int ShaderObject 
        {
            get { return m_ShaderObject; }

            protected set
            {
                m_ShaderObject = value;
            }
        }

        private int m_ShaderObject = 0;
    }
}
