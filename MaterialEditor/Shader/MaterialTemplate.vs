#version 450 core


uniform Transform
{
	mat4x4 Model;
	mat4x4 View;
	mat4x4 Proj;
};

layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;
layout(location=2) in vec2 TexCoord;
layout(location=3) in vec4 Tangent;


out STAGE_OUT
{
	vec3 Position;
	vec2 TexCoord;
	vec3 Normal;
	vec3 Tangent;
	vec3 Binormal;
} stage_out;


  
void main()
{	
	mat4 ModelView = View * Model;

	stage_out.TexCoord = TexCoord;
	
	gl_Position = Proj * View * Model * vec4(VertexPosition, 1);	
	
	stage_out.Position =   (ModelView * vec4(VertexPosition, 1)).xyz;
	
	stage_out.Normal =  normalize(mat3(ModelView) * VertexNormal);	

	stage_out.Tangent = normalize(mat3(ModelView) * vec3(Tangent));

	vec3 binormal = (cross( VertexNormal, Tangent.xyz )) * Tangent.w;
	stage_out.Binormal = normalize(mat3(ModelView) * binormal);	
}