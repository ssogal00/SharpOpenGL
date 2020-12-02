#version 450 core

layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec2 VertexTexCoord;

uniform vec2 ScreenSize;

out vec2 TexCoord;
  
void main()
{	
	TexCoord = VertexTexCoord;
	float fX = ((VertexPosition.x - ScreenSize.x * .5f) * 2.f) / ScreenSize.x;
	float fY = ((VertexPosition.y - ScreenSize.y * .5f) * 2.f) / ScreenSize.y;

	gl_Position = vec4(fX, fY, 0.0, 1.0);
}