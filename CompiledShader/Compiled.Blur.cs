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
namespace SharpOpenGL.Blur
{

public class Blur : MaterialBase
{
	public Blur() 
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
		return @"#version 430

layout (location = 0) in vec3 VertexPosition;
layout (location = 1) in vec2 VertexTexCoord;

out vec2 TexCoord;

void main()
{
    TexCoord = VertexTexCoord;  
	gl_Position = vec4(VertexPosition.xy, 0.0, 1.0);
}
";
	}

	public static string GetFSSourceCode()
	{
		return @"#version 430

in vec2 TexCoord;

uniform sampler2D ColorTex;
uniform vec2 BlurOffsets[9];
uniform vec2 BlurWeights[9];

layout( location = 0 ) out vec4 FragColor;

void main() 
{
	vec4 color = vec4(0,0,0,0);
    
    for( int i = 0; i < 9; i++ )
    {
        color += (texture(ColorTex, (TexCoord + BlurOffsets[i]))) * BlurWeights[i].x;        
    }
	        
    FragColor = color;
}";
	}
}
}
