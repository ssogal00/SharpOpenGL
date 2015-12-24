using System.Runtime.InteropServices;
namespace SharpOpenGL
{



[StructLayout(LayoutKind.Explicit,Size=12)]
public struct TestShaderVertexAttributes
{
			
	[FieldOffset(0), ComponentCount(3)]	
	public OpenTK.Vector3 VertexPosition;
	}
}
