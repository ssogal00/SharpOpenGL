#version 450


layout (location = 0 , binding = 0) uniform sampler2D ColorTex;

layout (location = 0 ) in vec3 InPosition;
layout (location = 1 ) in vec2 InTexCoord;

layout( location = 0 ) out vec4 FragColor;


void main() 
{
    vec4 color = texture(ColorTex, InTexCoord);
    float brightness = dot(color.xyz, vec3(0.2126, 0.7152, 0.0722));

    if(brightness > 1.0)
        FragColor = vec4(color.xyz, 1.0f);
    else
        FragColor = vec4(0,0,0,1);
}