using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

using System.Diagnostics;

namespace Core.OpenGLShader
{
    public abstract class ShaderProgramBase : IBindable
    {
        protected List<string> cachedUniformBufferNames = new List<string>();

        protected List<string> cachedUniformVarNames = new List<string>();

        protected List<string> cachedSamplerNames = new List<string>();

        protected void CacheShaderProgramInfo()
        {
            cachedUniformBufferNames = GetActiveUniformBlockNames();
            cachedUniformVarNames = GetActiveUniformNames();
            cachedSamplerNames = GetSampler2DNames();
        }

        public bool ContainsUniformBuffer(string name)
        {
            return cachedUniformBufferNames.Contains(name);
        }

        public bool ContainsUniformVariable(string name)
        {
            return cachedUniformVarNames.Contains(name);
        }

        public bool ContainsSampler(string name)
        {
            return cachedSamplerNames.Contains(name);
        }

        public ShaderProgramBase()
        {
            ProgramObject = GL.CreateProgram();
        }

        public void Bind()
        {
            UseProgram();
        }

        public void Unbind()
        {
            GL.UseProgram(0);
        }
        

        public void DeleteProgram()
        {
            GL.DeleteProgram(ProgramObject);
            ProgramObject = 0;
        }

        public string ProgramName = "";

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
            if (ProgramObject != 0)
            {
                int result = 0;

                GL.GetProgram(ProgramObject, GetProgramParameterName.LinkStatus, out result);

                return result == 1;
            }

            return false;
        }

        public void SetUniformVarData(string VarName, float fValue)
        {
            var Loc = GL.GetUniformLocation(ProgramObject, VarName);
            GL.Uniform1(Loc, fValue);
        }

        public void SetUniformVarData(string VarName, int data)
        {
            var Loc = GL.GetUniformLocation(ProgramObject, VarName);
            GL.Uniform1(Loc, data);
        }

        public void SetUniformVarData(string VarName, OpenTK.Vector2 Data)
        {
            var Loc = GL.GetUniformLocation(ProgramObject, VarName);
            GL.Uniform2(Loc, Data);
        }

        public void SetUniformVarData(string VarName, ref OpenTK.Vector2 Data)
        {
            var Loc = GL.GetUniformLocation(ProgramObject, VarName);
            GL.Uniform2(Loc, ref Data);
        }

        public void SetUniformVarData(string VarName, OpenTK.Vector3 Data)
        {
            var Loc = GL.GetUniformLocation(ProgramObject, VarName);
            GL.Uniform3(Loc, Data);
        }

        public void SetUniformVarData(string VarName, ref OpenTK.Vector3 Data)
        {
            var Loc = GL.GetUniformLocation(ProgramObject, VarName);
            GL.Uniform3(Loc, ref Data);
        }

        public void SetUniformVarData(string VarName, OpenTK.Vector4 data)
        {
            var Loc = GL.GetUniformLocation(ProgramObject, VarName);
            GL.Uniform4(Loc, ref data);
        }

        public void SetUniformVarData(string VarName, ref OpenTK.Vector4 Data)
        {
            var Loc = GL.GetUniformLocation(ProgramObject, VarName);
            GL.Uniform4(Loc, Data);
        }

        public void SetUniformVarData(string VarName, ref OpenTK.Matrix2 Data)
        {
            var Loc = GL.GetUniformLocation(ProgramObject, VarName);
            GL.UniformMatrix2(Loc, false, ref Data);
        }

        public void SetUniformVarData(string VarName, OpenTK.Matrix2 Data)
        {
            var Loc = GL.GetUniformLocation(ProgramObject, VarName);
            GL.UniformMatrix2(Loc, false, ref Data);
        }

        public void SetUniformVarData(string VarName, ref OpenTK.Matrix3 Data)
        {
            var Loc = GL.GetUniformLocation(ProgramObject, VarName);
            GL.UniformMatrix3(Loc, false, ref Data);
        }


        public void SetUniformVarData(string VarName, OpenTK.Matrix3 Data)
        {
            var Loc = GL.GetUniformLocation(ProgramObject, VarName);
            GL.UniformMatrix3(Loc, false, ref Data);
        }

        public void SetUniformVarData(string VarName, ref OpenTK.Matrix4 Data)
        {
            var Loc = GL.GetUniformLocation(ProgramObject, VarName);
            GL.UniformMatrix4(Loc, false, ref Data);
        }

        public void SetUniformVarData(string VarName, OpenTK.Matrix4 Data)
        {
            var Loc = GL.GetUniformLocation(ProgramObject, VarName);
            GL.UniformMatrix4(Loc, false, ref Data);
        }

        public void SetUniformVarData(string VarName, ref OpenTK.Matrix2x3 Data)
        {
            var Loc = GL.GetUniformLocation(ProgramObject, VarName);
            GL.UniformMatrix2x3(Loc, false, ref Data);
        }

        public void SetUniformVarData(string VarName, ref OpenTK.Matrix2x4 Data)
        {
            var Loc = GL.GetUniformLocation(ProgramObject, VarName);
            GL.UniformMatrix2x4(Loc, false, ref Data);
        }

        public void SetUniformVarData(string VarName, ref OpenTK.Matrix3x2 Data)
        {
            var Loc = GL.GetUniformLocation(ProgramObject, VarName);
            GL.UniformMatrix3x2(Loc, false, ref Data);
        }

        public void SetUniformVector2ArrayData(string VarName, ref float[] Data)
        {
            Debug.Assert(Data.Count() > 0);
            Debug.Assert(Data.Count() % 2 == 0);

            var Loc = GL.GetUniformLocation(ProgramObject, VarName);
            GL.Uniform2(Loc, Data.Count() / 2, Data);
        }

        public void SetUniformVector2ArrayData(string VarName, ref double[] Data)
        {
            Debug.Assert(Data.Count() > 0);
            Debug.Assert(Data.Count() % 2 == 0);

            var Loc = GL.GetUniformLocation(ProgramObject, VarName);
            GL.Uniform2(Loc, Data.Count() / 2, Data);
        }

        public void SetUniformVector3ArrayData(string VarName, ref float[] Data)
        {
            Debug.Assert(Data.Count() > 0);
            Debug.Assert(Data.Count() % 3 == 0);

            var Loc = GL.GetUniformLocation(ProgramObject, VarName);
            GL.Uniform3(Loc, Data.Count() / 3, Data);
        }

        public void SetUniformVector4ArrayData(string VarName, ref float[] Data)
        {
            Debug.Assert(Data.Count() > 0);
            Debug.Assert(Data.Count() % 4 == 0);

            var Loc = GL.GetUniformLocation(ProgramObject, VarName);
            GL.Uniform4(Loc, Data.Count() / 4, Data);
        }

        public int GetActiveUniformCount()
        {
            if (IsProgramLinked())
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
            if (IsProgramLinked())
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
            if (m_ProgramObject != 0)
            {
                GL.AttachShader(ProgramObject, ShaderToAttach.ShaderObject);
            }
        }

        public List<string> GetActiveUniformBlockNames()
        {
            List<string> result = new List<string>();

            if (IsProgramLinked())
            {
                int nBlockSize = GetActiveUniformBlockCount();

                for (var i = 0; i < nBlockSize; ++i)
                {
                    result.Add(GetUniformBlockName(i));
                }
            }

            return result;
        }

        public List<string> GetActiveUniformNames()
        {
            List<string> result = new List<string>();

            if (IsProgramLinked())
            {
                var uniformIndicesInBlock = new List<int>();

                var uniformBlockCount = GetActiveUniformBlockCount();

                for (int i = 0; i < uniformBlockCount; ++i)
                {
                    uniformIndicesInBlock.AddRange(GetUniformIndicesInBlock(i));
                }

                int nCount = GetActiveUniformCount();

                for (var i = 0; i < nCount; ++i)
                {
                    // skip uniform variables in block
                    if (uniformIndicesInBlock.Contains(i))
                    {
                        continue;
                    }

                    var uniformType = GetActiveUniformVariableType(i);
                    if (uniformType == ActiveUniformType.Sampler2D)
                    {
                        continue;
                    }

                    result.Add(GetActiveUniformVariableName(i));
                }
            }

            return result;
        }

        public string GetActiveUniformVariableName(int nIndex)
        {
            if (IsProgramLinked())
            {
                var result = GL.GetActiveUniformName(ProgramObject, nIndex);

                var index = result.IndexOf('[');
                if (index > 0)
                {
                    result = result.Remove(index);
                }

                return result;
            }

            return "";
        }

        public ActiveUniformType GetActiveUniformVariableType(int nIndex)
        {
            if (IsProgramLinked())
            {
                ActiveUniformType type;
                int size;
                GL.GetActiveUniform(ProgramObject, nIndex, out size, out type);
                return type;
            }

            Debug.Assert(false);

            return ActiveUniformType.Bool;
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

        public int ActiveUniformCount
        {
            get { return GetActiveUniformCount(); }
        }

        public string GetUniformBlockName(int nBlockIndex)
        {
            if (IsProgramLinked())
            {
                if (nBlockIndex < GetActiveUniformBlockCount())
                {
                    //var SB = new StringBuilder();
                    string SB = "";

                    int length = 0;

                    GL.GetActiveUniformBlock(ProgramObject, nBlockIndex, ActiveUniformBlockParameter.UniformBlockNameLength, out length);

                    int dummy = 0;

                    GL.GetActiveUniformBlockName(ProgramObject, nBlockIndex, length, out dummy, out SB);

                    return SB;
                }
            }

            return "";
        }

        public int GetUniformBlockIndex(String BlockName)
        {
            if (ProgramLinked)
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
                if (nBlockIndex < ActiveUniformBlockCount)
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
            if (ProgramLinked)
            {
                if (nBlockIndex < ActiveUniformBlockCount)
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

            if (Indices.Count > 0)
            {
                foreach (var index in Indices)
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
            if (ProgramLinked)
            {
                if (nBlockIndex < ActiveUniformBlockCount)
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

            if (ProgramLinked)
            {
                var Indices = GetUniformIndicesInBlock(nBlockIndex);

                foreach (var index in Indices)
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
            if (ProgramLinked)
            {
                var Count = GetActiveUniformCount();

                for (int i = 0; i < Count; ++i)
                {
                    int size;
                    ActiveUniformType type;

                    GL.GetActiveUniform(ProgramObject, i, out size, out type);

                    if (type == ActiveUniformType.Sampler2D)
                    {
                        result.Add(i);
                    }
                }
            }

            return result;
        }

        public List<string> GetSampler2DNames()
        {
            var result = new List<string>();
            if (ProgramLinked)
            {
                var Count = GetActiveUniformCount();

                for (int i = 0; i < Count; ++i)
                {
                    int size;
                    ActiveUniformType type;
                    
                    GL.GetActiveUniform(ProgramObject, i, out size, out type);

                    if (type == ActiveUniformType.Sampler2D)
                    {
                        result.Add(GL.GetActiveUniformName(ProgramObject, i));
                    }
                    else if (type == ActiveUniformType.SamplerCube)
                    {
                        result.Add(GL.GetActiveUniformName(ProgramObject, i));
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

        public int GetProgramBinaryLength()
        {
            if (ProgramLinked)
            {
                byte[] data = new byte[1024 * 1024 * 1024];
                int outLength;
                BinaryFormat outFormat;
                GL.GetProgramBinary(ProgramObject, 1024 * 1024 * 1024, out outLength, out outFormat, data);
                return outLength;
            }
            return 0;
        }

        public bool GetProgramBinary(ref byte[] data, out int outLength)
        {
            if (ProgramLinked)
            {   
                BinaryFormat outFormat;
                GL.GetProgramBinary(ProgramObject, data.Length, out outLength, out outFormat, data);

                if (outLength == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            outLength = 0;
            return false;
        }


        public List<int> GetUniformVariableOffsetsInBlock(int nBlockIndex)
        {
            if (ProgramLinked)
            {
                var Indices = GetUniformIndicesInBlock(nBlockIndex);

                if (Indices.Count > 0)
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
            if (ProgramLinked)
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

            if (ProgramLinked)
            {
                int nCount = GetActiveVertexAttributeCount();

                for (int index = 0; index < nCount; ++index)
                {
                    int nSize = 0;
                    int nBuffSize = 1024;
                    int nLength = 0;

                    ActiveAttribType Type = ActiveAttribType.None;

                    string sb = "";

                    GL.GetActiveAttrib(ProgramObject, index, nBuffSize, out nLength, out nSize, out Type, out sb);

                    var attributeLocation = GetAttributeLocation(sb);

                    if (attributeLocation >= 0)
                    {
                        result.Add(new VertexAttribute(attributeLocation, Type, sb.ToString()));
                    }
                }
            }

            return result.OrderBy(x => x.AttributeLocation).ToList();
        }

        public int GetAttributeLocation(string AttributeName)
        {
            if (ProgramLinked)
            {
                return GL.GetAttribLocation(ProgramObject, AttributeName);
            }

            return -1;
        }


        public void BindUniformBlock(int BlockIndex)
        {
            if (ProgramLinked)
            {
                if (BlockIndex < ActiveUniformBlockCount)
                {
                    GL.UniformBlockBinding(ProgramObject, BlockIndex, m_UniformBlockBindingIndex);
                    m_UniformBlockBindingIndex++;
                }
            }
        }

        public void BindUniformBlock(string BlockName)
        {
            if (ProgramLinked)
            {
                var BlockIndex = GetUniformBlockIndex(BlockName);

                Debug.Assert(BlockIndex != -1);

                if (BlockIndex < ActiveUniformBlockCount)
                {
                    GL.UniformBlockBinding(ProgramObject, BlockIndex, BlockIndex);
                }
                else
                {
                    Debug.Assert(false);
                }
            }
        }

        public int GetUniformBlockBindingPoint(int BlockIndex)
        {
            if (ProgramLinked)
            {
                if (BlockIndex < ActiveUniformBlockCount)
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
            if (ProgramLinked)
            {
                var BlockIndex = GetUniformBlockIndex(BlockName);

                return BlockIndex;                
            }

            return -1;
        }

        public int GetUniformLocation(string Name)
        {
            if (ProgramLinked)
            {
                var result = GL.GetUniformLocation(m_ProgramObject, Name);
                return result;
            }

            return -1;
        }

        public string GetUniformName(int Location)
        {
            if (ProgramLinked)
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

        private int m_ProgramObject = 0;

        private int m_UniformBlockBindingIndex = 0;
    }
}
