using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Core;
using OpenTK.Audio.OpenAL;
using System.IO;
using OpenTK.Mathematics;


namespace GLTF.V2
{
    public enum ComponentType
    {
        BYTE = 5120,
        UNSIGNED_BYTE = 5121,
        SHORT = 5122,
        UNSIGNED_SHORT=5123,
        UNSIGNED_INT=5125,
        FLOAT=5126,
    }

    public enum AttributeType
    {
        /// SCALAR
        /// VEC2
        /// VEC3
        /// VEC4
        /// MAT2
        /// MAT3
        /// MAT4
        None,
        SCALAR,
        VEC2,
        VEC3,
        VEC4,
        MAT2,
        MAT3,
        MAT4,
    }

    public enum BufferTarget
    {
        //
        ARRAY_BUFFER = 34962,
        ELEMENT_ARRAY_BUFFER = 34963,
    }

    public enum AlphaMode
    {
        OPAQUE,
        MASK,
        BLEND
    }

    

    public class JsonNumArrayToVector4TypeConverter : JsonConverter<Vector4>
    {
        public override Vector4 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var x = reader.GetSingle();
                var y = reader.GetSingle();
                var z = reader.GetSingle();
                var w = reader.GetSingle();
                return new Vector4(x,y,z,w);
            }

            return Vector4.Zero;
        }
        public override void Write(Utf8JsonWriter writer, Vector4 data, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }

    public class JsonNumArrayToVector3TypeConverter : JsonConverter<Vector3>
    {
        public override Vector3 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var x = reader.GetSingle();
                var y = reader.GetSingle();
                var z = reader.GetSingle();
                return new Vector3(x, y, z);
            }

            return Vector3.Zero;
        }
        public override void Write(Utf8JsonWriter writer, Vector3 data, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }

    public class JsonNumToComponentTypeConverter : JsonConverter<ComponentType>
    {
        public override ComponentType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                var value = reader.GetInt32();
                switch (value)
                {
                    case 5120:
                        return ComponentType.BYTE;
                    case 5121:
                        return ComponentType.UNSIGNED_BYTE;
                    case 5123:
                        return ComponentType.UNSIGNED_SHORT;
                    case 5125:
                        return ComponentType.UNSIGNED_INT;
                    case 5126:
                        return ComponentType.FLOAT;
                }
            }

            return ComponentType.BYTE;
        }

        public override void Write(Utf8JsonWriter writer, ComponentType data, JsonSerializerOptions options)
        {
            writer.WriteNumberValue((int)data);
        }
    }

    public class JsonStringToAttributeTypeConverter : JsonConverter<AttributeType>
    {
        public override AttributeType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var str = reader.GetString();
                AttributeType result;
                AttributeType.TryParse(str, out result);
                return result;
            }

            return AttributeType.SCALAR;
        }

        public override void Write(Utf8JsonWriter writer, AttributeType value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }

    public class JsonStringToAlphaModeConverter : JsonConverter<AlphaMode>
    {
        public override AlphaMode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var str = reader.GetString();
                AlphaMode mode;
                AlphaMode.TryParse(str, out mode);
                return mode;
            }

            return AlphaMode.OPAQUE;
        }

        public override void Write(Utf8JsonWriter writer, AlphaMode value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }

    public class JsonMinMaxConverter : JsonConverter<MinMax>
    {
        public override MinMax Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var result = new MinMax();
                
                result.Type = AttributeType.SCALAR;

                float x = 0;
                float y = 0;
                float z = 0;
                float w = 0;

                if (reader.TryGetSingle(out x))
                {
                    result.Type = AttributeType.SCALAR;
                    result.ScalarValue = x;
                }

                if (reader.TryGetSingle(out y))
                {
                    result.Type = AttributeType.VEC2;
                    result.Vector2Value = new Vector2(x,y);
                }

                if (reader.TryGetSingle(out z))
                {
                    result.Type = AttributeType.VEC3;
                    result.Vector3Value = new Vector3(x,y,z);
                }

                if (reader.TryGetSingle(out w))
                {
                    result.Type = AttributeType.VEC4;
                    result.Vector4Value = new Vector4(x,y,z,w);
                }

                return result;
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, MinMax value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }

    public class Helper
    {
        public static int ComponentSizeInByte(int componentType)
        {
            /// 5120 : BYTE
            /// 5121 : UNSIGNED_BYTE
            /// 5122 : SHORT
            /// 5123 : UNSIGNED_SHORT
            /// 5125 : UNSIGNED_INT
            /// 5126 : FLOAT
            
            switch (componentType)
            {
                case 5120:
                    return 1;
                case 5121:
                    return 1;
                case 5122:
                    return 2;
                case 5123:
                    return 2;
                case 5125:
                    return 4;
                case 5126:
                    return 4;
            }

            return 1;
        }
    }

    public class GLTF_V2
    {
        [JsonPropertyName("accessors")]
        public List<Accessor> accessors { get; set; }

        [JsonPropertyName("bufferViews")]
        public List<BufferView> bufferViews { get; set; }
        
        [JsonPropertyName("buffers")]
        public List<Buffer> buffers { get; set; }
        
        [JsonPropertyName("cameras")]
        public List<Camera> cameras { get; set; }
        
        [JsonPropertyName("meshes")]
        public List<Mesh> meshes { get; set; }
        
        [JsonPropertyName("asset")]
        public AssetInfo asset { get; set; }

        [JsonPropertyName("materials")]
        public List<GLTFJsonMaterial> materials { get; set; }

        [JsonPropertyName("images")]
        public List<GLTFImage> images { get; set; }

        public string BaseDir = "";

        public string GetMaterialName(int materialIndex)
        {
            return materials[materialIndex].name;
        }

        public string GetNormalTexturePath(int meshIndex, int primitiveIndex)
        {
            int materialIndex = meshes[meshIndex].primitives[primitiveIndex].material;
            
            if (materials[materialIndex].normalTexture.index >= 0)
            {
                return Path.Combine(this.BaseDir, images[materials[materialIndex].normalTexture.index].uri);
            }

            return string.Empty;
        }

        public string GetBaseColorTexturePath(int meshIndex, int primitiveIndex)
        {
            int materialIndex = meshes[meshIndex].primitives[primitiveIndex].material;
            int textureIndex = materials[materialIndex].pbrMetallicRoughness.baseColorTexture.index;

            if (textureIndex >= 0)
            {
                return Path.Combine(BaseDir, images[textureIndex].uri);
            }

            return string.Empty;
        }

        public string GetMetallicRoughnessTexturePath(int meshIndex, int primitiveIndex)
        {
            int materialIndex = meshes[meshIndex].primitives[primitiveIndex].material;
            int textureIndex = materials[materialIndex].pbrMetallicRoughness.metallicRoughnessTexture.index;
            if (textureIndex >= 0)
            {
                return Path.Combine(BaseDir, images[textureIndex].uri);
            }

            return string.Empty;
        }

        public string GetOcclusionTexturePath(int meshIndex, int primitiveIndex)
        {
            int materialIndex = meshes[meshIndex].primitives[primitiveIndex].material;
            int textureIndex = materials[materialIndex].occlusionTexture.index;
            if (textureIndex >= 0)
            {
                return Path.Combine(BaseDir, images[textureIndex].uri);
            }

            return string.Empty;
        }
    }

    public class GLTFImage
    {
        [JsonPropertyName("uri")]
        public string uri { get; set; }
    }

    public class TextureIndex
    {
        [JsonPropertyName("index")] public int index { get; set; } = -1;
    }

    public class PBRMetallicRoughness
    {
        [JsonPropertyName("baseColorTexture")]
        public TextureIndex baseColorTexture { get; set; }

        [JsonPropertyName("baseColorFactor")]
        public Vector4 baseColorFactor { get; set; }
        
        [JsonPropertyName("metallicRoughnessTexture")]
        public TextureIndex metallicRoughnessTexture { get; set; }

        [JsonPropertyName("metallicFactor")]
        public float metallicFactor { get; set; }

        [JsonPropertyName("roughnessFactor")]
        public float roughnessFactor { get; set; }
    }

    public class GLTFJsonMaterial
    {
        [JsonPropertyName("emissiveTexture")]
        public TextureIndex emissiveTexture { get; set; }
        
        [JsonPropertyName("occlusionTexture")]
        public TextureIndex occlusionTexture { get; set; }

        [JsonPropertyName("normalTexture")]
        public TextureIndex normalTexture { get; set; }

        [JsonPropertyName("doubleSided")] 
        public bool doubleSided { get; set; }

        [JsonPropertyName("pbrMetallicRoughness")]
        public PBRMetallicRoughness pbrMetallicRoughness { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("alphaMode")] 
        [JsonConverter(typeof(JsonStringToAlphaModeConverter))]
        public AlphaMode alphaMode { get; set; } = AlphaMode.OPAQUE;

    }

    public class AssetInfo
    {
        public string generator { get; set; }
        public string version { get; set; }
    }

    public class MinMax
    {
        public Vector4 Vector4Value { get; set; }
        public Vector3 Vector3Value { get; set; }

        public Vector2 Vector2Value { get; set; }

        public float ScalarValue { get; set; }

        public AttributeType Type { get; set; }
    }

    public class Camera
    {
        public string name { get; set; }
        public string type { get; set; }

        public Perspective perspective { get; set; }
    }

    public class Perspective
    {
        public float aspectRatio { get; set; }

        [JsonPropertyName("yfov")]
        public float YFov { get; set; }
        public float zfar { get; set; }
        public float znear { get; set; }
    }


    public class Mesh
    {
        public string name { get; set; }
        public List<Primitive> primitives { get; set; }
    }

    public class Primitive
    {
        public int mode { get; set; }
        public Dictionary<string,int> attributes { get; set; }
        public int indices { get; set; }
        [JsonPropertyName("material")]
        public int material { get; set; }
    }

    public class Attributes
    {
        [JsonPropertyName("NORMAL")]
        public int normal { get; set; }

        [JsonPropertyName("POSITION")]
        public int position { get; set; }
        [JsonPropertyName("TEXCOORD_0")]
        public int texcoord0 { get; set; }
    }

    public class Accessor
    {
        public int bufferView { get; set; }
        public int byteOffset { get; set; }
        /// <summary>
        /// SCALAR
        /// VEC2
        /// VEC3
        /// VEC4
        /// MAT2
        /// MAT3
        /// MAT4 
        /// </summary>
        /// 
        [JsonConverter(typeof(JsonStringToAttributeTypeConverter))]
        public AttributeType type { get; set; }
        public int count { get; set; }
        /// <summary>
        /// 5120 : BYTE
        /// 5121 : UNSIGNED_BYTE
        /// 5122 : SHORT
        /// 5123 : UNSIGNED_SHORT
        /// 5125 : UNSIGNED_INT
        /// 5126 : FLOAT
        /// </summary>
        ///
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ComponentType componentType { get; set; }

        [JsonConverter(typeof(JsonMinMaxConverter))]
        public MinMax min { get; set; }

        [JsonConverter(typeof(JsonMinMaxConverter))]
        public MinMax max { get; set; }
    }

    public class Buffer
    {
        public int byteLength { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class BufferView
    {
        public int buffer { get; set; }
        public int byteLength { get; set; }
        public int byteOffset { get; set; }
        /// <summary>
        /// buffer target
        /// 34962 : "ARRAY_BUFFER"
        /// 34963 : "ELEMENT_ARRAY_BUFFER"
        /// </summary>
        public int target { get; set; }

        public string name { get; set; }
        public int byteStride { get; set; }
    }
}
