using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShaderCompilerCore
{
    public class ShaderMacro
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class ShaderCompileInfo
    {
        [JsonPropertyName("vertexShader")]
        public string VertexShaderPath { get; set; }

        [JsonPropertyName("fragmentShader")]
        public string FragmentShaderPath { get; set; }

        [JsonPropertyName("vertexShaderDefines")]
        public List<ShaderMacro> VertexShaderDefines { get; set; }

        [JsonPropertyName("fragmentShaderDefines")]
        public List<ShaderMacro> FragmentShaderDefines { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class ShaderListToCompile
    {
        [JsonPropertyName("shaders")]
        public List<ShaderCompileInfo> ShaderList { get; set; }
    }
}