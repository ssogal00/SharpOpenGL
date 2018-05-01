
#version 450 core

layout (triangles) in;

uniform Transform
{
	mat4x4 Model;
	mat4x4 View;
	mat4x4 Proj;
};

in TCS_OUT
{
	vec3 Position;
	vec2 TexCoord;
	vec3 Normal;
	vec3 Tangent;
	vec3 Binormal;
} tes_in[];

out STAGE_OUT
{
	vec3 Position;
	vec2 TexCoord;
	vec3 Normal;
	vec3 Tangent;
	vec3 Binormal;
} tes_out;

void main(void)
{
	vec4 pos = (gl_TessCoord.x * gl_in[0].gl_Position) + (gl_TessCoord.y * gl_in[1].gl_Position) + (gl_TessCoord.z * gl_in[2].gl_Position);
	gl_Position = Proj * View * Model * pos;
	tes_out.Position = pos.xyz;
	tes_out.TexCoord = (gl_TessCoord.x * tes_in[0].TexCoord) + (gl_TessCoord.y * tes_in[1].TexCoord) + (gl_TessCoord.z * tes_in[2].TexCoord);
	tes_out.Normal = (gl_TessCoord.x * tes_in[0].Normal) + (gl_TessCoord.y * tes_in[1].Normal) + (gl_TessCoord.z * tes_in[2].Normal);
	tes_out.Tangent = (gl_TessCoord.x * tes_in[0].Tangent) + (gl_TessCoord.y * tes_in[1].Tangent) + (gl_TessCoord.z * tes_in[2].Tangent);
	tes_out.Binormal = (gl_TessCoord.x * tes_in[0].Binormal) + (gl_TessCoord.y * tes_in[1].Binormal) + (gl_TessCoord.z * tes_in[2].Binormal);
}