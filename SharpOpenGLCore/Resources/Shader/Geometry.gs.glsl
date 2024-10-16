﻿
#version 460

layout (triangles) in;
layout (triangle_strip, max_vertices=3) out;

layout (location=0) out vec4 GPosition;
layout (location=1) out vec3 GNormal;
layout (location=2) out vec2 GTexCoord;
layout (location=3) out vec3 GTangent;
layout (location=4) out vec3 GBinormal;
layout (location=5) noperspective out vec3 GEdgeDistance;

layout (location=0) in vec4 VPosition[];
layout (location=1) in vec3 VNormal[];
layout (location=2) in vec2 VTexcoord[];
layout (location=3) in vec3 VTangent[];
layout (location=4) in vec3 VBinormal[];

//layout (location=0) out vec3 OutPosition;
//layout (location=1) out vec3 OutNormal;
//layout (location=2) out vec2 OutTexcoord;
//layout (location=3) out vec4 OutTangent;


uniform mat4 ViewportMatrix;

void main()
{
	// Transform each vertex into viewport space
	vec3 p0 = vec3(ViewportMatrix * (gl_in[0].gl_Position / gl_in[0].gl_Position.w));
	vec3 p1 = vec3(ViewportMatrix * (gl_in[1].gl_Position / gl_in[1].gl_Position.w));
	vec3 p2 = vec3(ViewportMatrix * (gl_in[2].gl_Position / gl_in[2].gl_Position.w));
	// Find the altitudes (ha, hb and hc)
	float a = length(p1 - p2);
	float b = length(p2 - p0);
	float c = length(p1 - p0);
	float alpha = acos( (b*b + c*c - a*a) / (2.0*b*c) );
	float beta = acos( (a*a + c*c - b*b) / (2.0*a*c) );
	float ha = abs( c * sin( beta ) );
	float hb = abs( c * sin( alpha ) );
	float hc = abs( b * sin( alpha ) );
	// Send the triangle along with the edge distances
	GEdgeDistance = vec3( ha, 0, 0 );

	GNormal = VNormal[0];
	GPosition = VPosition[0];
	GTexCoord = VTexcoord[0];
	GTangent = VTangent[0];
	GBinormal = VBinormal[0];
	gl_Position = gl_in[0].gl_Position;
	EmitVertex();

	GEdgeDistance = vec3( 0, hb, 0 );
	GNormal = VNormal[1];
	GPosition = VPosition[1];
	GTexCoord = VTexcoord[1];
	GTangent = VTangent[1];
	GBinormal = VBinormal[1];
	gl_Position = gl_in[1].gl_Position;
	EmitVertex();

	GEdgeDistance = vec3( 0, 0, hc );
	GNormal = VNormal[2];
	GPosition = VPosition[2];
	GTexCoord = VTexcoord[2];
	GTangent = VTangent[2];
	GBinormal = VBinormal[2];
	gl_Position = gl_in[2].gl_Position;
	EmitVertex();

	EndPrimitive();
}