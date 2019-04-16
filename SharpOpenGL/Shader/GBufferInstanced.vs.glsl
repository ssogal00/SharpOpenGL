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
layout(location=1) out vec2 OutTexCoord;
layout(location=2) out vec3 OutNormal;
layout(location=3) out vec3 OutTangent;
layout(location=4) out vec3 OutBinormal;
layout(location=5) out vec2 OutMetallicRoughness;

uniform int RoughnessCount;
uniform int MetallicCount;
  
void main()
{
	int metallicIndex = gl_InstanceID / MetallicCount;
	int roughnessIndex = gl_InstanceID % RoughnessCount;	

	float metallicValue = float(metallicIndex)  / float(MetallicCount);	
	float roughnessValue = float(roughnessIndex) / float(RoughnessCount);

	vec4 translation = vec4( 10, roughnessValue * 150 + 10, metallicValue * 150 - 75, 1);

	OutMetallicRoughness = vec2(metallicValue,  roughnessValue);

	mat4 ModelView = View * Model;

	OutTexCoord = TexCoord;
	gl_Position = Proj * ModelView *  (vec4(VertexPosition, 1) + translation);
	OutPosition =   (ModelView *  (vec4(VertexPosition, 1) + translation) );
	
	OutNormal =  normalize(mat3(ModelView) * VertexNormal);	

	OutTangent = normalize(mat3(ModelView) * vec3(Tangent));

	vec3 binormal = (cross( VertexNormal, Tangent.xyz )) * Tangent.w;
	OutBinormal = normalize(mat3(ModelView) * binormal);	
	
}