


uniform LineInfo
{
	float Width;
	vec4 Color;
} Line;


layout (location=0) in vec3 GNormal;
layout (location=1) in vec3 GPosition;
layout (location=2) noperspective in vec3 GEdgeDistance;

layout (location=0) out vec4 FragColor;



void main()
{
	float d = min(GEdgeDistance.x, GEdgeDistance.y);
	d = min(d, GEdgeDistance.z);

	float mixVal = smoothstep(Line.Width-1,
	Line.Width+1, d);

}