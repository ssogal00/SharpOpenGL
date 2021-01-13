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
namespace CompiledMaterial
{
namespace GBufferMacro1
{


[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
}


[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Model;
}
}
namespace GBufferMacro1
{


[StructLayout(LayoutKind.Explicit,Size=32)]
public struct MaterialProperty
{
	[FieldOffset(0), ExposeUI]
	public System.UInt32 encodedPBRInfo;
	[FieldOffset(4), ExposeUI]
	public System.Boolean MetallicExist;
	[FieldOffset(8), ExposeUI]
	public System.Boolean RoghnessExist;
	[FieldOffset(12), ExposeUI]
	public System.Boolean MaskExist;
	[FieldOffset(16), ExposeUI]
	public System.Boolean NormalExist;
	[FieldOffset(20), ExposeUI]
	public System.Single Metallic;
	[FieldOffset(24), ExposeUI]
	public System.Single Roughness;
	[FieldOffset(28), ExposeUI]
	public System.Boolean MetallicRoughnessOneTexture;
}
}
namespace GBufferMacro2
{


[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
}


[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Model;
}
}
namespace GBufferMacro2
{


[StructLayout(LayoutKind.Explicit,Size=32)]
public struct MaterialProperty
{
	[FieldOffset(0), ExposeUI]
	public System.UInt32 encodedPBRInfo;
	[FieldOffset(4), ExposeUI]
	public System.Boolean MetallicExist;
	[FieldOffset(8), ExposeUI]
	public System.Boolean RoghnessExist;
	[FieldOffset(12), ExposeUI]
	public System.Boolean MaskExist;
	[FieldOffset(16), ExposeUI]
	public System.Boolean NormalExist;
	[FieldOffset(20), ExposeUI]
	public System.Single Metallic;
	[FieldOffset(24), ExposeUI]
	public System.Single Roughness;
	[FieldOffset(28), ExposeUI]
	public System.Boolean MetallicRoughnessOneTexture;
}
}
namespace BasicMaterial
{


[StructLayout(LayoutKind.Explicit,Size=16)]
public struct ColorBlock
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Vector3 Value;
}


[StructLayout(LayoutKind.Explicit,Size=192)]
public struct Transform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Model;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(128), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
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
	public OpenTK.Mathematics.Matrix4 Model;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(128), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
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
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
}


[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Model;
}


[StructLayout(LayoutKind.Explicit,Size=192)]
public struct PrevTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 PrevProj;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 PrevModel;
	[FieldOffset(128), ExposeUI]
	public OpenTK.Mathematics.Matrix4 PrevView;
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
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
}


[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Model;
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
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
}


[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Model;
}


[StructLayout(LayoutKind.Explicit,Size=192)]
public struct PrevTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 PrevProj;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 PrevModel;
	[FieldOffset(128), ExposeUI]
	public OpenTK.Mathematics.Matrix4 PrevView;
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
	public System.Boolean PositionDump;
	[FieldOffset(4), ExposeUI]
	public System.Boolean NormalDump;
	[FieldOffset(8), ExposeUI]
	public System.Boolean MetalicDump;
	[FieldOffset(12), ExposeUI]
	public System.Boolean DiffuseDump;
	[FieldOffset(16), ExposeUI]
	public System.Boolean RoughnessDump;
	[FieldOffset(20), ExposeUI]
	public System.Boolean MotionBlurDump;
}
}
namespace GBufferPNC
{


[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
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
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
}


[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Model;
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
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
}
}
namespace GBufferPNCT
{
}
namespace GBufferPNT
{


[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
}


[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Model;
}
}
namespace GBufferPNT
{
}
namespace GBufferPNTT
{


[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
}


[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Model;
}
}
namespace GBufferPNTT
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
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
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
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
}


[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Model;
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
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
}


[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Model;
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
namespace FXAAMaterial
{
}
namespace TBNMaterial
{


[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
}


[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Model;
}
}
namespace TBNMaterial
{
}
namespace SignedDistanceField
{
}

}
