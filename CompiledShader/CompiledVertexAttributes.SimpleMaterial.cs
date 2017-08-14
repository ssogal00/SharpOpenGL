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



[StructLayout(LayoutKind.Explicit,Size=12)]
public struct VertexAttribute
{
	
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Vector3 VertexPosition;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 12, new IntPtr(0));
	}
}

}
