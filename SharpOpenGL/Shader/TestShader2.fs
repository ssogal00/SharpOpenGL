
#version 430 core

in vec3 Color;
in vec2 OutTexCoord;
out vec4 FragColor;

uniform sampler2D TestTexture;


void main()
{   
    FragColor = texture(TestTexture, OutTexCoord);
}