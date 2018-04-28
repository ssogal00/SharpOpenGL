using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Core.OpenGLShader
{
    public class Shader : IDisposable
    {
        public Shader()
        {            
        }

        public void Dispose()
        {
            GL.DeleteShader(shaderObject);
            shaderObject = -1;
        }

        public void CompileShader(string ShaderSourceCode)
        {
            GL.ShaderSource(shaderObject, ShaderSourceCode);
            GL.CompileShader(shaderObject);            

            int nStatus;
            GL.GetShader(shaderObject, ShaderParameter.CompileStatus, out nStatus);

            if (nStatus != 1)
            {
                string ShaderErrLog;
                GL.GetShaderInfoLog(shaderObject, out ShaderErrLog);

                Console.WriteLine(ShaderErrLog);
            }
        }

        public bool CompileShader(string shaderSourceCode, out string errorlog)
        {
            GL.ShaderSource(shaderObject, shaderSourceCode);
            GL.CompileShader(shaderObject);

            errorlog = string.Empty;

            int nStatus;
            GL.GetShader(shaderObject, ShaderParameter.CompileStatus, out nStatus);

            if (nStatus != 1)
            {
                GL.GetShaderInfoLog(shaderObject, out errorlog);

                return false;
            }

            return true;
        }

        public int ShaderObject 
        {
            get { return shaderObject; }

            protected set
            {
                shaderObject = value;
            }
        }

        private int shaderObject = -1;
    }
}
