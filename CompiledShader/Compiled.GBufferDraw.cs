using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Core;
using Core.Buffer;
using Core.OpenGLShader;
using Core.Texture;
using Core.VertexCustomAttribute;
namespace SharpOpenGL.GBufferDraw
{

public class GBufferDraw
{
	ShaderProgram MaterialProgram;
	Core.OpenGLShader.VertexShader VSShader = new Core.OpenGLShader.VertexShader();
	Core.OpenGLShader.FragmentShader FSShader= new Core.OpenGLShader.FragmentShader();

	string CompileResult = "";

	public GBufferDraw()
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

	public void SetDiffuseTex2D(Core.Texture.Texture2D TextureObject)
	{
		GL.ActiveTexture(TextureUnit.Texture0);
        TextureObject.Bind();
        var Loc = MaterialProgram.GetSampler2DUniformLocation("DiffuseTex");		
		TextureObject.BindShader(TextureUnit.Texture0, Loc);
	}

	public void SetDiffuseTex2D(int TextureObject, Sampler sampler)
	{
		GL.ActiveTexture(TextureUnit.Texture0);
		GL.BindTexture(TextureTarget.Texture2D, TextureObject);
		sampler.BindSampler(TextureUnit.Texture0);
		var SamplerLoc = MaterialProgram.GetSampler2DUniformLocation("DiffuseTex");
		GL.ProgramUniform1(MaterialProgram.ProgramObject, SamplerLoc, 0);	
	}
	public void SetNormalTex2D(Core.Texture.Texture2D TextureObject)
	{
		GL.ActiveTexture(TextureUnit.Texture1);
        TextureObject.Bind();
        var Loc = MaterialProgram.GetSampler2DUniformLocation("NormalTex");		
		TextureObject.BindShader(TextureUnit.Texture1, Loc);
	}

	public void SetNormalTex2D(int TextureObject, Sampler sampler)
	{
		GL.ActiveTexture(TextureUnit.Texture1);
		GL.BindTexture(TextureTarget.Texture2D, TextureObject);
		sampler.BindSampler(TextureUnit.Texture1);
		var SamplerLoc = MaterialProgram.GetSampler2DUniformLocation("NormalTex");
		GL.ProgramUniform1(MaterialProgram.ProgramObject, SamplerLoc, 1);	
	}

	protected string GetVSSourceCode()
	{
		return @"#version 430 core


layout (std140) uniform Transform
{
	mat4 Model;
	mat4 Proj;
	mat4 View;	
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

	protected string GetFSSourceCode()
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
