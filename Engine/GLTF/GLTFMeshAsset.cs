using GLTF.V2;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
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
        Opacity,
        Mask,
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
        public readonly int Index = 0;
        public readonly string AttributeName = "";
        public readonly AttributeType AttributeType = AttributeType.SCALAR;


        public VertexAttributeSemantic(int attributeIndex, string attributeName, AttributeType @type)
        {
            this.Index = attributeIndex;
            this.AttributeName = attributeName;
            this.AttributeType = @type;
        }
        public bool Equals(VertexAttributeSemantic semantic)
        {
            return Index == semantic.Index && AttributeName == semantic.AttributeName;
        }

        public bool Equals(object o)
        {
            return Equals(o as VertexAttributeSemantic);
        }
        
        public override int GetHashCode()
        {
            return Index;
        }
    }

    public class GLTFMeshMaterial
    {
        public string Name = "";

        public Dictionary<PBRTextureType, string> TextureMap = new Dictionary<PBRTextureType, string>();

        public AlphaMode Alpha = AlphaMode.OPAQUE;

        public Vector3 EmissiveFactor = Vector3.Zero;
    }

    public class GLTFMeshAsset
    {
        public static async Task<List<GLTFMeshAsset>> LoadFromAsync(GLTF_V2 gltf)
        {
            var result = await Task.Factory.StartNew(() => { return LoadFrom(gltf);});
            return result;
        }

        public static List<GLTFMeshAsset> LoadFrom(GLTF_V2 gltf)
        {
            // buffer
            List<byte[]> bufferDatas = new List<byte[]>();
            List<byte[]> bufferViews = new List<byte[]>();
            Dictionary<int, List<float>> floatBufferViews = new Dictionary<int, List<float>>();

            var baseDir = gltf.BaseDir;

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

            for (int meshIndex = 0; meshIndex < gltf.meshes.Count; ++meshIndex)
            {
                var mesh = new GLTFMeshAsset();

                // for each mesh
                // parser index array and vertex attributes
                for (int pindex = 0; pindex < gltf.meshes[meshIndex].primitives.Count; ++pindex)
                {
                    // index array
                    int indexArraryAccessorIndex = gltf.meshes[meshIndex].primitives[pindex].indices;
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
                    foreach (var kvp in gltf.meshes[meshIndex].primitives[pindex].attributes)
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

                    // materials
                    var normTexture = gltf.GetNormalTexturePath(meshIndex, pindex);
                    var colorTexture = gltf.GetBaseColorTexturePath(meshIndex, pindex);
                    var metallicRoughTexture = gltf.GetMetallicRoughnessTexturePath(meshIndex, pindex);
                    var occlusionTexture = gltf.GetOcclusionTexturePath(meshIndex, pindex);

                    mesh.Material = new GLTFMeshMaterial();
                    
                    mesh.Material.Name = gltf.GetMaterialName(gltf.meshes[meshIndex].primitives[pindex].material);
                    mesh.Material.TextureMap.Add(PBRTextureType.Normal, normTexture);
                    mesh.Material.TextureMap.Add(PBRTextureType.BaseColor, colorTexture);
                    mesh.Material.TextureMap.Add(PBRTextureType.MetallicRoughness, metallicRoughTexture);
                    mesh.Material.TextureMap.Add(PBRTextureType.Occlusion, occlusionTexture);
                }

                parsedMeshList.Add(mesh);
            }

            return parsedMeshList;
        }

        public GLTFMeshAsset()
        {
        }

        public GLTFMeshMaterial Material { get; set; }

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

        public Vector3 MaxPosition { get; }

        public Vector3 MinPosition { get; }



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

        public Dictionary<PBRTextureType, string> TextureMap = new Dictionary<PBRTextureType, string>();

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
