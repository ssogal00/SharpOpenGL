using System.Runtime.InteropServices;
namespace SharpOpenGL
{



[StructLayout(LayoutKind.Explicit,Size=20)]
public struct TestShaderVertexAttributes
{
			
	[FieldOffsetAttribute(0), ComponentCount(1)]	
	public OpenTK.Vector3 vColor;
			
	[FieldOffsetAttribute(12), ComponentCount(1)]	
	public OpenTK.Vector2 vPos;
	}
}
