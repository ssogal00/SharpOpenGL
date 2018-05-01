
#version 450 core

layout (vertices=3) out;


in STAGE_OUT
{
	vec3 Position;
	vec2 TexCoord;
	vec3 Normal;
	vec3 Tangent;
	vec3 Binormal;
} tcs_in[];

out TCS_OUT
{
	vec3 Position;
	vec2 TexCoord;
	vec3 Normal;
	vec3 Tangent;
	vec3 Binormal;
} tcs_out[];

void main(void)
{
	if(gl_InvocationID == 0)
	{
		gl_TessLevelInner[0] = 3.0;
		gl_TessLevelOuter[0] = 3.0;
		gl_TessLevelOuter[1] = 3.0;
		gl_TessLevelOuter[2] = 3.0;
	}
	gl_out[gl_InvocationID].gl_Position = gl_in[gl_InvocationID].gl_Position;

	tcs_out[gl_InvocationID].Position = tcs_in[gl_InvocationID].Position;
	tcs_out[gl_InvocationID].TexCoord = tcs_in[gl_InvocationID].TexCoord;
	tcs_out[gl_InvocationID].Normal = tcs_in[gl_InvocationID].Normal;
	tcs_out[gl_InvocationID].Tangent = tcs_in[gl_InvocationID].Tangent;
	tcs_out[gl_InvocationID].Binormal = tcs_in[gl_InvocationID].Binormal;
}