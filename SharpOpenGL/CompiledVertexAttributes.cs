using System.Runtime.InteropServices;
namespace SharpOpenGL
{



[StructLayout(LayoutKind.Explicit,Size=24)]
public struct TestShaderVertexAttributes
{
			
	[FieldOffset(0), ComponentCount(1)]	
	public OpenTK.Vector3 VertexColor;
			
	[FieldOffset(12), ComponentCount(1)]	
	public OpenTK.Vector3 VertexPosition;
	}
}
