
#version 450 core


layout(location=0) in vec4 InPosition;
layout(location=1) in vec2 InTexCoord;
layout(location=2) in vec3 InNormal;
layout(location=3) in vec3 InTangent;
layout(location=4) in vec3 InBinormal;
layout(location=5) in vec2 MetallicRoughness;


layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;

layout (location = 0, binding=0) uniform sampler2D DiffuseTex;
layout (location = 1, binding=1) uniform sampler2D NormalTex;
layout (location = 2, binding=2) uniform sampler2D MaskTex;
layout (location = 3, binding=3) uniform sampler2D MetalicTex;
layout (location = 4, binding=4) uniform sampler2D RoughnessTex;

uniform bool MetalicExist;
uniform bool MaskMapExist;
uniform bool NormalMapExist;
uniform bool RoughnessExist;
uniform bool DiffuseMapExist;
uniform int LightChannel = 0;
uniform vec3 DiffuseOverride;

void main()
{   
    if(DiffuseMapExist)
	{
        DiffuseColor = texture(DiffuseTex, InTexCoord);            
    }
    else
    {
        DiffuseColor = vec4(DiffuseOverride,0);           
    }    

    if(RoughnessExist)
    {
        DiffuseColor.a = texture(RoughnessTex, InTexCoord).x;
    }
    else
    {
        DiffuseColor.a = MetallicRoughness.y;
    }

    if(InPosition.w == 0)
    {
        DiffuseColor = vec4(1,0,0,0);
    }

    mat3 TangentToModelViewSpaceMatrix = mat3( InTangent.x, InTangent.y, InTangent.z, 
								    InBinormal.x, InBinormal.y, InBinormal.z, 
								    InNormal.x, InNormal.y, InNormal.z);

    if(NormalMapExist)
    {
        vec3 NormalMapNormal = (2.0f * (texture( NormalTex, InTexCoord ).xyz) - vec3(1.0f));
	    vec3 BumpNormal = normalize(TangentToModelViewSpaceMatrix * NormalMapNormal.xyz);
	
        NormalColor.xyz = BumpNormal.xyz;
    }
    else
    {
        NormalColor.xyz = InNormal.xyz;
    }

    if(MetalicExist)
    {
        NormalColor.a = texture(MetalicTex, InTexCoord).x;        
    }
    else
    {
        NormalColor.a = MetallicRoughness.x;
    }

    PositionColor = InPosition;
	PositionColor.a = LightChannel;
}
