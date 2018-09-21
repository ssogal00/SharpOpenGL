#version 450 core


layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec2 TexCoord;

uniform mat4 ViewMatrix;

layout(location=0) out vec3 OutTexCoord;
layout(location=1) out vec2 TexCoord2;
  
void main()
{	
	OutTexCoord = mat3(ViewMatrix) * VertexPosition;
	TexCoord2 = TexCoord;
	gl_Position = vec4(VertexPosition.xy, 0, 1.0);
}
