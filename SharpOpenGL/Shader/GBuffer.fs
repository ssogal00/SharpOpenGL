
#version 450 core


layout(location=0) in vec4 InPosition;
layout(location=1) in vec2 InTexCoord;
layout(location=2) in vec3 InNormal;
layout(location=3) in vec3 InTangent;
layout(location=4) in vec3 InBinormal;


layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;

layout (location = 0, binding=0) uniform sampler2D DiffuseTex;
layout (location = 1, binding=1) uniform sampler2D NormalTex;
layout (location = 2, binding=2) uniform sampler2D MaskTex;
layout (location = 3, binding=3) uniform sampler2D MetalicTex;
layout (location = 4, binding=4) uniform sampler2D RoughnessTex;

uniform int MetalicExist;
uniform int MaskMapExist;
uniform int NormalMapExist;
uniform int RoughnessExist;
uniform int DiffuseMapExist;

uniform float Metalic = 0;
uniform float Roughness = 0;
uniform vec3 DiffuseOverride;

void main()
{   
    if(MaskMapExist > 0)
    {
    	vec4 MaskValue= texture(MaskTex, InTexCoord);
    	if(MaskValue.x > 0)
    	{
    		DiffuseColor = texture(DiffuseTex, InTexCoord);           
            //DiffuseColor.xyz = pow(DiffuseColor.xyz, vec3(1.0/2.2));  
    	}
    	else
    	{
    		discard;
    	}
    }
    else
    {
        if(DiffuseMapExist > 0)
    	{
            DiffuseColor = texture(DiffuseTex, InTexCoord);
            //DiffuseColor.xyz = pow(DiffuseColor.xyz, vec3(1.0/2.2));  
        }
        else
        {
            DiffuseColor = vec4(DiffuseOverride,0);
           // DiffuseColor.xyz = pow(DiffuseColor.xyz, vec3(1.0/2.2));  
        }
    }

    if(RoughnessExist > 0)
    {
        DiffuseColor.a = texture(RoughnessTex, InTexCoord).x;
    }
    else
    {
        DiffuseColor.a = Roughness;
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
        vec3 NormalMapNormal = (2.0f * (texture( NormalTex, InTexCoord ).xyz) - vec3(1.0f));
	    vec3 BumpNormal = normalize(TangentToModelViewSpaceMatrix * NormalMapNormal.xyz);
	
        NormalColor.xyz = BumpNormal.xyz;
    }
    else
    {
        NormalColor.xyz = InNormal.xyz;
    }

    if(MetalicExist > 0)
    {
        NormalColor.a = texture(MetalicTex, InTexCoord).x;        
    }
    else
    {
        NormalColor.a = Metalic;
    }

    PositionColor = InPosition;
}
