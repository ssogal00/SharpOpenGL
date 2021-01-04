﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using GLTF.V2;
using System.IO;
using System.Linq;
using System.Printing;
using System.Windows.Documents.DocumentStructures;
using System.Xaml;
using Core;
using Core.Primitive;
using OpenTK.Compute.OpenCL;
using OpenTK.Graphics.ES11;
using OpenTK.Mathematics;
using AttributeType = GLTF.V2.AttributeType;

namespace GLTF
{
    public class VertexAttributeSemantic : IEquatable<VertexAttributeSemantic>
    {
        public readonly int index = 0;
        public readonly string name = "";
        public readonly AttributeType attributeType = AttributeType.SCALAR;


        public VertexAttributeSemantic(int attributeIndex, string attributeName, AttributeType @type)
        {
            this.index = attributeIndex;
            this.name = attributeName;
            this.attributeType = @type;
        }
        public bool Equals(VertexAttributeSemantic semantic)
        {
            return index == semantic.index && name == semantic.name;
        }

        public bool Equals(object o)
        {
            return Equals(o as VertexAttributeSemantic);
        }
        
        public override int GetHashCode()
        {
            return index;
        }
    }
    public class GLTFMesh
    {
        public static List<GLTFMesh> LoadFrom(GLTF_V2 gltf)
        {
            // buffer
            List<byte[]> mBufferDatas = new List<byte[]>();
            // bufferView
            Dictionary<int, List<Vector4>> vector4BufferViews = new Dictionary<int, List<Vector4>>();
            Dictionary<int, List<Vector3>> vector3BufferViews = new Dictionary<int, List<Vector3>>();
            Dictionary<int, List<Vector2>> vector2BufferViews = new Dictionary<int, List<Vector2>>();

            Dictionary<int, List<ushort>> uShortBufferViews = new Dictionary<int, List<ushort>>();
            Dictionary<int, List<uint>> uIntBufferViews = new Dictionary<int, List<uint>>();
            Dictionary<int, List<float>> floatBufferViews = new Dictionary<int, List<float>>();

            var baseDir = Path.GetDirectoryName(gltf.Path);

            for (int i = 0; i < gltf.buffers.Count; ++i)
            {
                var filepath = gltf.buffers[i].uri;
                byte[] result = File.ReadAllBytes(Path.Combine(baseDir, filepath));
                mBufferDatas.Add(result);
            }

            // fill buffer views
            for (int i = 0; i < gltf.accessors.Count; ++i)
            {
                var bufferViewIndex = gltf.accessors[i].bufferView;
                var componentType = gltf.accessors[i].componentType;
                var count = gltf.accessors[i].count;
                var attributeType = gltf.accessors[i].type;

                var bufferIndex = gltf.bufferViews[bufferViewIndex].buffer;
                var offset = gltf.bufferViews[bufferViewIndex].byteOffset;
                var length = gltf.bufferViews[bufferViewIndex].byteLength;

                var span = new Span<byte>(mBufferDatas[bufferIndex], offset, length);

                int countPerRead = 1;
                int bytesPerRead = 1;

                switch (attributeType)
                {
                    case AttributeType.VEC3:
                        countPerRead = 3;
                        break;
                    case AttributeType.VEC2:
                        countPerRead = 2;
                        break;
                    case AttributeType.VEC4:
                        countPerRead = 4;
                        break;
                    case AttributeType.SCALAR:
                        countPerRead = 1;
                        break;
                    case AttributeType.MAT2:
                        countPerRead = 4;
                        break;
                    case AttributeType.MAT3:
                        countPerRead = 9;
                        break;
                }

                switch (componentType)
                {
                    case ComponentType.UNSIGNED_INT:
                    case ComponentType.FLOAT:
                        bytesPerRead = 4;
                        break;
                    case ComponentType.BYTE:
                    case ComponentType.UNSIGNED_BYTE:
                        bytesPerRead = 1;
                        break;
                    case ComponentType.SHORT:
                    case ComponentType.UNSIGNED_SHORT:
                        bytesPerRead = 2;
                        break;
                }

                //
                if (attributeType == AttributeType.VEC3)
                {
                    Debug.Assert(vector3BufferViews.ContainsKey(bufferViewIndex) == false);
                    vector3BufferViews.Add(bufferViewIndex, new List<Vector3>());
                }
                else if (attributeType == AttributeType.VEC2)
                {
                    Debug.Assert(vector2BufferViews.ContainsKey(bufferViewIndex) == false);
                    vector2BufferViews.Add(bufferViewIndex, new List<Vector2>());
                }
                else if (attributeType == AttributeType.SCALAR)
                {
                    if (componentType == ComponentType.UNSIGNED_SHORT)
                    {
                        uShortBufferViews.Add(bufferViewIndex, new List<ushort>());
                    }
                    else if (componentType == ComponentType.UNSIGNED_INT)
                    {
                        uIntBufferViews.Add(bufferViewIndex, new List<uint>());
                    }
                }

                int bytesToRead = bytesPerRead * countPerRead;

                for (int readCount = 0; readCount < count; ++readCount)
                {
                    var sliced = span.Slice(readCount * bytesToRead, bytesToRead);
                    if (attributeType == AttributeType.VEC3)
                    {
                        var parsed = ToVector3(ref sliced);
                        vector3BufferViews[bufferViewIndex].Add(parsed);
                    }
                    else if (attributeType == AttributeType.VEC2)
                    {
                        var parsed = ToVector2(ref sliced);
                        vector2BufferViews[bufferViewIndex].Add(parsed);
                    }
                    else if (attributeType == AttributeType.VEC4)
                    {
                        var parsed = ToVector4(ref sliced);
                        vector4BufferViews[bufferViewIndex].Add(parsed);
                    }
                    else if (attributeType == AttributeType.SCALAR)
                    {
                        if (componentType == ComponentType.UNSIGNED_SHORT)
                        {
                            var parsed = ToUShort(ref sliced);
                            uShortBufferViews[bufferViewIndex].Add(parsed);
                        }
                        else if (componentType == ComponentType.UNSIGNED_INT)
                        {
                            var parsed = ToUInt(ref sliced);
                            uIntBufferViews[bufferViewIndex].Add(parsed);
                        }
                        else if (componentType == ComponentType.FLOAT)
                        {
                            var parsed = BitConverter.ToSingle(sliced);
                            floatBufferViews[bufferViewIndex].Add(parsed);
                        }
                    }
                }
            }

            List<GLTFMesh> parsedMeshList = new List<GLTFMesh>();

            for (int i = 0; i < gltf.meshes.Count; ++i)
            {
                var mesh = new GLTFMesh();

                for (int j = 0; j < gltf.meshes[i].primitives.Count; ++j)
                {
                    int accessorIndex = 0;
                    int indexToBufferView = 0;
                    
                    foreach (var kvp in gltf.meshes[i].primitives[j].attributes)
                    {
                        string semantic = kvp.Key;
                        accessorIndex = kvp.Value;

                        var attributeSemantic = new VertexAttributeSemantic(accessorIndex, semantic, gltf.accessors[accessorIndex].type);
                        
                        mesh.mVertexAttributeList.Add(attributeSemantic);

                        indexToBufferView = gltf.accessors[accessorIndex].bufferView;
                        
                        if (gltf.accessors[accessorIndex].type == AttributeType.SCALAR)
                        {
                            mesh.mFloatVertexAttributes.Add(attributeSemantic, floatBufferViews[indexToBufferView]);
                        }
                        else if (gltf.accessors[accessorIndex].type == AttributeType.VEC2)
                        {
                            mesh.mVector2VertexAttributes.Add(attributeSemantic, vector2BufferViews[indexToBufferView]);
                        }
                        else if (gltf.accessors[accessorIndex].type == AttributeType.VEC3)
                        {
                            mesh.mVector3VertexAttributes.Add(attributeSemantic, vector3BufferViews[indexToBufferView]);
                        }
                        else if (gltf.accessors[accessorIndex].type == AttributeType.VEC4)
                        {
                            mesh.mVector4VertexAttributes.Add(attributeSemantic, vector4BufferViews[indexToBufferView]);
                        }
                    }

                    accessorIndex = gltf.meshes[i].primitives[j].indices;
                    indexToBufferView = gltf.accessors[accessorIndex].bufferView;

                    if (gltf.accessors[accessorIndex].componentType == ComponentType.UNSIGNED_INT)
                    {
                        mesh.mUIntIndices = uIntBufferViews[indexToBufferView];
                    }
                    else if (gltf.accessors[accessorIndex].componentType == ComponentType.UNSIGNED_SHORT)
                    {
                        mesh.mUShortIndices = uShortBufferViews[indexToBufferView];
                    }
                }
            }

            return parsedMeshList;
        }

        public GLTFMesh()
        {
            
        }

        public List<VertexAttributeSemantic> VertexAttributeList
        {
            get => mVertexAttributeList;
        }
        public Dictionary<VertexAttributeSemantic, List<float>> FloatVertexAttributes
        {
            get => mFloatVertexAttributes;
        }
        public Dictionary<VertexAttributeSemantic, List<Vector2>> Vector2VertexAttributes
        {
            get => mVector2VertexAttributes;
        }
        public Dictionary<VertexAttributeSemantic, List<Vector3>> Vector3VertexAttributes
        {
            get => mVector3VertexAttributes;
        }
        public Dictionary<VertexAttributeSemantic, List<Vector4>> Vector4VertexAttributes
        {
            get => mVector4VertexAttributes;
        }

        public List<uint> UIntIndices
        {
            get => mUIntIndices;
        }
        public List<ushort> UShortIndices
        {
            get => mUShortIndices;
        }

        protected List<VertexAttributeSemantic> mVertexAttributeList = new List<VertexAttributeSemantic>();

        protected Dictionary<VertexAttributeSemantic, List<float>> mFloatVertexAttributes = new Dictionary<VertexAttributeSemantic, List<float>>();

        protected Dictionary<VertexAttributeSemantic, List<Vector3>> mVector3VertexAttributes = new Dictionary<VertexAttributeSemantic, List<Vector3>>();

        protected Dictionary<VertexAttributeSemantic, List<Vector2>> mVector2VertexAttributes = new Dictionary<VertexAttributeSemantic, List<Vector2>>();

        protected Dictionary<VertexAttributeSemantic, List<Vector4>> mVector4VertexAttributes = new Dictionary<VertexAttributeSemantic, List<Vector4>>();

        protected List<uint> mUIntIndices= new List<uint>();
        protected List<ushort> mUShortIndices = new List<ushort>();

        private static uint ToUInt(ref Span<byte> span)
        {
            return BitConverter.ToUInt32(span);
        }

        private static ushort ToUShort(ref Span<byte> span)
        {
            return BitConverter.ToUInt16(span);
        }

        private static Vector3 ToVector3(ref Span<byte> span)
        {
            var xPart=span.Slice(0, 4);
            var yPart=span.Slice(4, 4);
            var zPart =span.Slice(8, 4);
            var x = BitConverter.ToSingle(xPart);
            var y = BitConverter.ToSingle(yPart);
            var z = BitConverter.ToSingle(zPart);

            return new Vector3(x,y,z);
        }

        private static Vector2 ToVector2(ref Span<byte> span)
        {
            var xPart = span.Slice(0, 4);
            var yPart = span.Slice(4, 4);
            
            var x = BitConverter.ToSingle(xPart);
            var y = BitConverter.ToSingle(yPart);
            

            return new Vector2(x, y);
        }

        private static Vector4 ToVector4(ref Span<byte> span)
        {
            var xPart = span.Slice(0, 4);
            var yPart = span.Slice(4, 4);
            var zPart = span.Slice(8, 4);
            var wPart = span.Slice(12, 4);

            var x = BitConverter.ToSingle(xPart);
            var y = BitConverter.ToSingle(yPart);
            var z = BitConverter.ToSingle(zPart);
            var w = BitConverter.ToSingle(wPart);

            return new Vector4(x, y, z,w);
        }
    }
}
