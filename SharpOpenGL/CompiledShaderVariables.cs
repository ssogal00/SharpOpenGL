using System.Runtime.InteropServices;
namespace SharpOpenGL
{


[StructLayout(LayoutKind.Explicit,Size=128)]
public struct TestShader_VS_Transform
{
			
	[FieldOffset(0)]
	public OpenTK.Matrix4 ModelView;
			
	[FieldOffset(64)]
	public OpenTK.Matrix4 Proj;
	}


[StructLayout(LayoutKind.Explicit,Size=0)]
public struct TestShader_FS_
{
	}
}
