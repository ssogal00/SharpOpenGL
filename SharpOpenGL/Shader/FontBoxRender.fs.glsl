#version 450 core

in vec2 TexCoord;

uniform vec3 BoxColor;
uniform float BoxAlpha;

out vec4 FragColor;

void main()
{   
    FragColor =vec4(BoxColor, BoxAlpha);
}