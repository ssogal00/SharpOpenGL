using System;
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
    public enum PBRTextureType
    {
        Emissive,
        Normal,
        Occlusion,
        BaseColor,
        MetallicRoughness,
    }

    public class PBRInfo
    {
        public string EmissiveTexture;
        public string NormalTexture;
        public string OcclusionTexture;
        public string BaseColorTexture;
    }

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
    public class GLTFMeshAsset
    {
        public static List<GLTFMeshAsset> LoadFrom(GLTF_V2 gltf)
        {
            // buffer
            List<byte[]> bufferDatas = new List<byte[]>();
            List<byte[]> bufferViews = new List<byte[]>();
            Dictionary<int, List<float>> floatBufferViews = new Dictionary<int, List<float>>();

            var baseDir = Path.GetDirectoryName(gltf.Path);

            // buffers
            for (int i = 0; i < gltf.buffers.Count; ++i)
            {
                var filepath = gltf.buffers[i].uri;

                Debug.Assert(File.Exists(Path.Combine(baseDir, filepath)));

                byte[] result = File.ReadAllBytes(Path.Combine(baseDir, filepath));
                bufferDatas.Add(result);
            }

            // bufferViews
            for (int i = 0; i < gltf.bufferViews.Count; ++i)
            {
                var arr = (new Span<byte>( bufferDatas[gltf.bufferViews[i].buffer], gltf.bufferViews[i].byteOffset, gltf.bufferViews[i].byteLength)).ToArray();
                bufferViews.Add(arr);
            }

            List<GLTFMeshAsset> parsedMeshList = new List<GLTFMeshAsset>();

            for (int i = 0; i < gltf.meshes.Count; ++i)
            {
                var mesh = new GLTFMeshAsset();

                // for each mesh
                // parser index array and vertex attributes
                for (int pindex = 0; pindex < gltf.meshes[i].primitives.Count; ++pindex)
                {
                    // index array
                    int indexArraryAccessorIndex = gltf.meshes[i].primitives[pindex].indices;
                    int indexArrayBufferViewIndex = gltf.accessors[indexArraryAccessorIndex].bufferView;
                    
                    if (gltf.accessors[indexArraryAccessorIndex].componentType == ComponentType.UNSIGNED_INT)
                    {
                        int byteLength = 4 * gltf.accessors[indexArraryAccessorIndex].count;
                        
                        var indexSpan = new Span<byte>(bufferDatas[indexArrayBufferViewIndex]);
                        
                        mesh.UIntIndices = ToUIntList(ref indexSpan, gltf.accessors[indexArrayBufferViewIndex].byteOffset, byteLength, gltf.accessors[indexArraryAccessorIndex].count);
                    }
                    else if (gltf.accessors[indexArraryAccessorIndex].componentType == ComponentType.UNSIGNED_SHORT)
                    {
                        int byteLength = 2 * gltf.accessors[indexArraryAccessorIndex].count;

                        var indexSpan = new Span<byte>(bufferDatas[indexArrayBufferViewIndex]);

                        mesh.UShortIndices = ToUShortList(ref indexSpan, gltf.accessors[indexArraryAccessorIndex].byteOffset, byteLength, gltf.accessors[indexArraryAccessorIndex].count);
                    }

                    // vertex attributes
                    foreach (var kvp in gltf.meshes[i].primitives[pindex].attributes)
                    {
                        string semantic = kvp.Key;
                        int accessorIndex = kvp.Value;
                        int bufferViewIndex = gltf.accessors[accessorIndex].bufferView;

                        var attributeSemantic = new VertexAttributeSemantic(bufferViewIndex, semantic, gltf.accessors[accessorIndex].type);

                        // we found new vertex attribute
                        if (!mesh.mVertexAttributeMap.ContainsKey(semantic))
                        {
                            mesh.mVertexAttributeMap.Add(semantic, attributeSemantic);

                            if (gltf.accessors[accessorIndex].type == AttributeType.SCALAR)
                            {
                                var floatSpan = new Span<byte>(bufferViews[bufferViewIndex]);
                                var length = gltf.accessors[accessorIndex].count * 4 * 2;
                                List<float> floatList = ToFloatList(ref floatSpan, gltf.accessors[accessorIndex].byteOffset, length, gltf.accessors[accessorIndex].count);
                                mesh.mFloatVertexAttributes.Add(attributeSemantic, floatList);
                            }
                            else if (gltf.accessors[accessorIndex].type == AttributeType.VEC2)
                            {
                                var vec2Span = new Span<byte>(bufferViews[bufferViewIndex]);
                                var length = gltf.accessors[accessorIndex].count * 4 * 2;
                                List<Vector2> vec2List = ToVector2List(ref vec2Span, gltf.accessors[accessorIndex].byteOffset, length, gltf.accessors[accessorIndex].count);
                                mesh.mVector2VertexAttributes.Add(attributeSemantic, vec2List);
                            }
                            else if (gltf.accessors[accessorIndex].type == AttributeType.VEC3)
                            {
                                var vec3Span = new Span<byte>(bufferViews[bufferViewIndex]);
                                var length = gltf.accessors[accessorIndex].count * 4 * 3;
                                List<Vector3> vec3List = ToVector3List(ref vec3Span, gltf.accessors[accessorIndex].byteOffset, length, gltf.accessors[accessorIndex].count);
                                mesh.mVector3VertexAttributes.Add(attributeSemantic, vec3List);
                            }
                            else if (gltf.accessors[accessorIndex].type == AttributeType.VEC4)
                            {
                                var vec4Span = new Span<byte>(bufferViews[bufferViewIndex]);
                                var length = gltf.accessors[accessorIndex].count * 4 * 4;
                                List<Vector4> vec4List = ToVector4List(ref vec4Span, gltf.accessors[accessorIndex].byteOffset, length, gltf.accessors[accessorIndex].count);
                                mesh.mVector4VertexAttributes.Add(attributeSemantic, vec4List);
                            }
                        }
                    }
                }

                parsedMeshList.Add(mesh);
            }

            return parsedMeshList;
        }

        public GLTFMeshAsset()
        {
        }

        public PBRInfo PBRInfo = new PBRInfo();

        public List<string> TexturePaths { get; set; }

        public string Name { get; set; }

        public List<VertexAttributeSemantic> VertexAttributeList
        {
            get => mVertexAttributeList;
        }

        public Dictionary<string, VertexAttributeSemantic> VertexAttributeMap
        {
            get => mVertexAttributeMap;
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
            set => mUIntIndices = value;
        }
        public List<ushort> UShortIndices
        {
            get => mUShortIndices;
            set => mUShortIndices = value;
        }

        protected List<VertexAttributeSemantic> mVertexAttributeList = new List<VertexAttributeSemantic>();

        protected Dictionary<string, VertexAttributeSemantic> mVertexAttributeMap = new Dictionary<string, VertexAttributeSemantic>();

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

        private static List<ushort> ToUShortList(ref Span<byte> span, int start, int length, int count)
        {
            List<ushort> result = new List<ushort>();

            var tempSpan = span.Slice(start, length);
            for (int i = 0; i < count; ++i)
            {
                var anotherSpan = tempSpan.Slice(i * 2, 2);
                result.Add(ToUShort(ref anotherSpan));
            }

            return result;
        }

        private static List<uint> ToUIntList(ref Span<byte> span, int start, int length, int count)
        {
            List<uint> result = new List<uint>();

            var tempSpan = span.Slice(start, length);
            for (int i = 0; i < count; ++i)
            {
                var anotherSpan = tempSpan.Slice(i * 4, 4);
                result.Add(ToUInt(ref anotherSpan));
            }

            return result;
        }

        private static List<float> ToFloatList(ref Span<byte> span, int start, int length, int count)
        {
            List<float> result = new List<float>();

            var tempSpan = span.Slice(start, length);
            for (int i = 0; i < count; ++i)
            {
                var anotherSpan = tempSpan.Slice(i * 4, 4);
                result.Add(BitConverter.ToSingle(anotherSpan));
            }

            return result;
        }

        private static List<Vector3> ToVector3List(ref Span<byte> span, int start, int length, int count)
        {
            List<Vector3> result = new List<Vector3>();

            var tempSpan = span.Slice(start, length);
            for (int i = 0; i < count; ++i)
            {
                var anotherSpan = tempSpan.Slice(i * 3 * 4, 12);
                result.Add(ToVector3(ref anotherSpan));
            }

            return result;
        }

        private static List<Vector2> ToVector2List(ref Span<byte> span, int start, int length, int count)
        {
            List<Vector2> result = new List<Vector2>();

            var tempSpan = span.Slice(start, length);
            for (int i = 0; i < count; ++i)
            {
                var anotherSpan = tempSpan.Slice(i * 2 * 4, 8);
                result.Add(ToVector2(ref anotherSpan));
            }

            return result;
        }

        private static List<Vector4> ToVector4List(ref Span<byte> span, int start, int length, int count)
        {
            List<Vector4> result = new List<Vector4>();

            var tempSpan = span.Slice(start, length);
            for (int i = 0; i < count; ++i)
            {
                var anotherSpan = tempSpan.Slice(i * 4 * 4, 16);
                result.Add(ToVector4(ref anotherSpan));
            }

            return result;
        }
    }
}
