
#version 450 core


layout(location=0) in vec4 InPosition;
layout(location=1) in vec2 InTexCoord;
layout(location=2) in vec3 InNormal;
layout(location=3) in vec3 InTangent;
layout(location=4) in vec3 InBinormal;


layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;

void main()
{   
    DiffuseColor = vec4(0.9,0.0,0.0,0);

    NormalColor.xyz = InNormal.xyz;
    
    NormalColor.a = 0;    

    PositionColor = InPosition;
}