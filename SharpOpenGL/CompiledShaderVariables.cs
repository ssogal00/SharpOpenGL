using System.Runtime.InteropServices;
namespace SharpOpenGL
{


[StructLayout(LayoutKind.Explicit,Size=48)]
public struct TestShader_VS_Uniforms
{
			
	[FieldOffset(0)]
	public OpenTK.Vector3 translation;
			
	[FieldOffset(12)]
	public System.Single scale;
			
	[FieldOffset(16)]
	public OpenTK.Vector4 rotation;
			
	[FieldOffset(32)]
	public System.Boolean enabled;
	}


[StructLayout(LayoutKind.Explicit,Size=48)]
public struct TestShader_FS_Uniforms
{
			
	[FieldOffset(0)]
	public OpenTK.Vector3 translation;
			
	[FieldOffset(12)]
	public System.Single scale;
			
	[FieldOffset(16)]
	public OpenTK.Vector4 rotation;
			
	[FieldOffset(32)]
	public System.Boolean enabled;
	}
}
