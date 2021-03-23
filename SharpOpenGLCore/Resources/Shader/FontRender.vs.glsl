#version 450 core

layout(location=0) in vec2 VertexPosition;
layout(location=1) in vec2 VertexTexCoord;
layout(location=2) in vec4 VertexColor;

uniform vec2 ScreenSize;

layout(location=0) out vec2 TexCoord;
layout(location=1) out vec4 OutColor;
  
void main()
{	
	TexCoord = VertexTexCoord;
	OutColor = VertexColor;
	float fX = ((VertexPosition.x - ScreenSize.x * .5f) * 2.f) / ScreenSize.x;
	float fY = ((VertexPosition.y - ScreenSize.y * .5f) * 2.f) / ScreenSize.y;

	gl_Position = vec4(fX, -fY, 0.0, 1.0);
}