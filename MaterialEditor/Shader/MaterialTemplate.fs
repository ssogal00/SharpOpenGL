#version 430 core

layout(location=0) in vec3 InPosition;
layout(location=1) in vec2 InTexCoord;
layout(location=2) in vec3 InNormal;
layout(location=3) in vec3 InTangent;
layout(location=4) in vec3 InBinormal;

layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;

uniform float time;

{uniformVariableDeclaration}

{sampler2DVariableDeclaration}

vec4 GetDiffuseColor()
{
    return {diffuseColorCode};
}

vec3 GetNormalColor()
{
	mat3 TangentToModelViewSpaceMatrix = mat3( InTangent.x, InTangent.y, InTangent.z, 
								    InBinormal.x, InBinormal.y, InBinormal.z, 
								    InNormal.x, InNormal.y, InNormal.z);

	vec3 NormalMapNormal = (2.0f * (({normalColorCode}).xyz) - vec3(1.0f));
	vec3 BumpNormal = normalize(TangentToModelViewSpaceMatrix * NormalMapNormal.xyz);

	return BumpNormal;
}


float GetSpecularColor()
{
	return {specularColorCode};
}


void main()
{   
    DiffuseColor = GetDiffuseColor();
    NormalColor.xyz = GetNormalColor();
	NormalColor.a = GetSpecularColor();
    PositionColor = vec4(InPosition, 0);
}