using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SharpOpenGL
{
    public class ShaderProgram
    {

        public ShaderProgram()
        {
            ProgramObject = GL.CreateProgram();
        }

        public void LinkProgram()
        {
            GL.LinkProgram(ProgramObject);
        }

        public void UseProgram()
        {
            GL.UseProgram(ProgramObject);
        }

        public bool IsProgramLinked()
        { 
            if(ProgramObject != 0)
            {
                int result = 0;

                GL.GetProgram(ProgramObject, GetProgramParameterName.LinkStatus, out result);

                return result == 1;
            }

            return false;
        }

        public int GetActiveUniformCount()
        {
            if(IsProgramLinked())
            {
                int result = 0;
                
                GL.GetProgram(ProgramObject, GetProgramParameterName.ActiveUniforms, out result);

                return result;
            }

            return 0;
        }

        public int GetActiveUniformBlockCount()
        {
            if(IsProgramLinked())
            {
                int nResult = 0;

                GL.GetProgram(ProgramObject, GetProgramParameterName.ActiveUniformBlocks, out nResult);

                return nResult;
            }

            return 0;
        }

        public void AttachShader(Shader ShaderToAttach)
        {
            if(m_ProgramObject != 0)
            {
                GL.AttachShader(ProgramObject, ShaderToAttach.ShaderObject);
            }
        }

        public List<string> GetActiveUniformBlockNames()
        {
            List<string> result = new List<string>();

            if(IsProgramLinked())
            {
                int nBlockSize = GetActiveUniformBlockCount();

                for(var i = 0; i < nBlockSize; ++i)
                {
                    result.Add(GetUniformBlockName(i));
                }
            }

            return result;
        }

        public string GetUniformBlockName(int nBlockIndex)
        {
            if(IsProgramLinked())
            {
                if(nBlockIndex < GetActiveUniformBlockCount())
                {
                    var SB = new StringBuilder();
                    
                    int length = 0;
                    
                    GL.GetActiveUniformBlock(ProgramObject, nBlockIndex, ActiveUniformBlockParameter.UniformBlockNameLength, out length);

                    int dummy = 0;

                    GL.GetActiveUniformBlockName(ProgramObject, nBlockIndex, length, out dummy, SB);

                    return SB.ToString();
                }
            }

            return "";
        }

        public int ProgramObject
        {
            get
            {
                return m_ProgramObject;
            }

            protected set
            {
                m_ProgramObject = value;
            }
        }
     

        private int m_ProgramObject = 0;
    }    
}
