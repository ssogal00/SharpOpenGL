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

        public bool ProgramLinked
        {
            get { return IsProgramLinked(); }
        }

        public List<string> ActiveUniformBlockNames
        {
            get { return GetActiveUniformBlockNames(); }
        }

        public int ActiveUniformBlockCount
        {
            get { return GetActiveUniformBlockCount(); }
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

        public List<int> GetUniformIndicesInBlock(int nBlockIndex)
        {
            List<int> result = new List<int>();            
            
            if (ProgramLinked)
            {
                if(nBlockIndex < ActiveUniformBlockCount)
                {
                    int nSize = 0;

                    GL.GetActiveUniformBlock(ProgramObject, nBlockIndex, ActiveUniformBlockParameter.UniformBlockActiveUniforms, out nSize);

                    var arr = new int[nSize];

                    GL.GetActiveUniformBlock(ProgramObject, nBlockIndex, ActiveUniformBlockParameter.UniformBlockActiveUniformIndices, arr);

                    result.AddRange(arr);

                    return result;
                }
            }

            return result;
        }

        public List<string> GetUniformVariableNamesInBlock(int nBlockIndex)
        {
            var Indices = GetUniformIndicesInBlock(nBlockIndex);

            var result = new List<string>();

            if(Indices.Count > 0)
            {
                foreach(var index in Indices)
                {
                    result.Add(GL.GetActiveUniformName(ProgramObject, index));
                }
            }

            return result;
        }

        public int GetUniformBlockSize(int nBlockIndex)
        {
            if(ProgramLinked)
            {
                if(nBlockIndex < ActiveUniformBlockCount )
                {
                    int nResult = 0;

                    GL.GetActiveUniformBlock(ProgramObject, nBlockIndex, ActiveUniformBlockParameter.UniformBlockDataSize, out nResult);

                    return nResult;
                }
            }

            return 0;
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
