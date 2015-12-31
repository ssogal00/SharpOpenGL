
#version 430 core

in vec3 Color;
out vec4 FragColor;

uniform sampler2D TestTexture;

void main()
{

    //FragColor = vec4(Color, 1.0);
    FragColor = texture(TestTexture, vec2(0.5,0.5));
}