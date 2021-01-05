
#version 450 core


layout(location=0) in vec4 InPosition;
layout(location=1) in vec3 InNormal;
layout(location=2) in vec2 InTexCoord;



layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;


layout (location = 0, binding=0) uniform sampler2D DiffuseTex;
layout (location = 1, binding=1) uniform sampler2D NormalTex;
layout (location = 2, binding=2) uniform sampler2D MaskTex;
layout (location = 3, binding=3) uniform sampler2D MetalicTex;
layout (location = 4, binding=4) uniform sampler2D RoughnessTex;

void main()
{   	
	DiffuseColor = texture(DiffuseTex, InTexCoord);
    NormalColor = vec4(InNormal.xyz,0);    
    PositionColor = InPosition;
}