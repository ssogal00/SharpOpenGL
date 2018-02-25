using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Core;
using Core.Buffer;
using Core.OpenGLShader;
using Core.Texture;
using Core.VertexCustomAttribute;
namespace SharpOpenGL.ScreenSpaceDraw
{

public class ScreenSpaceDraw
{
	ShaderProgram MaterialProgram;
	Core.OpenGLShader.VertexShader VSShader = new Core.OpenGLShader.VertexShader();
	Core.OpenGLShader.FragmentShader FSShader= new Core.OpenGLShader.FragmentShader();

	string CompileResult = "";

	public ScreenSpaceDraw()
	{
		MaterialProgram = new Core.OpenGLShader.ShaderProgram();
		
		VSShader.CompileShader(GetVSSourceCode());
		FSShader.CompileShader(GetFSSourceCode());

		MaterialProgram.AttachShader(VSShader);
		MaterialProgram.AttachShader(FSShader);	
		
		MaterialProgram.LinkProgram( out CompileResult );	

		Initialize(MaterialProgram);
	}

	public ShaderProgram GetProgramObject()
	{
		return MaterialProgram;
	}

	public void Use()
	{
		MaterialProgram.UseProgram();
	}

	public void Initialize(ShaderProgram ProgramObject)
	{
	}


	public void SetColorTex2D(Core.Texture.Texture2D TextureObject)
	{
		GL.ActiveTexture(TextureUnit.Texture0);
        TextureObject.Bind();
        var Loc = MaterialProgram.GetSampler2DUniformLocation("ColorTex");
		// GL.Uniform1(Loc, (int)(0));
		TextureObject.BindShader(TextureUnit.Texture0, Loc);
	}

	protected string GetVSSourceCode()
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

	protected string GetFSSourceCode()
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
