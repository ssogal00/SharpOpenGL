using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core.GLTF
{
    public class JsonNumToStringConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.TryGetInt64(out long l) ? l.ToString() : reader.GetDouble().ToString();
            }

            return reader.GetString();
        }

        public override void Write(Utf8JsonWriter writer, string data, JsonSerializerOptions options)
        {
            writer.WriteStringValue(data);
        }
    }

    public class DictionaryToArrayConverter<TDictionary, TKey, TValue> : JsonConverter<List<TValue>> where TDictionary : class, IDictionary<TKey, TValue>
    {
        public override List<TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                List<TValue> result = JsonSerializer.Deserialize<List<TValue>>(ref reader, options);
                if (result != null)
                {
                    return result;
                }
            }

            if (reader.TokenType == JsonTokenType.StartObject)
            {
                Dictionary<TKey, TValue> dicResult = JsonSerializer.Deserialize<Dictionary<TKey, TValue>>(ref reader, options);
                if (dicResult != null)
                {
                    return dicResult.Select(x => x.Value).ToList();
                }
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, List<TValue> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }


    public class GLTF
    {
        [JsonConstructor]
        public GLTF()
        {

        }
        public Dictionary<string, Accessor> accessors { get; set; }
        public Dictionary<string, BufferView> bufferViews { get; set; }
        public Dictionary<string, Buffer> buffers { get; set; }
        public Dictionary<string, Camera> cameras { get; set; }
        public Dictionary<string, Mesh> meshes { get; set; }

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
        public string indices { get; set; }
        public string material { get; set; }
        public int mode { get; set; }
        public Attributes attributes { get; set; }
    }

    public class Attributes
    {
        [JsonPropertyName("NORMAL")]
        public string normal { get; set; }

        [JsonPropertyName("POSITION")]
        public string position { get; set; }
        [JsonPropertyName("TEXCOORD_0")]
        public string texcoord0 { get; set; }
    }

    public class Accessor
    {
        [JsonConverter(typeof(JsonNumToStringConverter))]
        public string bufferView { get; set; }
        public int byteOffset { get; set; }
        public int byteStride { get; set; }
        public string type { get; set; }
        public int count { get; set; }
        public int componentType { get; set; }
    }

    public class Buffer
    {
        public int byteLength { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class BufferView
    {
        [JsonPropertyName("buffer")]
        public string buffer { get; set; }
        [JsonPropertyName("byteLength")]
        public int byteLength { get; set; }
        public int byteOffset { get; set; }
        public int target { get; set; }
    }
}
