using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Core;
using Core.Buffer;
using Core.OpenGLShader;
using Core.Texture;
using Core.VertexCustomAttribute;
namespace SharpOpenGL.TestShader.VertexShader
{

public interface IVertexShaderInterface
{
	void SetColorBlockBlockData(ref ColorBlock Data);
	void SetTransformBlockData(ref Transform Data);
	
}

public class VertexShaderBase
{
	ShaderProgram VSProgram;
	Core.OpenGLShader.VertexShader VSShader;

	public VertexShaderBase(ShaderProgram programObject)
	{
		VSProgram = programObject;
		VSShader.CompileShader(GetShaderSourceCode());
		VSProgram.AttachShader(VSShader);
		Initialize();
	}

	public void Initialize()
	{
		ColorBlockBuffer = new Core.Buffer.DynamicUniformBuffer();
		TransformBuffer = new Core.Buffer.DynamicUniformBuffer();
	}
	Core.Buffer.DynamicUniformBuffer ColorBlockBuffer;
	Core.Buffer.DynamicUniformBuffer TransformBuffer;

	public void SetColorBlockBlockData(ref ColorBlock Data)
	{
		var Loc = VSProgram.GetUniformBlockIndex("ColorBlock");
		ColorBlockBuffer.BindBufferBase(Loc);
		ColorBlockBuffer.BufferWholeData<ColorBlock>(ref Data);
	}
	public void SetTransformBlockData(ref Transform Data)
	{
		var Loc = VSProgram.GetUniformBlockIndex("Transform");
		TransformBuffer.BindBufferBase(Loc);
		TransformBuffer.BufferWholeData<Transform>(ref Data);
	}

	protected string GetShaderSourceCode()
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
}
}
