
#version 450 core

in vec2 OutTexCoord;

layout (binding = 0) uniform sampler2D ColorTex;

out vec4 FragColor;

void main() 
{      

    FragColor = texture(ColorTex, OutTexCoord);    
}