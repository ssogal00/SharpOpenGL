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

layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;
layout (location = 3) out vec4 VelocityColor;


#if NORMAL_EXIST
uniform sampler2D NormalTex;
#endif


#if METALLIC_EXIST
uniform sampler2D MetallicTex;
#else
uniform float Metallic = 0;
#endif

#if ROUGHNESS_EXIST
uniform sampler2D RoughnessTex;
#else
uniform float Roughness = 0;
#endif

#if MASK_EXIST
uniform sampler2D MaskTex;
#endif

#if DIFFUSE_EXIST
uniform sampler2D DiffuseTex;
#else
uniform vec3 DiffuseColor;
#endif

void main()
{   
#if MASK_EXIST
	vec4 MaskValue= texture(MaskTex, InTexCoord);
	if(MaskValue.x > 0)
	{
		DiffuseColor = texture(DiffuseTex, InTexCoord);
	}
	else
	{
		discard;
	}    
#else
	DiffuseColor = texture(DiffuseTex, InTexCoord);    
#endif

#if ROUGHNESS_EXIST
    DiffuseColor.a = texture(RoughnessTex, InTexCoord).x;
#else
    DiffuseColor.a = Roughness;
#endif   

#if VERTEX_PNTT && NORMAL_EXIST
    mat3 TangentToModelViewSpaceMatrix = mat3( InTangent.x, InTangent.y, InTangent.z, 
								    InBinormal.x, InBinormal.y, InBinormal.z, 
								    InNormal.x, InNormal.y, InNormal.z);    
    
    vec3 NormalMapNormal = (2.0f * (texture( NormalTex, InTexCoord ).xyz) - vec3(1.0f));
    vec3 BumpNormal = normalize(TangentToModelViewSpaceMatrix * NormalMapNormal.xyz);

    NormalColor.xyz = BumpNormal.xyz;
#else    
    NormalColor.xyz = InNormal.xyz;
#endif    
    
#if METALLIC_EXIST    
    NormalColor.a = texture(MetalicTex, InTexCoord).x;        
#else
    NormalColor.a = Metalic;
#endif    

    PositionColor = InPosition;
}
