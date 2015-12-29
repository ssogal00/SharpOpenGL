using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using SharpOpenGL.Buffer;

namespace SharpOpenGL
{
    public class ShaderProgram
    {

        public ShaderProgram()
        {
            ProgramObject = GL.CreateProgram();            
        }

        /// <summary>
        /// Link Program
        /// </summary>
        /// <param name="result">링크 결과를 담는 스트링</param>
        /// <returns>성공시 true, 실패시 false</returns>
        public bool LinkProgram(out String result)
        {
            GL.LinkProgram(ProgramObject);

            result = "";

            if (!ProgramLinked)
            {
                result = GL.GetProgramInfoLog(ProgramObject);
                return false;
            }

            return true;
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

        /// <summary>
        /// Return Block Count in Shader Program
        /// </summary>
        /// <returns>Count of Blocks</returns>
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

        /// <summary>
        /// Attach Fragment, Vertex, etc shader to Program
        /// </summary>
        /// <param name="ShaderToAttach"></param>
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

        public int GetUniformBlockIndex(String BlockName)
        {
            if(ProgramLinked)
            {
                return GL.GetUniformBlockIndex(ProgramObject, BlockName);
            }

            return -1;
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

        /// <summary>
        /// Return whether this block is referenced by Vertex Shader
        /// </summary>
        /// <param name="nBlockIndex"></param>
        /// <returns>true/false</returns>
        public bool IsBlockRefByVertexShader(int nBlockIndex)
        {
            if(ProgramLinked)
            {
                if(nBlockIndex < ActiveUniformBlockCount)
                {
                    int nReturn = 0;
                    GL.GetActiveUniformBlock(ProgramObject, nBlockIndex, ActiveUniformBlockParameter.UniformBlockReferencedByVertexShader, out nReturn);

                    return nReturn == 1;                    
                }
            }

            return false;
        }

        /// <summary>
        /// Uniform Block안의 Unifrom Variable이름들을 List로 리턴한다.
        /// </summary>
        /// <param name="nBlockIndex"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get Data Block Size 
        /// </summary>
        /// <param name="nBlockIndex"></param>
        /// <returns></returns>
        public int GetUniformBlockDataSize(int nBlockIndex)
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


        public List<ActiveUniformType> GetUniformVariableTypesInBlock(int nBlockIndex)
        {
            var result = new List<ActiveUniformType>();

            if(ProgramLinked)
            {
                var Indices = GetUniformIndicesInBlock(nBlockIndex);

                foreach(var index in Indices)
                {
                    int size;
                    ActiveUniformType type;
                    
                    GL.GetActiveUniform(ProgramObject, index, out size, out type);

                    result.Add(type);
                }
            }

            return result;
        }
        
        public List<int> GetSampler2DUniformLocations()
        {
            var result = new List<int>();
            if(ProgramLinked)
            {
                var Count = GetActiveUniformCount();

                for(int i = 0; i < Count; ++i)
                {
                    int size;
                    ActiveUniformType type;

                    GL.GetActiveUniform(ProgramObject, i, out size, out type);

                    if(type == ActiveUniformType.Sampler2D)
                    {
                        result.Add(i);
                    }
                }
            }

            return result;            
        }

        public int GetSampler2DUniformLocation(string Name)
        {
            if (ProgramLinked)
            {
                return GL.GetUniformLocation(ProgramObject, Name);
            }
            return -1;
        }

        
        public List<int> GetUniformVariableOffsetsInBlock(int nBlockIndex)
        {
            if(ProgramLinked)
            {
                var Indices = GetUniformIndicesInBlock(nBlockIndex);
                
                if(Indices.Count > 0)
                {
                    int[] result = new int[Indices.Count];

                    GL.GetActiveUniforms(ProgramObject, Indices.Count, Indices.ToArray(), ActiveUniformParameter.UniformOffset, result);

                    return result.ToList();
                }                
            }

            return null;
        }       


        public List<string> GetUniformVariableTypeStringsInBlock(int nBlockIndex)
        {
            var result = new List<string>();

            var types = GetUniformVariableTypesInBlock(nBlockIndex);

            types.ForEach(x => result.Add(x.ToString()));

            return result;
        }

        public List<UniformVariableMetaData> GetUniformVariableMetaDataList(int nBlockIndex)
        {
            var result = new List<UniformVariableMetaData>();

            var types = GetUniformVariableTypesInBlock(nBlockIndex);
            var names = GetUniformVariableNamesInBlock(nBlockIndex);
            var offsets = GetUniformVariableOffsetsInBlock(nBlockIndex);

            if (types.Count() > 0)
            {
                if (types.Count == names.Count && names.Count == offsets.Count)
                {
                    for (int i = 0; i < types.Count; ++i)
                    {
                        result.Add(new UniformVariableMetaData(names[i], types[i], offsets[i]));
                    }
                }
            }

            return result;
        }

        public int GetActiveVertexAttributeCount()
        {
            if(ProgramLinked)
            {
                int result = 0;
                
                GL.GetProgram(ProgramObject, GetProgramParameterName.ActiveAttributes, out result);                

                return result;
            }

            return 0;
        }
        
        
        public List<VertexAttribute> GetActiveVertexAttributeList()
        {
            var result = new List<VertexAttribute>();

            if(ProgramLinked)
            {
                int nCount = GetActiveVertexAttributeCount();

                for (int index = 0; index < nCount; ++index)
                {
                    int nSize = 0;
                    int nBuffSize = 1024;
                    int nLength = 0;

                    ActiveAttribType Type = ActiveAttribType.None;

                    StringBuilder sb = new StringBuilder();

                    GL.GetActiveAttrib(ProgramObject, index, nBuffSize, out nLength, out nSize, out Type, sb);

                    var attributeLocation = GetAttributeLocation(sb.ToString());

                    result.Add(new VertexAttribute(attributeLocation, Type, sb.ToString()));
                }
            }

            return result.OrderBy(x=>x.AttributeLocation).ToList();
        }

        public int GetAttributeLocation(string AttributeName)
        {   
            if(ProgramLinked)
            {
                return GL.GetAttribLocation(ProgramObject, AttributeName);
            }

            return -1;
        }

        
        public void BindUniformBlock(int BlockIndex)
        {
            if(ProgramLinked)
            {
                if(BlockIndex < ActiveUniformBlockCount)
                {
                    GL.UniformBlockBinding(ProgramObject, BlockIndex, m_UniformBlockBindingIndex);
                    m_UniformBlockBindingIndex++;
                }
            }
        }

        public void BindUniformBlock(string BlockName)
        {
            if(ProgramLinked)
            {
                var BlockIndex = GetUniformBlockIndex(BlockName);

                if(BlockIndex < ActiveUniformBlockCount)
                {
                    GL.UniformBlockBinding(ProgramObject, BlockIndex, m_UniformBlockBindingIndex);
                    m_UniformBlockBindingIndex++;
                }
            }
        }

        public int GetUniformBlockBindingPoint(int BlockIndex)
        {
            if(ProgramLinked)
            {
                if(BlockIndex < ActiveUniformBlockCount)
                {
                    int Index = -1;
                    GL.GetActiveUniformBlock(m_ProgramObject, BlockIndex, ActiveUniformBlockParameter.UniformBlockBinding, out Index);

                    return Index;
                }
            }

            return -1;
        }

        public int GetUniformBlockBindingPoint(string BlockName)
        {
            if(ProgramLinked)
            {
                var BlockIndex = GetUniformBlockIndex(BlockName);

                if(BlockIndex < ActiveUniformBlockCount)
                {
                    int Index = -1;
                    GL.GetActiveUniformBlock(m_ProgramObject, BlockIndex, ActiveUniformBlockParameter.UniformBlockBinding, out Index);
                    return Index;
                }
            }

            return -1;
        }

        public int GetUniformLocation(string Name)
        {
            if(ProgramLinked)
            {
                var result = GL.GetUniformLocation(m_ProgramObject, Name);                
                return result;
            }

            return -1;
        }

        public string GetUniformName(int Location)
        { 
            if(ProgramLinked)
            {
                return GL.GetActiveUniformName(ProgramObject, Location);
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

        public Dictionary<string, int> Sampler2DMap = new Dictionary<string,int>();

        private int m_ProgramObject = 0;

        private int m_UniformBlockBindingIndex = 0;
    }    
}
