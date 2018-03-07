
#version 430 core


layout(location=0) in vec3 InPosition;
layout(location=1) in vec2 InTexCoord;
layout(location=2) in vec3 InNormal;
layout(location=3) in vec3 InTangent;
layout(location=4) in vec3 InBinormal;


layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;

uniform sampler2D DiffuseTex;
uniform sampler2D NormalTex;
uniform sampler2D MaskTex;

uniform int MaskMapExist;

void main()
{   
    if(MaskMapExist > 0)
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

    mat3 TangentToModelViewSpaceMatrix = mat3( InTangent.x, InTangent.y, InTangent.z, 
								   InBinormal.x, InBinormal.y, InBinormal.z, 
								   InNormal.x, InNormal.y, InNormal.z);
    

    vec4 NormalMapNormal = (2.0f * (texture( NormalTex, InTexCoord )) - 1.0f);			
	vec3 BumpNormal = normalize(TangentToModelViewSpaceMatrix * NormalMapNormal.xyz);			

    if(length(BumpNormal) > 0)
	{
		BumpNormal = normalize(BumpNormal);
	}
	else
	{
		BumpNormal = vec3(1,1,1);
	}

	// NormalColor = texture(NormalTex, InTexCoord);
    NormalColor.xyz = BumpNormal;
    PositionColor = vec4(InPosition, 0);
}