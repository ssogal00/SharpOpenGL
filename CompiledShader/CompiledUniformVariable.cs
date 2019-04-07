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
using ZeroFormatter;
using ZeroFormatter.Formatters;
using Core.CustomAttribute;
namespace SharpOpenGL
{
namespace BasicMaterial
{


[StructLayout(LayoutKind.Explicit,Size=16)]
public struct ColorBlock
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Vector3 Value;
}


[StructLayout(LayoutKind.Explicit,Size=192)]
public struct Transform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Matrix4 Model;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Matrix4 View;
	[FieldOffset(128), ExposeUI]
	public OpenTK.Matrix4 Proj;
}
}
namespace BasicMaterial
{
}
namespace SimpleMaterial
{


[StructLayout(LayoutKind.Explicit,Size=192)]
public struct Transform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Matrix4 Model;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Matrix4 View;
	[FieldOffset(128), ExposeUI]
	public OpenTK.Matrix4 Proj;
}
}
namespace SimpleMaterial
{
}
namespace ScreenSpaceDraw
{
}
namespace GBufferDraw
{


[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Matrix4 Proj;
}


[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Matrix4 Model;
}
}
namespace GBufferDraw
{
}
namespace EquirectangleToCube
{
}
namespace CubemapConvolution
{
}
namespace GBufferInstanced
{


[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Matrix4 Proj;
}


[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Matrix4 Model;
}
}
namespace GBufferInstanced
{
}
namespace GBufferWithoutTexture
{


[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Matrix4 Proj;
}


[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Matrix4 Model;
}
}
namespace GBufferWithoutTexture
{
}
namespace GBufferDump
{
}
namespace GBufferDump
{


[StructLayout(LayoutKind.Explicit,Size=32)]
public struct Dump
{
	[FieldOffset(0), ExposeUI]
	public System.Int32 PositionDump;
	[FieldOffset(4), ExposeUI]
	public System.Int32 NormalDump;
	[FieldOffset(8), ExposeUI]
	public System.Int32 MetalicDump;
	[FieldOffset(12), ExposeUI]
	public System.Int32 DiffuseDump;
	[FieldOffset(16), ExposeUI]
	public System.Int32 RoughnessDump;
}
}
namespace GBufferPNC
{


[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Matrix4 Proj;
}
}
namespace GBufferPNC
{
}
namespace GBufferCubeTest
{


[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Matrix4 Proj;
}


[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Matrix4 Model;
}
}
namespace GBufferCubeTest
{
}
namespace GBufferPNCT
{


[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Matrix4 Proj;
}
}
namespace GBufferPNCT
{
}
namespace Blur
{
}
namespace BloomMaterial
{
}
namespace LightMaterial
{
}
namespace LightMaterial
{


[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Matrix4 Proj;
}
}
namespace CubemapMaterial
{
}
namespace MSGBufferMaterial
{


[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Matrix4 Proj;
}


[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Matrix4 Model;
}
}
namespace MSGBufferMaterial
{
}
namespace DepthVisualizeMaterial
{
}
namespace FontRenderMaterial
{
}
namespace FontBoxRenderMaterial
{
}
namespace GridRenderMaterial
{


[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Matrix4 Proj;
}


[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Matrix4 Model;
}
}
namespace GridRenderMaterial
{
}
namespace ThreeDTextRenderMaterial
{
}
namespace ResolveMaterial
{
}
namespace SSAOMaterial
{
}
namespace LUTGenerateMaterial
{
}
namespace PrefilterMaterial
{
}
namespace HDAOMaterial
{
}

}
