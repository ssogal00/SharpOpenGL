using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GLTF.V2
{
    public class GLTF_V2
    {
        public List<Accessor> accessors { get; set; }
        public List<BufferView> bufferViews { get; set; }
        public List<Buffer> buffers { get; set; }
        public List<Camera> cameras { get; set; }
        public List<Mesh> meshes { get; set; }

        public AssetInfo asset { get; set; }
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
        public Attributes attributes { get; set; }
        public int indices { get; set; }
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
        public string type { get; set; }
        public int count { get; set; }
        /// <summary>
        /// 5120 : BYTE
        /// 5121 : UNSIGNED_BYTE
        /// 5122 : SHORT
        /// 5123 : UNSIGNED_SHORT
        /// 5125 : UNSIGNED_INT
        /// 5126 : FLOAT
        /// </summary>
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
