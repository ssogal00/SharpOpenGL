#version 450 core

uniform mat4x4 Model;
uniform mat4x4 View;
uniform mat4x4 Proj;

layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec2 TexCoord;

layout(location=0) out vec2 OutTexCoord;
  
void main()
{	
	gl_Position = Proj * View * Model * vec4(VertexPosition, 1);
	OutTexCoord = TexCoord;	
}