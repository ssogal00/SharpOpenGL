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

public class BasicMaterialMaterial
{
	ShaderProgram MaterialProgram;
	Core.OpenGLShader.VertexShader VSShader;
	Core.OpenGLShader.FragmentShader FSShader;

	public BasicMaterialMaterial()
	{
		MaterialProgram = new Core.OpenGLShader.ShaderProgram();
		
		VSShader.CompileShader(GetVSSourceCode());
		FSShader.ComplieShader(GetFSSourceCode());

		MaterialProgram.AttachShader(VSShader);
		MaterialProgram.AttachShader(FSShader);
		
		MaterialProgram.Link();	

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

	public void SetTestTexture2D(Core.Texture.Texture2D TextureObject)
	{
		GL.ActiveTexture(TextureUnit.Texture0);
        TextureObject.Bind();
        var Loc = FSProgram.GetSampler2DUniformLocation("TestTexture");
		GL.Uniform1(Loc, (int)(0));
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
