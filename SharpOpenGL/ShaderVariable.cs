using System.Runtime.InteropServices;
namespace SharpOpenGL
{


[StructLayout(LayoutKind.Explicit,Size=48)]
public struct TestShader_VS_Uniforms
{
			
	[FieldOffset(0)]
	public OpenTK.Vector3 translation;
			
	[FieldOffset(12)]
	public float scale;
			
	[FieldOffset(16)]
	public OpenTK.Vector4 rotation;
			
	[FieldOffset(32)]
	public bool enabled;
	}



[StructLayout(LayoutKind.Explicit,Size=48)]
public struct TestShader_FS_Uniforms
{
			
	[FieldOffset(0)]
	public OpenTK.Vector3 translation;
			
	[FieldOffset(12)]
	public float scale;
			
	[FieldOffset(16)]
	public OpenTK.Vector4 rotation;
			
	[FieldOffset(32)]
	public bool enabled;
	}

}
