#version 450 core


layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec2 TexCoord;

uniform mat4 ViewMatrix;

layout(location=0) out vec3 OutTexCoord;
  
void main()
{	
	OutTexCoord = mat3(ViewMatrix) * VertexPosition;
	gl_Position = vec4(VertexPosition.xyz, 1.0);
}
