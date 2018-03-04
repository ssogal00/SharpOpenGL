#version 430 core


uniform Transform
{
	mat4x4 Model;
	mat4x4 View;
	mat4x4 Proj;
};


uniform vec3 Value;

uniform mat4 TestModel;
uniform mat4 TestProj;
uniform mat4 TestView;

layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;
layout(location=2) in vec2 TexCoord;
layout(location=3) in vec4 Tangent;

layout(location = 0) out vec2 OutTexCoord;
layout(location = 1) out vec3 Position;
  
void main()
{	
	OutTexCoord = TexCoord;
	gl_Position = Proj * View * Model * vec4(VertexPosition, 1);
	Position =   (View * Model * vec4(VertexPosition, 1)).xyz;
}