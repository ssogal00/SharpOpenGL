using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharpOpenGLCore
{
    public class ShaderCompileInfo
    {
        [JsonPropertyName("vertexShader")] 
        public string VertexShaderPath { get; set; }

        [JsonPropertyName("fragmentShader")]
        public string FragmentShaderPath { get; set; }

        [JsonPropertyName("vertexShaderDefines")]
        public List<string> VertexShaderDefines { get; set; }

        [JsonPropertyName("fragmentShaderDefines")]
        public List<string> FragmentShaderDefines { get; set; }

        [JsonPropertyName("name")] 
        public string Name;
    }

    public class ShaderListToCompile
    {
        [JsonPropertyName("shaders")]
        public List<ShaderCompileInfo> ShaderList { get; set; }
    }
}
