#version 450 core

layout(location=0) in vec2 TexCoord;
layout(location=1) in vec4 Color;

uniform vec3 TextColor;
uniform sampler2D FontTexture;

out vec4 FragColor;

void main()
{   	
    FragColor = Color;
}