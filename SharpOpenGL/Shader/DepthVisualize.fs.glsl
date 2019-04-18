
#version 450 core

layout (location=0) in vec2 InTexCoord;
layout (location=0) out vec4 FragColor;

layout (location=0, binding=0) uniform sampler2D DepthTex;


void main() 
{   
    FragColor = texture(DepthTex, InTexCoord);
}