using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Core;
using Core.Buffer;
using Core.OpenGLShader;
using Core.Texture;
using Core.VertexCustomAttribute;
using Core.MaterialBase;
namespace SharpOpenGL.GBufferDraw
{



[StructLayout(LayoutKind.Explicit,Size=48)]
public struct VertexAttribute
{
	
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Vector3 VertexPosition;
		
	[FieldOffset(12), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Vector3 VertexNormal;
		
	[FieldOffset(24), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Vector2 TexCoord;
		
	[FieldOffset(32), ComponentCount(4), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Vector4 Tangent;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 48, new IntPtr(0));
		GL.EnableVertexAttribArray(1);
		GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 48, new IntPtr(12));
		GL.EnableVertexAttribArray(2);
		GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 48, new IntPtr(24));
		GL.EnableVertexAttribArray(3);
		GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, 48, new IntPtr(32));
	}
}

}
