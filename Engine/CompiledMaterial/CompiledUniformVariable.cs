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

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
}

[Serializable]
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

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Model;
}
}
namespace GBufferMacro2
{

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
}

[Serializable]
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

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Model;
}
}
namespace ScreenSpaceDraw
{
}
namespace EquirectangleToCube
{
}
namespace GBufferDump
{

[Serializable]
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
namespace DeferredLightMaterial
{

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
}
}
namespace GeometryWireframeMaterial
{

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
}

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=32)]
public struct LineInfo
{
	[FieldOffset(0), ExposeUI]
	public System.Single Width;
	[FieldOffset(16), ExposeUI]
	public OpenTK.Mathematics.Vector4 Color;
}

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Model;
}
}
namespace BasicMaterial
{

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=16)]
public struct ColorBlock
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Vector3 Value;
}

[Serializable]
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

[Serializable]
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
namespace GBufferDraw
{

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
}

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Model;
}

[Serializable]
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
namespace CubemapConvolution
{
}
namespace GBufferInstanced
{

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
}

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Model;
}
}
namespace GBufferWithoutTexture
{

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
}

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Model;
}

[Serializable]
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
namespace GBufferPNC
{

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
}
}
namespace GBufferCubeTest
{

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
}

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Model;
}
}
namespace GBufferPNCT
{

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
}
}
namespace GBufferPNT
{

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
}

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Model;
}
}
namespace GBufferPNTT
{

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
}

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Model;
}
}
namespace Blur
{
}
namespace BloomMaterial
{
}
namespace CubemapMaterial
{
}
namespace MSGBufferMaterial
{

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
}

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Model;
}
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

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
}

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Model;
}
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
namespace FXAAMaterial
{
}
namespace TBNMaterial
{

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=128)]
public struct CameraTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 View;
	[FieldOffset(64), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Proj;
}

[Serializable]
[StructLayout(LayoutKind.Explicit,Size=64)]
public struct ModelTransform
{
	[FieldOffset(0), ExposeUI]
	public OpenTK.Mathematics.Matrix4 Model;
}
}
namespace SignedDistanceField
{
}

}
