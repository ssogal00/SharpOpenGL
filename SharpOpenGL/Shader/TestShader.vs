#version 430 core


uniform Transform
{
	mat4x4 Model;
	mat4x4 View;
	mat4x4 Proj;
};

uniform ColorBlock
{
	vec3 Value;
};

layout(location=0) in vec3 VertexPosition;

out vec3 Color;

void main()
{
 Color = vec3(1,0,0);
 gl_Position = ( Proj * View * Model) * vec4(VertexPosition, 1);
//gl_Position = vec4(VertexPosition, 1); 
}