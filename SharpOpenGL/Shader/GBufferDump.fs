#version 450

layout (location = 0 , binding = 0) uniform sampler2D PositionTex;
layout (location = 1 , binding = 1) uniform sampler2D DiffuseTex;
layout (location = 2 , binding = 2) uniform sampler2D NormalTex;


layout (location = 0 ) in vec2 InTexCoord;

layout( location = 0 ) out vec4 FragColor;

uniform Dump
{
    int PositionDump;
    int NormalDump;
    int MetalicDump;
    int DiffuseDump;
    int RoughnessDump;
};

void main() 
{
    if(PositionDump > 0) 
    {   
        FragColor = texture(PositionTex, InTexCoord);
    }
    else if(NormalDump > 0)
    {
        FragColor = texture(NormalTex, InTexCoord);
    }
    else if(DiffuseDump > 0)
    {
        FragColor = texture(DiffuseTex, InTexCoord);
    }
    else if(MetalicDump > 0)
    {
        FragColor = texture(NormalTex,InTexCoord).aaaa;
    }
    else if(RoughnessDump > 0)
    {
   		FragColor = texture(DiffuseTex, InTexCoord).aaaa;
    }
    else
    {
        FragColor = texture(NormalTex,InTexCoord).aaaa;
    }
}
