#version 430 core


uniform Transform
{
	mat4x4 Model;
	mat4x4 View;
	mat4x4 Proj;
};


uniform vec3 Value;


layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;
layout(location=2) in vec2 TexCoord;
layout(location=3) in vec4 Tangent;


layout(location=0) out vec3 OutPosition;
layout(location=1) out vec2 OutTexCoord;
layout(location=2) out vec3 OutNormal;
layout(location=3) out vec3 OutTangent;
layout(location=4) out vec3 OutBinormal;

  
void main()
{	
	mat4 ModelView = View * Model;

	OutTexCoord = TexCoord;
	gl_Position = Proj * View * Model * vec4(VertexPosition, 1);
	OutPosition =   (ModelView * vec4(VertexPosition, 1)).xyz;
	
	OutNormal =  (ModelView * vec4(VertexNormal,0.0)).xyz;	

	if(length(OutNormal) > 	0)
	{
		OutNormal = normalize(OutNormal);
	}

	OutTangent = (ModelView * Tangent).xyz;

	if(length(OutTangent) > 0)
	{
		OutTangent = normalize(OutTangent);
	}

	vec3 binormal = ( cross( VertexNormal, Tangent.xyz ) * Tangent.w) ;	
	OutBinormal = normalize((ModelView * vec4(binormal, 0.0)).xyz);	

	if(length(OutBinormal) > 0)
	{
		OutBinormal = normalize(OutBinormal);
	}
}