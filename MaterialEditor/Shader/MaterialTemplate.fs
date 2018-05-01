#version 450 core

layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;

in STAGE_OUT
{
	vec3 Position;
	vec2 TexCoord;
	vec3 Normal;
	vec3 Tangent;
	vec3 Binormal;
} stage_in;

uniform float time;

{uniformVariableDeclaration}

{sampler2DVariableDeclaration}

vec4 GetDiffuseColor()
{
    return {diffuseColorCode};
}

vec3 GetNormalColor()
{
	mat3 TangentToModelViewSpaceMatrix = mat3( stage_in.Tangent.x, stage_in.Tangent.y, stage_in.Tangent.z, 
								    stage_in.Binormal.x, stage_in.Binormal.y, stage_in.Binormal.z, 
								    stage_in.Normal.x, stage_in.Normal.y, stage_in.Normal.z);

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
    PositionColor = vec4(stage_in.Position, 0);
}