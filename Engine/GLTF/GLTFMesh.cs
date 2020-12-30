using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using GLTF.V2;
using System.IO;
using System.Printing;
using System.Xaml;
using Core.Primitive;
using OpenTK.Graphics.ES11;
using OpenTK.Mathematics;
using AttributeType = GLTF.V2.AttributeType;

namespace GLTF
{
    public class GLTFMesh
    {

        public GLTFMesh(GLTF_V2 gltf)
        {
            var baseDir= Path.GetDirectoryName(gltf.Path);

            for (int i = 0; i < gltf.buffers.Count; ++i)
            {
                var filepath= gltf.buffers[i].uri;
                byte[] result = File.ReadAllBytes(Path.Combine(baseDir,filepath));
                mBufferDatas.Add(result);
            }

            for (int i = 0; i < gltf.accessors.Count; ++i)
            {
                var bufferViewIndex= gltf.accessors[i].bufferView;
                var componentType = gltf.accessors[i].componentType;
                var count = gltf.accessors[i].count;
                var attributeType = gltf.accessors[i].type;

                var bufferIndex= gltf.bufferViews[bufferViewIndex].buffer;
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
                    Debug.Assert(mVector3BufferViews.ContainsKey(bufferViewIndex) == false);
                    mVector3BufferViews.Add(bufferViewIndex, new List<Vector3>());
                }
                else if (attributeType == AttributeType.VEC2)
                {
                    Debug.Assert(mVector2BufferViews.ContainsKey(bufferViewIndex) == false);
                    mVector2BufferViews.Add(bufferViewIndex, new List<Vector2>());
                }
                else if (attributeType == AttributeType.SCALAR)
                {
                    if (componentType == ComponentType.UNSIGNED_SHORT)
                    {
                        mUShortBufferViews.Add(bufferViewIndex, new List<ushort>());
                    }
                    else if (componentType == ComponentType.UNSIGNED_INT)
                    {
                        mUIntBufferViews.Add(bufferViewIndex, new List<uint>());
                    }
                }

                int bytesToRead = bytesPerRead * countPerRead;

                for (int readCount = 0; readCount < count; ++readCount)
                {
                    var sliced = span.Slice(readCount * bytesToRead, bytesToRead);
                    if (attributeType == AttributeType.VEC3)
                    {
                        var parsed = ToVector3(ref sliced);
                        mVector3BufferViews[bufferViewIndex].Add(parsed);
                    }
                    else if (attributeType == AttributeType.VEC2)
                    {
                        var parsed = ToVector2(ref sliced);
                        mVector2BufferViews[bufferViewIndex].Add(parsed);
                    }
                    else if (attributeType == AttributeType.VEC4)
                    {
                        var parsed = ToVector4(ref sliced);
                        mVector4BufferViews[bufferViewIndex].Add(parsed);
                    }
                    else if (attributeType == AttributeType.SCALAR)
                    {
                        if (componentType == ComponentType.UNSIGNED_SHORT)
                        {
                            var parsed = ToUShort(ref sliced);
                            mUShortBufferViews[bufferViewIndex].Add(parsed);
                        }
                        else if (componentType == ComponentType.UNSIGNED_INT)
                        {
                            var parsed = ToUInt(ref sliced);
                            mUIntBufferViews[bufferViewIndex].Add(parsed);
                        }
                        else if (componentType == ComponentType.FLOAT)
                        {
                            var parsed = BitConverter.ToSingle(sliced);
                            mFloatBufferViews[bufferViewIndex].Add(parsed);
                        }
                    }
                }
            }
        }

        private uint ToUInt(ref Span<byte> span)
        {
            return BitConverter.ToUInt32(span);
        }

        private ushort ToUShort(ref Span<byte> span)
        {
            return BitConverter.ToUInt16(span);
        }

        private Vector3 ToVector3(ref Span<byte> span)
        {
            var xPart=span.Slice(0, 4);
            var yPart=span.Slice(4, 4);
            var zPart =span.Slice(8, 4);
            var x = BitConverter.ToSingle(xPart);
            var y = BitConverter.ToSingle(yPart);
            var z = BitConverter.ToSingle(zPart);

            return new Vector3(x,y,z);
        }

        private Vector2 ToVector2(ref Span<byte> span)
        {
            var xPart = span.Slice(0, 4);
            var yPart = span.Slice(4, 4);
            
            var x = BitConverter.ToSingle(xPart);
            var y = BitConverter.ToSingle(yPart);
            

            return new Vector2(x, y);
        }

        private Vector4 ToVector4(ref Span<byte> span)
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

        // buffer
        private List<byte[]> mBufferDatas = new List<byte[]>();
        // bufferView
        private Dictionary<int, List<Vector4>> mVector4BufferViews = new Dictionary<int, List<Vector4>>();
        private Dictionary<int, List<Vector3>> mVector3BufferViews = new Dictionary<int, List<Vector3>>();
        private Dictionary<int, List<Vector2>> mVector2BufferViews = new Dictionary<int, List<Vector2>>();
        
        private Dictionary<int, List<ushort>> mUShortBufferViews = new Dictionary<int, List<ushort>>();
        private Dictionary<int, List<uint>> mUIntBufferViews = new Dictionary<int, List<uint>>();
        private Dictionary<int, List<float>> mFloatBufferViews = new Dictionary<int, List<float>>();
    }
}
