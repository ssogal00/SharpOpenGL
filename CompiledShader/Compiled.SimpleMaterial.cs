using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Core;
using Core.Buffer;
using Core.OpenGLShader;
using Core.Texture;
using Core.VertexCustomAttribute;
namespace SharpOpenGL.SimpleMaterial
{

public class SimpleMaterial
{
	ShaderProgram MaterialProgram;
	Core.OpenGLShader.VertexShader VSShader = new Core.OpenGLShader.VertexShader();
	Core.OpenGLShader.FragmentShader FSShader= new Core.OpenGLShader.FragmentShader();

	string CompileResult = "";

	public SimpleMaterial()
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
		TransformBuffer = new Core.Buffer.DynamicUniformBuffer(ProgramObject, @"Transform");
	}
	Core.Buffer.DynamicUniformBuffer TransformBuffer;

	public void SetTransformBlockData(ref Transform Data)
	{
		var Loc = MaterialProgram.GetUniformBlockIndex("Transform");
		TransformBuffer.Bind();		
		TransformBuffer.BindBufferBase(0);
		TransformBuffer.BufferData<Transform>(ref Data);		
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

layout(location=0) in vec3 VertexPosition;

void main()
{		
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

void main()
{   
    FragColor = vec4(1,0,0,0);
}";
	}
}
}
