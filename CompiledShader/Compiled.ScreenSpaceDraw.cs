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
namespace SharpOpenGL.ScreenSpaceDraw
{

public class ScreenSpaceDraw : MaterialBase
{
	public ScreenSpaceDraw() 
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

	public void SetColorTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"ColorTex", TextureObject);
	}

	public void SetColorTex2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"ColorTex", TextureObject);
	}

	public static string GetVSSourceCode()
	{
		return @"#version 430 core


layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec2 TexCoord;

out vec2 OutTexCoord;
  
void main()
{	
	OutTexCoord = TexCoord;	    
	gl_Position = vec4(VertexPosition.xy, 0.0, 1.0);
}";
	}

	public static string GetFSSourceCode()
	{
		return @"
#version 430 core

in vec2 OutTexCoord;

uniform sampler2D ColorTex;

out vec4 FragColor;

void main() 
{      

    FragColor = texture(ColorTex, OutTexCoord);    
}";
	}
}
}
