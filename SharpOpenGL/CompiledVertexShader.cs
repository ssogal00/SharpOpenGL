using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SharpOpenGL;
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

	public VertexShaderBase(ShaderProgram programObject)
	{
		VSProgram = programObject;
		Initialize();
	}

	public void Initialize()
	{
		ColorBlockBuffer = new SharpOpenGL.Buffer.DynamicUniformBuffer();
		TransformBuffer = new SharpOpenGL.Buffer.DynamicUniformBuffer();
	}
	SharpOpenGL.Buffer.DynamicUniformBuffer ColorBlockBuffer;
	SharpOpenGL.Buffer.DynamicUniformBuffer TransformBuffer;

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
}
}
