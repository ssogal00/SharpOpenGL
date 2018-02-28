
#version 430 core


layout (location = 0) in vec2 InTexCoordValue;
layout (location = 1) in vec3 Position;

layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;

uniform sampler2D DiffuseTex;
uniform sampler2D NormalTex;
uniform sampler2D MaskTex;

uniform int MaskMapExist;

// subroutine vec4 ShadeModelType(vec2 TexCoord);
// subroutine uniform ShadeModelType shadeModel;

// subroutine (ShadeModelType)
// vec4 DiffuseWithoutMaskMap(vec2 TexCoord)
// {
// 	return texture(DiffuseTex, TexCoord);
// }

// subroutine (ShadeModelType)
// vec4 DiffuseWithMaskMap(vec2 TexCoord)
// {
// 	return texture(NormalTex, TexCoord);
// }

void main()
{   
    DiffuseColor = texture(DiffuseTex, InTexCoordValue);
    PositionColor = vec4(Position, 0);
    NormalColor = texture(NormalTex, InTexCoordValue);
}