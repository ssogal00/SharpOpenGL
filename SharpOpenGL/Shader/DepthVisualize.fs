
#version 450 core

in vec2 OutTexCoord;

uniform sampler2D DepthTex;
uniform float MaxDepth;

out vec4 FragColor;

void main() 
{      
    vec4 value = texture(DepthTex, OutTexCoord);
    FragColor = vec4(1, 0, 0, 1);
}