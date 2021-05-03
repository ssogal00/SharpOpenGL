


#version 460 core

layout (location = 0) in vec3 VertexPosition;
layout (location = 1) in vec3 VertexNormal;
layout (location = 2) in vec2 Texcoord;
layout (location = 3) in vec4 Tangent;

uniform mat3x3 NormalMatrix;

uniform ModelTransform
{
	mat4x4 Model;
};

uniform CameraTransform
{
	mat4x4 View;
	mat4x4 Proj;
};

layout (location=0) out vec3 OutPosition;
layout (location=1) out vec3 OutNormal;
layout (location=2) out vec2 OutTexcoord;
layout (location=3) out vec4 OutTangent;


void main()
{
	mat4 ModelView = View * Model;
	mat4 MVP = Proj * View * Model;
	OutNormal = normalize(NormalMatrix * VertexNormal);
	OutPosition = vec3(ModelView * vec4(VertexPosition,1));
	OutTexcoord = Texcoord;
	gl_Position = MVP * vec4(VertexPosition, 1.0f);
}