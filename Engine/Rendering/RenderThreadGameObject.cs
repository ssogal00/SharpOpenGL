using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CompiledMaterial.GBufferDraw;
using Core;
using Core.Buffer;
using Core.Primitive;
using GLTF;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using AttributeType = GLTF.V2.AttributeType;

namespace Engine
{
    public class RenderThreadGameObject : IDisposable
    {   
        public RenderThreadGameObject(GameObject gameObject)
        {
            Debug.Assert(RenderingThread.IsInRenderingThread());
            mGameObject = gameObject;
            Initialize();
        }

        public virtual void Dispose()
        {
            if (mVertexArray != null)
            {
                ResourceManager.Instance.DeleteVertexArray(mVertexArray);
            }

            if (mIndexBuffer != null)
            {
                ResourceManager.Instance.DeleteIndexBuffer(mIndexBuffer);
            }

            foreach (var vb in mVertexBuffers)
            {
                ResourceManager.Instance.DeleteVertexBuffer(vb.BufferHandle);
            }
        }

        public virtual void Render()
        {
            // 1. material bind
            // 2. material param setup
            // 3. vertex array bind
            // 4. draw
            // unbind...
            
            var material = ShaderManager.Instance.GetMaterial(mGameObject.MaterialName);
            

            Debug.Assert(material != null);

            material.Bind();

            SetMaterialParams();
            
            ChangeRenderState();

            mVertexArray.Bind();

            if (mHasIndex)
            {
                Debug.Assert(mIndexCount > 0);
                mIndexBuffer.Bind();
                GL.DrawElements(PrimitiveType.Triangles, mIndexCount, mIndexType, 0);
                mIndexBuffer.Unbind();
            }
            else
            {
                Debug.Assert(mGameObject.VertexCount > 0);
                GL.DrawArrays(PrimitiveType.Triangles, 0, mGameObject.VertexCount);
            }
            

            mVertexArray.Unbind();

            material.Unbind();

            RestoreRenderState();
        }

        public virtual void RenderMultipleSections()
        {
            for (int sectionIndex = 0; sectionIndex < mGameObject.MeshSectionList.Count; ++sectionIndex)
            {
                mVertexArrayList[sectionIndex].Bind();

                mVertexArrayList[sectionIndex].Unbind();
            }
        }

        protected virtual void ChangeRenderState()
        {

        }

        protected virtual void RestoreRenderState()
        {

        }

        protected virtual void SetMaterialParams(int sectionIndex)
        {
            var vec3Params = mGameObject.GetVector3Params(sectionIndex);
            var vec2Params = mGameObject.GetVector2Params(sectionIndex);
            var vec4Params = mGameObject.GetVector4Params(sectionIndex);
            var mat4Params = mGameObject.GetMatrix4Params(sectionIndex);
            var textureParams = mGameObject.GetTextureParams(sectionIndex);
            var boolParams = mGameObject.GetBoolParams(sectionIndex);
            var floatParams = mGameObject.GetFloatParams(sectionIndex);
            var intParams = mGameObject.GetIntParams(sectionIndex);

            var material = ShaderManager.Instance.GetMaterial(mGameObject.MeshSectionList[sectionIndex].MaterialName);

            Debug.Assert(material != null);

            material.Bind();

            // set if it exists
            foreach (var kvp in vec3Params)
            {
                material.SetUniformVariable(kvp.Item1, kvp.Item2);
            }

            foreach (var kvp in vec4Params)
            {
                material.SetUniformVariable(kvp.Item1, kvp.Item2);
            }

            foreach (var kvp in vec2Params)
            {
                material.SetUniformVariable(kvp.Item1, kvp.Item2);
            }

            foreach (var kvp in mat4Params)
            {
                material.SetUniformVariable(kvp.Item1, kvp.Item2);
            }

            foreach (var kvp in boolParams)
            {
                material.SetUniformVariable(kvp.Item1, kvp.Item2);
            }

            foreach (var kvp in textureParams)
            {
                var tex = TextureManager.Instance.LoadTexture2D(kvp.Item2);
                material.SetTexture(kvp.Item1, tex);
            }

            foreach (var kvp in intParams)
            {
                material.SetUniformVariable(kvp.Item1, kvp.Item2);
            }
        }

        protected virtual void SetMaterialParams()
        {
            var vec3Params = mGameObject.GetVector3Params();
            var vec2Params = mGameObject.GetVector2Params();
            var vec4Params = mGameObject.GetVector4Params();
            var mat4Params = mGameObject.GetMatrix4Params();
            var textureParams = mGameObject.GetTextureParams();
            var boolParams = mGameObject.GetBoolParams();
            var floatParams = mGameObject.GetFloatParams();
            var intParams = mGameObject.GetIntParams();

            var material = ShaderManager.Instance.GetMaterial(mGameObject.MaterialName);

            // set if it exists
            foreach (var kvp in vec3Params)
            {
                material.SetUniformVariable(kvp.Item1, kvp.Item2);
            }

            foreach (var kvp in vec4Params)
            {
                material.SetUniformVariable(kvp.Item1, kvp.Item2);
            }

            foreach (var kvp in vec2Params)
            {
                material.SetUniformVariable(kvp.Item1, kvp.Item2);
            }

            foreach (var kvp in mat4Params)
            {
                material.SetUniformVariable(kvp.Item1, kvp.Item2);
            }

            foreach (var kvp in boolParams)
            {
                material.SetUniformVariable(kvp.Item1, kvp.Item2);
            }

            foreach (var kvp in textureParams)
            {
                var tex=TextureManager.Instance.LoadTexture2D(kvp.Item2);
                material.SetTexture(kvp.Item1, tex);
            }

            foreach (var kvp in intParams)
            {
                material.SetUniformVariable(kvp.Item1, kvp.Item2);
            }
        }

        protected void InitializeMultipleSections()
        {
            // for each section
            for (int sectionIndex = 0; sectionIndex < mGameObject.MeshSectionCount; sectionIndex++)
            {
                var section = mGameObject.MeshSectionList[sectionIndex];
                var vertexArray = ResourceManager.Instance.CreateVertexArray();
                mVertexArrayList.Add(vertexArray);

                var attrList = section.VertexAttributeMap
                .OrderBy(x => x.Value.GetSemanticIndexInShader())
                .Select(x => x.Value)
                .ToArray();

                vertexArray.Bind();

                // fill vertex bufer data
                for (int i = 0; i < attrList.Length; ++i)
                {
                    Vector3[] vector3Data = null;
                    Vector2[] vector2Data = null;
                    Vector4[] vector4Data = null;

                    SOAVertexBuffer<Vec3_VertexAttribute> vector3VB = null;
                    SOAVertexBuffer<Vec2_VertexAttribute> vector2VB = null;
                    SOAVertexBuffer<Vec4_VertexAttribute> vector4VB = null;

                    switch (attrList[i].AttributeType)
                    {
                        case AttributeType.VEC4:
                            vector4Data = section.Vector4VertexAttributes[attrList[i]].ToArray();
                            vector4VB = ResourceManager.Instance.CreateSOAVertexBuffer<Vec4_VertexAttribute>();
                            if (mVertexBufferMap.ContainsKey(sectionIndex) == false)
                            {
                                mVertexBufferMap.Add(sectionIndex, new List<OpenGLBuffer>());
                            }
                            mVertexBufferMap[sectionIndex].Add(vector4VB);
                            vector4VB.BufferData<Vector4>(ref vector4Data);
                            vector4VB.BindVertexAttribute(i);
                            break;

                        case AttributeType.VEC3:
                            vector3Data = section.Vector3VertexAttributes[attrList[i]].ToArray();
                            vector3VB = ResourceManager.Instance.CreateSOAVertexBuffer<Vec3_VertexAttribute>();
                            if (mVertexBufferMap.ContainsKey(sectionIndex) == false)
                            {
                                mVertexBufferMap.Add(sectionIndex, new List<OpenGLBuffer>());
                            }
                            vector3VB.BufferData<Vector3>(ref vector3Data);
                            vector3VB.BindVertexAttribute(i);
                            break;

                        case AttributeType.VEC2:
                            vector2Data = section.Vector2VertexAttributes[attrList[i]].ToArray();
                            vector2VB = ResourceManager.Instance.CreateSOAVertexBuffer<Vec2_VertexAttribute>();
                            if (mVertexBufferMap.ContainsKey(sectionIndex) == false)
                            {
                                mVertexBufferMap.Add(sectionIndex, new List<OpenGLBuffer>());
                            }
                            vector2VB.BufferData<Vector2>(ref vector2Data);
                            vector2VB.BindVertexAttribute(i);
                            break;
                    }
                }

                if (section.UIntIndices.Count > 0)
                {
                    var indexBuffer = ResourceManager.Instance.CreateIndexBuffer();
                    mIndexBufferList.Add(indexBuffer);
                    indexBuffer.Bind();
                    indexBuffer.BufferData(section.UIntIndices.ToArray());
                    mIndexType = DrawElementsType.UnsignedInt;
                    mIndexCount = section.UIntIndices.Count;
                    indexBuffer.Unbind();
                }
                else if (section.UShortIndices.Count > 0)
                {
                    var indexBuffer = ResourceManager.Instance.CreateIndexBuffer();
                    mIndexBufferList.Add(indexBuffer);
                    indexBuffer.Bind();
                    indexBuffer.BufferData(section.UShortIndices.ToArray());
                    mIndexType = DrawElementsType.UnsignedShort;
                    mIndexCount = section.UShortIndices.Count;
                    indexBuffer.Unbind();
                }
                else
                {
                    mHasIndex = false;
                }

                vertexArray.Unbind();
            }
        }

        protected void Initialize()
        {
            mVertexArray = ResourceManager.Instance.CreateVertexArray();

            var attrList = mGameObject.VertexAttributeMap
                .OrderBy(x => x.Value.GetSemanticIndexInShader())
                .Select(x => x.Value)
                .ToArray();

            mVertexArray.Bind();

            // fill vertex bufer data
            for (int i = 0; i < attrList.Length; ++i)
            {
                Vector3[] vector3Data = null;
                Vector2[] vector2Data = null;
                Vector4[] vector4Data = null;

                SOAVertexBuffer<Vec3_VertexAttribute> vector3VB = null;
                SOAVertexBuffer<Vec2_VertexAttribute> vector2VB = null;
                SOAVertexBuffer<Vec4_VertexAttribute> vector4VB = null;

                switch (attrList[i].AttributeType)
                {
                    case AttributeType.VEC4:
                        vector4Data = mGameObject.Vector4VertexAttributes[attrList[i]].ToArray();
                        vector4VB = ResourceManager.Instance.CreateSOAVertexBuffer<Vec4_VertexAttribute>();
                        mVertexBuffers.Add(vector4VB);
                        vector4VB.BufferData<Vector4>(ref vector4Data);
                        vector4VB.BindVertexAttribute(i);
                        break;

                    case AttributeType.VEC3:
                        vector3Data = mGameObject.Vector3VertexAttributes[attrList[i]].ToArray();
                        vector3VB = ResourceManager.Instance.CreateSOAVertexBuffer<Vec3_VertexAttribute>();
                        mVertexBuffers.Add(vector3VB);
                        vector3VB.BufferData<Vector3>(ref vector3Data);
                        vector3VB.BindVertexAttribute(i);
                        break;

                    case AttributeType.VEC2:
                        vector2Data = mGameObject.Vector2VertexAttributes[attrList[i]].ToArray();
                        vector2VB = ResourceManager.Instance.CreateSOAVertexBuffer<Vec2_VertexAttribute>();
                        mVertexBuffers.Add(vector2VB);
                        vector2VB.BufferData<Vector2>(ref vector2Data);
                        vector2VB.BindVertexAttribute(i);
                        break;
                }
            }
            
            if (mGameObject.UIntIndices.Count > 0)
            {
                mIndexBuffer = ResourceManager.Instance.CreateIndexBuffer();
                mIndexBuffer.Bind();
                mIndexBuffer.BufferData(mGameObject.UIntIndices.ToArray());
                mIndexType = DrawElementsType.UnsignedInt;
                mIndexCount = mGameObject.UIntIndices.Count;
                mIndexBuffer.Unbind();
            }
            else if (mGameObject.UShortIndices.Count > 0)
            {
                mIndexBuffer = ResourceManager.Instance.CreateIndexBuffer();
                mIndexBuffer.Bind();
                mIndexBuffer.BufferData(mGameObject.UShortIndices.ToArray());
                mIndexType = DrawElementsType.UnsignedShort;
                mIndexCount = mGameObject.UShortIndices.Count;
                mIndexBuffer.Unbind();
            }
            else
            {
                mHasIndex = false;
            }


            mVertexArray.Unbind();
        }

        protected GameObject mGameObject = null;

        protected VertexArray mVertexArray;

        protected List<VertexArray> mVertexArrayList;

        protected IndexBuffer mIndexBuffer;

        protected List<IndexBuffer> mIndexBufferList;

        protected List<OpenGLBuffer> mVertexBuffers = new List<OpenGLBuffer>();

        // mesh section index is key
        protected Dictionary<int, List<OpenGLBuffer>> mVertexBufferMap = new Dictionary<int, List<OpenGLBuffer>>();

        protected int mIndexCount = 0;
        
        protected DrawElementsType mIndexType = DrawElementsType.UnsignedInt;

        protected bool mHasIndex = true;
    }
}
