
#version 450 core


layout(location=0) in vec4 InPosition;
layout(location=1) in vec2 InTexCoord;
layout(location=2) in vec3 InNormal;
layout(location=3) in vec3 InLocalPosition;


layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;

layout (location=0, binding=0) uniform sampler2D EquirectangularMap;

const vec2 invAtan = vec2(0.1591, 0.3183);
vec2 SampleSphericalMap(vec3 v)
{
    vec2 uv = vec2(atan(v.z, v.x), asin(v.y));
    uv *= invAtan;
    uv += 0.5;
    return uv;
}

void main()
{  
    vec2 uv = SampleSphericalMap(normalize(InLocalPosition));
    vec3 color = texture(EquirectangularMap, uv).rgb;   
    
    DiffuseColor = vec4(color, 1.0);
    NormalColor = vec4(InNormal, 1);
    PositionColor = InPosition;
}
