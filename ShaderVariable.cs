using System.Runtime.InteropServices;
namespace SharpOpenGL
{


[StructLayout(LayoutKind.Explicit)]
public struct TestShader_VS_Uniforms
{
			
	[FieldOffset(32)]
	public bool enabled;
			
	[FieldOffset(16)]
	public OpenTK.Vector4 rotation;
			
	[FieldOffset(12)]
	public float scale;
			
	[FieldOffset(0)]
	public OpenTK.Vector3 translation;
	}



[StructLayout(LayoutKind.Explicit)]
public struct TestShader_FS_Uniforms
{
			
	[FieldOffset(32)]
	public bool enabled;
			
	[FieldOffset(16)]
	public OpenTK.Vector4 rotation;
			
	[FieldOffset(12)]
	public float scale;
			
	[FieldOffset(0)]
	public OpenTK.Vector3 translation;
	}

}
