using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Core;
using Core.Buffer;
using Core.OpenGLShader;
using Core.Texture;
using Core.VertexCustomAttribute;
using Core.MaterialBase;
namespace SharpOpenGL.GBufferDraw
{

public class GBufferDraw : MaterialBase
{
	public GBufferDraw() 
	 : base (GetVSSourceCode(), GetFSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return MaterialProgram;
	}

	public void Use()
	{
		MaterialProgram.UseProgram();
	}

	public void SetDiffuseTex2D(Core.Texture.Texture2D TextureObject)
	{
		SetTexture(@"DiffuseTex", TextureObject);
	}

	public void SetDiffuseTex2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"DiffuseTex", TextureObject);
	}
	public void SetNormalTex2D(Core.Texture.Texture2D TextureObject)
	{
		SetTexture(@"NormalTex", TextureObject);
	}

	public void SetNormalTex2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"NormalTex", TextureObject);
	}

	public static string GetVSSourceCode()
	{
		return @"#version 430 core


uniform Transform
{
	mat4x4 Model;
	mat4x4 View;
	mat4x4 Proj;
};


uniform vec3 Value;

uniform mat4 TestModel;
uniform mat4 TestProj;
uniform mat4 TestView;

layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec2 TexCoord;


layout(location = 0) out vec2 OutTexCoord;
layout(location = 1) out vec3 Position;
  
void main()
{	
	OutTexCoord = TexCoord;
	gl_Position = Proj * View * Model * vec4(VertexPosition, 1);
	Position =   (View * Model * vec4(VertexPosition, 1)).xyz;
}";
	}

	public static string GetFSSourceCode()
	{
		return @"
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
}";
	}
}
}
