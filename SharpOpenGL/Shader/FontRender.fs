#version 450 core

in vec2 TexCoord;

uniform vec3 TextColor;
uniform sampler2D FontTexture;

out vec4 FragColor;

void main()
{   
	vec4 TexCol = texture(FontTexture, TexCoord);
    FragColor =vec4(1,0,0,TexCol.a);
}