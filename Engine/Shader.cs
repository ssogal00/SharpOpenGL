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
            string ShaderErrLog = "";
            bool bSuccess = CompileShader(ShaderSourceCode, out ShaderErrLog);

            if (bSuccess == false)
            {
                Console.WriteLine(ShaderErrLog);
            }
        }

        public bool CompileShader(string[] shaderSourceCodes, out string errorlog)
        {
            int [] LengthList = new int[shaderSourceCodes.Length];

            for (int i = 0; i < shaderSourceCodes.Length; ++i)
            {
                LengthList[i] = shaderSourceCodes[i].Length;
            }

            GL.ShaderSource(shaderObject, shaderSourceCodes.Length, shaderSourceCodes, LengthList);
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
