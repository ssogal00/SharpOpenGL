using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Core;

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
        public List<GLTFMaterial> materials { get; set; }

        [JsonPropertyName("images")]
        public List<GLTFImage> images { get; set; }

        public string Path = "";
    }

    public class GLTFImage
    {
        [JsonPropertyName("uri")]
        public string uri { get; set; }
    }

    public class TextureIndex
    {
        [JsonPropertyName("index")]
        public int index { get; set; }
    }

    public class PBRMetallicRoughness
    {
        [JsonPropertyName("baseColorTexture")]
        public TextureIndex baseColorTexture { get; set; }

        [JsonPropertyName("metallicRoughnessTexture")]
        public TextureIndex metallicRoughnessTexture { get; set; }
    }

    public class GLTFMaterial
    {
        [JsonPropertyName("emissiveTexture")] 
        public TextureIndex emissiveTexture { get; set; }
        
        [JsonPropertyName("occlusionTexture")] 
        public TextureIndex occlusionTexture { get; set; }

        [JsonPropertyName("normalTexture")]
        public TextureIndex normalTexture { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; }

    }

    public class AssetInfo
    {
        public string generator { get; set; }
        public string version { get; set; }
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
        public int byteStride { get; set; }
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
    }
}
