using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Core;
using Core.Buffer;
using Core.OpenGLShader;
using Core.Texture;
using Core.VertexCustomAttribute;
namespace SharpOpenGL.BasicMaterial
{

public class BasicMaterial
{
	ShaderProgram MaterialProgram;
	Core.OpenGLShader.VertexShader VSShader = new Core.OpenGLShader.VertexShader();
	Core.OpenGLShader.FragmentShader FSShader= new Core.OpenGLShader.FragmentShader();

	string CompileResult = "";

	public BasicMaterial()
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
		ColorBlockBuffer = new Core.Buffer.DynamicUniformBuffer(ProgramObject, @"ColorBlock");
		TransformBuffer = new Core.Buffer.DynamicUniformBuffer(ProgramObject, @"Transform");
	}
	Core.Buffer.DynamicUniformBuffer ColorBlockBuffer;
	Core.Buffer.DynamicUniformBuffer TransformBuffer;

	public void SetColorBlockBlockData(ref ColorBlock Data)
	{
		var Loc = MaterialProgram.GetUniformBlockIndex("ColorBlock");
		ColorBlockBuffer.Bind();
		// ColorBlockBuffer.BindBufferBase(Loc);
		ColorBlockBuffer.BindBufferBase(0);
		ColorBlockBuffer.BufferData<ColorBlock>(ref Data);
		// ColorBlockBuffer.BufferWholeData<ColorBlock>(ref Data);
	}
	public void SetTransformBlockData(ref Transform Data)
	{
		var Loc = MaterialProgram.GetUniformBlockIndex("Transform");
		TransformBuffer.Bind();
		// TransformBuffer.BindBufferBase(Loc);
		TransformBuffer.BindBufferBase(0);
		TransformBuffer.BufferData<Transform>(ref Data);
		// TransformBuffer.BufferWholeData<Transform>(ref Data);
	}

	public void SetTestTexture2D(Core.Texture.Texture2D TextureObject)
	{
		GL.ActiveTexture(TextureUnit.Texture0);
        TextureObject.Bind();
        var Loc = MaterialProgram.GetSampler2DUniformLocation("TestTexture");
		// GL.Uniform1(Loc, (int)(0));
		TextureObject.BindShader(TextureUnit.Texture0, Loc);
	}

	protected string GetVSSourceCode()
	{
		return @"#version 430 core


uniform Transform
{
	mat4x4 Model;
	mat4x4 View;
	mat4x4 Proj;
};

uniform ColorBlock
{
	vec3 Value;
};

layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec2 TexCoord;

out vec3 Color;
out vec2 OutTexCoord;

void main()
{
	Color = vec3(1,0,0);
	OutTexCoord = TexCoord;
	gl_Position = ( Proj * View * Model) * vec4(VertexPosition, 1);
}";
	}

	protected string GetFSSourceCode()
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
