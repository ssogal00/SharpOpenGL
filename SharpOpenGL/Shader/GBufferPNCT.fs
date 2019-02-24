
#version 450 core


layout(location=0) in vec4 InPosition;
layout(location=1) in vec3 InVertexColor;
layout(location=2) in vec3 InNormal;
layout(location=3) in vec2 InTexCoord;


layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;

layout (location = 0, binding=0) uniform sampler2D SpecularTex;

void main()
{   	
	DiffuseColor = vec4(InVertexColor, 1.0);    	
    NormalColor = vec4(InNormal.xyz,0);
    NormalColor.a = texture(SpecularTex, InTexCoord).x;
    PositionColor = InPosition;
}