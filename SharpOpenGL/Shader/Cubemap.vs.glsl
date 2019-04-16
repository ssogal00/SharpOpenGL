#version 450 core

layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;
layout(location=2) in vec2 TexCoord;
layout(location=3) in vec4 Tangent;

uniform mat4 ViewMatrix;
uniform mat4 ProjMatrix;
uniform mat4 ModelMatrix;

layout(location=0) out vec3 OutTexCoord;
layout(location=1) out vec2 TexCoord2;
  
void main()
{	
	vec4 vPosition = ProjMatrix * ViewMatrix * ModelMatrix * vec4(VertexPosition, 1.0);
	OutTexCoord = VertexPosition.xyz;
	TexCoord2 = TexCoord;
	//gl_Position = vec4(vPosition.xy, 0, 1.0);
	gl_Position = vPosition.xyww;
}
