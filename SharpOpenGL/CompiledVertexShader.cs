using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace SharpOpenGL.TestShader.VertexShader
{

public class VertexShaderBase
{
	ShaderProgram VSProgram;

	VertexShaderBase(ShaderProgram programObject)
	{
		VSProgram = programObject;
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
