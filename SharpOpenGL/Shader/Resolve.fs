#version 450

out vec4 FragColor;
  
layout (location = 0) in vec2 TexCoords;

uniform sampler2D ColorTex;
uniform sampler2D BlurTex; 

void main()
{
	FragColor = texture(ColorTex, TexCoords) + texture(BlurTex, TexCoords);
}