
#version 450 core

layout (location=0) in vec3 CubemapTexCoord;
layout (location=1) in vec2 InTexCoord;

layout (location=0, binding=0) uniform samplerCube texCubemap;


layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;
layout (location = 3) out vec4 VelocityColor;

uniform int LightChannel = 0;

void main()
{
    vec4 Color = texture(texCubemap, -CubemapTexCoord);    
    DiffuseColor = Color;
	PositionColor.a = LightChannel;
}