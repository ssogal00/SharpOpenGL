
#version 450 core


layout(location=0) in vec4 InPosition;
layout(location=1) in vec2 InTexCoord;
layout(location=2) in vec3 InNormal;
layout(location=3) in vec3 InTangent;
layout(location=4) in vec3 InBinormal;


layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;

layout (location = 0, binding=0) uniform sampler2DMS DiffuseTex;
layout (location = 1, binding=1) uniform sampler2DMS NormalTex;
layout (location = 2, binding=2) uniform sampler2DMS MaskTex;
layout (location = 3, binding=3) uniform sampler2DMS SpecularTex;

uniform int SpecularMapExist;
uniform int MaskMapExist;
uniform int NormalMapExist;

void main()
{   
    ivec2 TexCoord = ivec2(InTexCoord);
    if(MaskMapExist > 0)
    {
        vec4 MaskValue = texelFetch(MaskTex, TexCoord,0);
    	if(MaskValue.x > 0)
    	{
    		DiffuseColor = texelFetch(DiffuseTex, TexCoord,0);            
    	}
    	else
    	{
    		discard;
    	}
    }
    else
    {
    	DiffuseColor = texelFetch(DiffuseTex, TexCoord,0);
    }

    if(InPosition.w == 0)
    {
        DiffuseColor = vec4(1,0,0,0);
    }

    mat3 TangentToModelViewSpaceMatrix = mat3( InTangent.x, InTangent.y, InTangent.z, 
								    InBinormal.x, InBinormal.y, InBinormal.z, 
								    InNormal.x, InNormal.y, InNormal.z);

    if(NormalMapExist > 0)
    {
        vec3 NormalMapNormal = (2.0f * (texelFetch( NormalTex, TexCoord,0 ).xyz) - vec3(1.0f));
	    vec3 BumpNormal = normalize(TangentToModelViewSpaceMatrix * NormalMapNormal.xyz);
	
        NormalColor.xyz = BumpNormal.xyz;
    }
    else
    {
        NormalColor.xyz = InNormal.xyz;
    }

    if(SpecularMapExist > 0)
    {
        NormalColor.a = texelFetch(SpecularTex, TexCoord,0).x;
    }
    else
    {
        NormalColor.a = 0;
    }

    PositionColor = InPosition;
}