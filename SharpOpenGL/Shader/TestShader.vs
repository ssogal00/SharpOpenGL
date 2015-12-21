#version 430 core


uniform Transform
{
	mat4x4 ModelView;
	mat4x4 Proj;
};

layout(location=0) in vec3 VertexColor;
layout(location=1) in vec3 VertexPosition;

out vec3 Color;

void main()
{
 Color = VertexColor;
// gl_Position = (ModelView * Proj) * vec4(VertexPosition, 1);
 gl_Position = vec4(VertexPosition, 1); 
}