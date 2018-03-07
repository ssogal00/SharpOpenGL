using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Core;
using Core.Buffer;
using Core.OpenGLShader;
using Core.Texture;
using Core.VertexCustomAttribute;
using Core.MaterialBase;
namespace SharpOpenGL.LightMaterial
{


[StructLayout(LayoutKind.Explicit,Size=64)]
public struct Light
{
	[FieldOffset(0)]
	public OpenTK.Vector3 LightDir;
	[FieldOffset(16)]
	public OpenTK.Vector3 LightAmbient;
	[FieldOffset(32)]
	public OpenTK.Vector3 LightDiffuse;
	[FieldOffset(48)]
	public OpenTK.Vector3 LightSpecular;
	[FieldOffset(60)]
	public System.Single LightSpecularShininess;
}

}
