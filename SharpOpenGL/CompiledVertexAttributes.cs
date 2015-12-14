using System.Runtime.InteropServices;
namespace SharpOpenGL
{



[StructLayout(LayoutKind.Explicit,Size=20)]
public struct TestShaderVertexAttributes
{
			
	[FieldOffset(0)]
	public OpenTK.Vector3 vColor;
			
	[FieldOffset(12)]
	public OpenTK.Vector2 vPos;
	}
}
