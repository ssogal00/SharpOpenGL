using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace SharpOpenGL
{



[StructLayout(LayoutKind.Explicit,Size=20)]
public struct TestShaderVertexAttributes
{
			
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]	
	public OpenTK.Vector3 VertexPosition;
			
	[FieldOffset(12), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]	
	public OpenTK.Vector2 TexCoord;
	}
}
