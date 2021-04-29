


uniform LineInfo
{
	float Width;
	vec4 Color;
};

//layout (location=0) out vec3 GNormal;
//layout (location=1) out vec3 GPosition;
//layout (location=2) out vec2 GTexCoord;
//layout (location=3) out vec4 GTangent;
//layout (location=4) noperspective out vec3 GEdgeDistance;

layout (location=0) in vec3 GNormal;
layout (location=1) in vec3 GPosition;
layout (location=2) in vec2 GTexCoord;
layout (location=3) in vec4 GTangent;
layout (location=4) noperspective in vec3 GEdgeDistance;

layout (location=0) out vec4 FragColor;

uniform sampler2D diffuseTex;

void main()
{
	float d = min(GEdgeDistance.x, GEdgeDistance.y);
	d = min(d, GEdgeDistance.z);

	float mixVal = smoothstep(Line.Width-1,
	Line.Width+1, d);

	vec4 Color = texture(diffuseTex, GTexCoord);

	FragColor = mix(Line.Color, Color, mixVal);
}