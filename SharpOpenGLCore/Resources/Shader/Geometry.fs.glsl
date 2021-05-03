


uniform LineInfo
{
	float Width;
	vec4 Color;
};

layout (location=0) in vec3 GPosition;
layout (location=1) in vec3 GNormal;
layout (location=2) in vec2 GTexCoord;
layout (location=3) in vec3 GTangent;
layout (location=4) in vec3 GBinormal;
layout (location=5) noperspective in vec3 GEdgeDistance;

layout (location=0) out vec4 FragColor;

uniform sampler2D DiffuseTex;

void main()
{
	float d = min(GEdgeDistance.x, GEdgeDistance.y);
	d = min(d, GEdgeDistance.z);

	float mixVal = smoothstep(Width-1,	Width+1, d);

	vec4 diffuseColor = texture(DiffuseTex, GTexCoord);

	FragColor = mix(Color, diffuseColor, mixVal);
}