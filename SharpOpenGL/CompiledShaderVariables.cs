using System.Runtime.InteropServices;
namespace SharpOpenGL
{


[StructLayout(LayoutKind.Explicit,Size=16)]
public struct VS_ColorBlock
{
			
	[FieldOffset(0)]
	public OpenTK.Vector3 Value;
	}


[StructLayout(LayoutKind.Explicit,Size=192)]
public struct VS_Transform
{
			
	[FieldOffset(0)]
	public OpenTK.Matrix4 Model;
			
	[FieldOffset(64)]
	public OpenTK.Matrix4 View;
			
	[FieldOffset(128)]
	public OpenTK.Matrix4 Proj;
	}


[StructLayout(LayoutKind.Explicit,Size=0)]
public struct TestShader_FS_
{
	}
}
