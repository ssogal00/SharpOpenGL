
#if VERTEX_PNTT
layout(location=0) in vec4 InPosition;
layout(location=1) in vec3 InNormal;
layout(location=2) in vec2 InTexCoord;
layout(location=3) in vec3 InTangent;
layout(location=4) in vec3 InBinormal;
#if WIRE_FRAME
layout (location=5) noperspective in vec3 GEdgeDistance;
#endif

#elif VERTEX_PNT

layout(location=0) in vec4 InPosition;
layout(location=1) in vec3 InNormal;
layout(location=2) in vec2 InTexCoord;

#elif VERTEX_PNC

layout(location=0) in vec4 InPosition;
layout(location=1) in vec3 InNormal;
layout(location=2) in vec3 InColor;

#endif

#if WIRE_FRAME
uniform LineInfo
{
	float Width;
	vec4 WireframeColor;
};
#endif
layout (location = 0, binding=0) uniform sampler2D DiffuseTex;
layout (location = 1, binding=1) uniform sampler2D NormalTex;
layout (location = 2, binding=2) uniform sampler2D MaskTex;
layout (location = 3, binding=3) uniform sampler2D MetallicTex;
layout (location = 4, binding=4) uniform sampler2D RoughnessTex;
layout(location = 5, binding = 5) uniform sampler2D MetallicRoughnessTex;

layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;
layout (location = 3) out vec4 VelocityColor;

uniform MaterialProperty
{
	// 00000000 00000000 00000000 00000000
	// 00000000
	// bit position 0 => metallicExist;
	// bit position 1 => roghnessExist;
	// bit position 2 => maskExist;
	// bit position 3 => normalExist;
	// bit position 4 => occlusionExist;


	uint encodedPBRInfo;
	bool MetallicExist;
	bool RoghnessExist;
	bool MaskExist;
	bool NormalExist;
	float Metallic;
	float Roughness;
	bool MetallicRoughnessOneTexture;
};

float GetMetallicValue(vec2 texcoord)
{
	if (MetallicExist)
	{
		if (MetallicRoughnessOneTexture)
		{
			return texture(MetallicRoughnessTex, texcoord).b;
		}
		else
		{
			return texture(MetallicTex, texcoord).b;
		}
	}
	else
	{
		return Metallic;
	}
}

float GetRoughnessValue(vec2 texcoord)
{
	if (MetallicExist)
	{
		if (MetallicRoughnessOneTexture)
		{
			return texture(MetallicRoughnessTex, texcoord).g;
		}
		else
		{
			return texture(RoughnessTex, texcoord).g;
		}
	}
	else
	{
		return Roughness;
	}
}

vec4 GetDiffuseColor(vec2 texcoord)
{
	vec4 diffuseColor = texture(DiffuseTex,texcoord);
#if WIRE_FRAME
	float d = min(GEdgeDistance.x, GEdgeDistance.y);
	d = min(d, GEdgeDistance.z);
	float mixVal = smoothstep(Width-1,	Width+1, d);
	return mix(WireframeColor, diffuseColor, mixVal);	
#else
	return diffuseColor;
#endif
}

void main()
{   
	if(MaskExist)
	{
		vec4 MaskValue= texture(MaskTex, InTexCoord);
		if(MaskValue.x > 0)
		{

			DiffuseColor = GetDiffuseColor(InTexCoord);
		}
		else
		{
			discard;
		}    
	}
	else
	{
		DiffuseColor = GetDiffuseColor(InTexCoord);
	}
	
	DiffuseColor.a = GetRoughnessValue(InTexCoord);    

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
	NormalColor.xyz = InNormal.xyz;
#endif
    	
	NormalColor.a = GetMetallicValue(InTexCoord);	

    PositionColor = InPosition;
}
