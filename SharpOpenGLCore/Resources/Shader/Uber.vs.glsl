

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

//
#if VERTEX_PNTT
#define TANGENT_EXIST 1
#define TEXCOORD_EXIST 1
layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;
layout(location=2) in vec2 TexCoord;
layout(location=3) in vec4 Tangent;

layout(location=0) out vec4 OutPosition;
layout(location=1) out vec3 OutNormal;
layout(location=2) out vec2 OutTexCoord;
layout(location=3) out vec3 OutTangent;
layout(location=4) out vec3 OutBinormal;

#elif VERTEX_PNT
#define TEXCOORD_EXIST 1
layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;
layout(location=2) in vec2 TexCoord;

layout(location=0) out vec4 OutPosition;
layout(location=1) out vec3 OutNormal;
layout(location=2) out vec2 OutTexCoord;

#elif VERTEX_PNC
#define VERTEXCOLOR_EXIST 1
#define TEXCOORD_EXIST 0
layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;
layout(location=2) in vec3 VertexColor;

layout(location=0) out vec4 OutPosition;
layout(location=1) out vec3 OutNormal;
layout(location=2) out vec3 OutVertexColor;
#endif
  
void main()
{	
	//
	mat4 ModelView = View * Model;
	gl_Position = Proj * View * Model * vec4(VertexPosition, 1);

#if TEXCOORD_EXIST	
	OutTexCoord = TexCoord;		
#endif

#if VERTEXCOLOR_EXIST
	OutVertexColor = VertexColor;
#endif

	OutPosition =   (ModelView * vec4(VertexPosition, 1));
	OutNormal =  normalize(mat3(ModelView) * VertexNormal);	

#if TANGENT_EXIST
	OutTangent = normalize(mat3(ModelView) * vec3(Tangent));
	vec3 binormal = (cross( VertexNormal, Tangent.xyz )) * Tangent.w;
	OutBinormal = normalize(mat3(ModelView) * binormal);	
#endif
}