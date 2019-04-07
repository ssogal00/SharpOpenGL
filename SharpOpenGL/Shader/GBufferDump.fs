#version 450

layout (location = 0 , binding = 0) uniform sampler2D PositionTex;
layout (location = 1 , binding = 1) uniform sampler2D DiffuseTex;
layout (location = 2 , binding = 2) uniform sampler2D NormalTex;
layout (location = 3 , binding = 3) uniform sampler2D MotionBlurTex;


layout (location = 0 ) in vec2 InTexCoord;

layout( location = 0 ) out vec4 FragColor;

uniform Dump
{
    bool PositionDump;
    bool NormalDump;
    bool MetalicDump;
    bool DiffuseDump;
    bool RoughnessDump;
    bool MotionBlurDump; 
};

void main() 
{
    if(PositionDump) 
    {   
        FragColor = texture(PositionTex, InTexCoord);
    }
    else if(NormalDump)
    {
        FragColor = texture(NormalTex, InTexCoord);
    }
    else if(DiffuseDump)
    {
        FragColor = texture(DiffuseTex, InTexCoord);
    }
    else if(MetalicDump)
    {
        FragColor = texture(NormalTex,InTexCoord).aaaa;
    }
    else if(RoughnessDump)
    {
   		FragColor = texture(DiffuseTex, InTexCoord).aaaa;
    }
    else if(MotionBlurDump)
    {
        FragColor = texture(MotionBlurTex, InTexCoord);
    }
    else
    {
        FragColor = texture(NormalTex,InTexCoord).aaaa;
    }
}
