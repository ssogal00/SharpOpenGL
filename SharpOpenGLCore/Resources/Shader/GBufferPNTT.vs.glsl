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

uniform mat4 NormalMatrix;


layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;
layout(location=2) in vec2 TexCoord;
layout(location=3) in vec4 Tangent;

layout(location=0) out vec4 OutPosition;
layout(location=1) out vec3 OutNormal;
layout(location=2) out vec2 OutTexCoord;
layout(location=3) out vec4 OutTangent;

  
void main()
{	
	mat4 ModelView = View * Model;
	gl_Position = Proj * View * Model * vec4(VertexPosition, 1);
	OutPosition =   (ModelView * vec4(VertexPosition, 1));	
	OutNormal =  normalize(mat3(ModelView) * VertexNormal);
	OutTexCoord = TexCoord;
	OutTangent = Tangent;
}