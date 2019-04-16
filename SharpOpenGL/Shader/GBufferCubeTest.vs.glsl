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


layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;

layout(location=0) out vec4 OutPosition;
layout(location=1) out vec3 OutNormal;
layout(location=2) out vec3 LocalPosition;
  
void main()
{	
	mat4 ModelView = View * Model;

	LocalPosition = VertexPosition;

	gl_Position = Proj * View * Model * vec4(VertexPosition, 1);
	OutPosition =   (ModelView * vec4(VertexPosition, 1));
	
	OutNormal =  normalize(mat3(ModelView) * VertexNormal);		
}