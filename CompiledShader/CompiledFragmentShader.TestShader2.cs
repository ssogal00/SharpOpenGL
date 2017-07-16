using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Core;
using Core.Buffer;
using Core.OpenGLShader;
using Core.Texture;
using Core.VertexCustomAttribute;
namespace CompiledShader.TestShader2.FragmentShader
{

public interface IFragmentShaderInterface
{
	void SetTestTexture2D(Core.Texture.Texture2D TextureObject);
}

public class FragmentShaderBase
{
	ShaderProgram FSProgram;
	Core.OpenGLShader.FragmentShader FSShader;

	public FragmentShaderBase(ShaderProgram programObject)
	{
		FSProgram = programObject;			
		FSShader.CompileShader(GetShaderSourceCode());
		FSProgram.AttachShader(FSShader);
	}
	public void SetTestTexture2D(Core.Texture.Texture2D TextureObject)
	{
		GL.ActiveTexture(TextureUnit.Texture0);
        TextureObject.Bind();
        var Loc = FSProgram.GetSampler2DUniformLocation("TestTexture");
		GL.Uniform1(Loc, (int)(0));
	}
	protected string GetShaderSourceCode()
	{
		return @"
#version 430 core

in vec3 Color;
in vec2 OutTexCoord;
out vec4 FragColor;

uniform sampler2D TestTexture;


void main()
{   
    FragColor = texture(TestTexture, OutTexCoord);
}";
	}
}
}
