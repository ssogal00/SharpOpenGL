#version 450 core

#if VERTEX_PNTT

layout(location=0) in vec4 InPosition;
layout(location=1) in vec3 InNormal;
layout(location=2) in vec2 InTexCoord;
layout(location=3) in vec3 InTangent;
layout(location=4) in vec3 InBinormal;

#elif VERTEX_PNT

layout(location=0) in vec4 InPosition;
layout(location=1) in vec3 InNormal;
layout(location=2) in vec2 InTexCoord;

#elif VERTEX_PNC

layout(location=0) in vec4 InPosition;
layout(location=1) in vec3 InNormal;
layout(location=2) in vec3 InColor;

#endif

layout (location = 0, binding=0) uniform sampler2D DiffuseTex;
layout (location = 1, binding=1) uniform sampler2D NormalTex;
layout (location = 2, binding=2) uniform sampler2D MaskTex;
layout (location = 3, binding=3) uniform sampler2D MetallicTex;
layout (location = 4, binding=4) uniform sampler2D RoughnessTex;

layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;
layout (location = 3) out vec4 VelocityColor;

uniform MaterialProperty
{
	bool MetallicExist;
	bool RoghnessExist;
	bool MaskExist;
	bool NormalExist;
	float Metallic;
	float Roughness;
	float DiffuseColor;	
};

void main()
{   
	if(MaskExist)
	{
		vec4 MaskValue= texture(MaskTex, InTexCoord);
		if(MaskValue.x > 0)
		{
			DiffuseColor = texture(DiffuseTex, InTexCoord);
		}
		else
		{
			discard;
		}    
	}
	else
	{
		DiffuseColor = texture(DiffuseTex, InTexCoord);    
	}

	if(RoghnessExist)
    {
    	DiffuseColor.a = texture(RoughnessTex, InTexCoord).x;
    }
    else
    {
    	DiffuseColor.a = Roughness;
	}

#if VERTEX_PNTT
	if(NormalExist)
    {
    	mat3 TangentToModelViewSpaceMatrix = mat3( InTangent.x, InTangent.y, InTangent.z, 
								    InBinormal.x, InBinormal.y, InBinormal.z, 
								    InNormal.x, InNormal.y, InNormal.z);    
    
	    vec3 NormalMapNormal = (2.0f * (texture( NormalTex, InTexCoord ).xyz) - vec3(1.0f));
	    vec3 BumpNormal = normalize(TangentToModelViewSpaceMatrix * NormalMapNormal.xyz);

	    NormalColor.xyz = BumpNormal.xyz;
    }
    else
    {
    	NormalColor.xyz = InNormal.xyz;
	}
#else
	NormalColor = InNormal.xyz;
#endif
    
	if(MetallicExist)
    {
    	NormalColor.a = texture(MetalicTex, InTexCoord).x;
	}
	else
    {
    	NormalColor.a = Metallic;
	}

    PositionColor = InPosition;
}
