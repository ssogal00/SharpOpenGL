#version 430 core


layout (std140) uniform Transform
{
	mat4 Model;
	mat4 Proj;
	mat4 View;	
};

uniform vec3 Value;

uniform mat4 TestModel;
uniform mat4 TestProj;
uniform mat4 TestView;

layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec2 TexCoord;


layout(location = 0) out vec2 OutTexCoord;
layout(location = 1) out vec3 Position;
  
void main()
{	
	OutTexCoord = TexCoord;
	gl_Position = Proj * View * Model * vec4(VertexPosition, 1);
	Position =   (View * Model * vec4(VertexPosition, 1)).xyz;
}