#version 450 core

uniform ModelTransform
{
	mat4x4 Model;
};

uniform CameraTransform
{
	mat4x4 View;
	mat4x4 Proj;
};

layout(location=0) in vec3 VertexPosition;
  
void main()
{	
	gl_Position = Proj * View * Model * vec4(VertexPosition, 1);
}