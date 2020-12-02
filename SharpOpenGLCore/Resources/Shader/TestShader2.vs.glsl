#version 450 core


uniform Transform
{
	mat4x4 Model;
	mat4x4 View;
	mat4x4 Proj;
};

layout(location=0) in vec3 VertexPosition;

void main()
{		
	gl_Position = ( Proj * View * Model) * vec4(VertexPosition, 1);
}