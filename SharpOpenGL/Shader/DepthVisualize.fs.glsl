
#version 450 core

layout (location=0) in vec2 InTexCoord;
layout (location=0) out vec4 FragColor;

layout (location=0, binding=0) uniform sampler2D DepthTex;

uniform float Near; 
uniform float Far;

void main() 
{   
	float depth = texture(DepthTex, InTexCoord).x;
	float linearizedDepth = (2.0 * Near) / (Far + Near - depth * (Far - Near));
    FragColor = vec4(linearizedDepth, linearizedDepth, linearizedDepth, 1.0f);
}