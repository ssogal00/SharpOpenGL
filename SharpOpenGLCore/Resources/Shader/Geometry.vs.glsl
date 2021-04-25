


#version 430 core

layout (location = 0) in vec3 VertexPosition;
layout (location = 1) in vec3 VertexNormal;
layout (location = 2) in vec2 Texcoord;
layout (location = 3) in vec4 Tangent;

uniform mat4 ModelViewMatrix;
uniform mat3 NormalMatrix;
uniform mat4 ProjectionMatrix;
uniform mat4 MVP;

layout (location=0) out vec3 OutPosition;
layout (location=1) out vec3 OutNormal;
layout (location=2) out vec2 OutTexcoord;
layout (location=3) out vec4 OutTangent;


void main()
{
	OutNormal = normalize(NormalMatrix * VertexNormal);
	OutPosition = vec3(ModelViewMatrix * vec4(VertexPosition,1));
	OutTexcoord = Texcoord;
	gl_Position = MVP * vec4(VertexPosition, 1.0f);
}