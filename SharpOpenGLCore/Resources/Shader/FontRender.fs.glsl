#version 450 core

layout(location=0) in vec2 TexCoord;
layout(location=1) in vec4 Color;

layout(binding=0) uniform sampler2D FontTexture;

layout(location = 0) out vec4 PositionColor;
layout(location = 1) out vec4 DiffuseColor;
layout(location = 2) out vec4 NormalColor;
layout(location = 3) out vec4 VelocityColor;

void main()
{   	
    DiffuseColor = texture(FontTexture, TexCoord);
    
}