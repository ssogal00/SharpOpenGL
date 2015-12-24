using System.Runtime.InteropServices;
namespace SharpOpenGL
{



[StructLayout(LayoutKind.Explicit,Size=20)]
public struct TestShaderVertexAttributes
{
			
	[FieldOffset(0), ComponentCount(3)]	
	public OpenTK.Vector3 VertexPosition;
			
	[FieldOffset(12), ComponentCount(2)]	
	public OpenTK.Vector2 TexCoord;
	}
}
