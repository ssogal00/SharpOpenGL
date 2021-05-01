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
namespace BasicMaterial
{


[StructLayout(LayoutKind.Explicit,Size=12)]
public struct VertexAttribute
{
	
	[Index(0)]
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexPosition;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 12, new IntPtr(0));
	}
}
}
namespace SimpleMaterial
{


[StructLayout(LayoutKind.Explicit,Size=12)]
public struct VertexAttribute
{
	
	[Index(0)]
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexPosition;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 12, new IntPtr(0));
	}
}
}
namespace GBufferDraw
{


[StructLayout(LayoutKind.Explicit,Size=48)]
public struct VertexAttribute
{
	
	[Index(0)]
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexPosition;
		
	[Index(1)]
	[FieldOffset(12), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexNormal;
		
	[Index(2)]
	[FieldOffset(24), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector2 TexCoord;
		
	[Index(3)]
	[FieldOffset(32), ComponentCount(4), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector4 Tangent;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 48, new IntPtr(0));
		GL.EnableVertexAttribArray(1);
		GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 48, new IntPtr(12));
		GL.EnableVertexAttribArray(2);
		GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 48, new IntPtr(24));
		GL.EnableVertexAttribArray(3);
		GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, 48, new IntPtr(32));
	}
}
}
namespace CubemapConvolution
{


[StructLayout(LayoutKind.Explicit,Size=12)]
public struct VertexAttribute
{
	
	[Index(0)]
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 Position;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 12, new IntPtr(0));
	}
}
}
namespace GBufferInstanced
{


[StructLayout(LayoutKind.Explicit,Size=48)]
public struct VertexAttribute
{
	
	[Index(0)]
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexPosition;
		
	[Index(1)]
	[FieldOffset(12), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexNormal;
		
	[Index(2)]
	[FieldOffset(24), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector2 TexCoord;
		
	[Index(3)]
	[FieldOffset(32), ComponentCount(4), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector4 Tangent;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 48, new IntPtr(0));
		GL.EnableVertexAttribArray(1);
		GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 48, new IntPtr(12));
		GL.EnableVertexAttribArray(2);
		GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 48, new IntPtr(24));
		GL.EnableVertexAttribArray(3);
		GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, 48, new IntPtr(32));
	}
}
}
namespace GBufferWithoutTexture
{


[StructLayout(LayoutKind.Explicit,Size=48)]
public struct VertexAttribute
{
	
	[Index(0)]
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexPosition;
		
	[Index(1)]
	[FieldOffset(12), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexNormal;
		
	[Index(2)]
	[FieldOffset(24), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector2 TexCoord;
		
	[Index(3)]
	[FieldOffset(32), ComponentCount(4), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector4 Tangent;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 48, new IntPtr(0));
		GL.EnableVertexAttribArray(1);
		GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 48, new IntPtr(12));
		GL.EnableVertexAttribArray(2);
		GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 48, new IntPtr(24));
		GL.EnableVertexAttribArray(3);
		GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, 48, new IntPtr(32));
	}
}
}
namespace GBufferPNC
{


[StructLayout(LayoutKind.Explicit,Size=36)]
public struct VertexAttribute
{
	
	[Index(0)]
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexPosition;
		
	[Index(1)]
	[FieldOffset(12), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexNormal;
		
	[Index(2)]
	[FieldOffset(24), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexColor;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 36, new IntPtr(0));
		GL.EnableVertexAttribArray(1);
		GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 36, new IntPtr(12));
		GL.EnableVertexAttribArray(2);
		GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 36, new IntPtr(24));
	}
}
}
namespace GBufferCubeTest
{


[StructLayout(LayoutKind.Explicit,Size=24)]
public struct VertexAttribute
{
	
	[Index(0)]
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexPosition;
		
	[Index(1)]
	[FieldOffset(12), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexNormal;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 24, new IntPtr(0));
		GL.EnableVertexAttribArray(1);
		GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 24, new IntPtr(12));
	}
}
}
namespace GBufferPNCT
{


[StructLayout(LayoutKind.Explicit,Size=44)]
public struct VertexAttribute
{
	
	[Index(0)]
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexPosition;
		
	[Index(1)]
	[FieldOffset(12), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexNormal;
		
	[Index(2)]
	[FieldOffset(24), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexColor;
		
	[Index(3)]
	[FieldOffset(36), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector2 TexCoord;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 44, new IntPtr(0));
		GL.EnableVertexAttribArray(1);
		GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 44, new IntPtr(12));
		GL.EnableVertexAttribArray(2);
		GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 44, new IntPtr(24));
		GL.EnableVertexAttribArray(3);
		GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, 44, new IntPtr(36));
	}
}
}
namespace GBufferPNT
{


[StructLayout(LayoutKind.Explicit,Size=32)]
public struct VertexAttribute
{
	
	[Index(0)]
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexPosition;
		
	[Index(1)]
	[FieldOffset(12), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexNormal;
		
	[Index(2)]
	[FieldOffset(24), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector2 TexCoord;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 32, new IntPtr(0));
		GL.EnableVertexAttribArray(1);
		GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 32, new IntPtr(12));
		GL.EnableVertexAttribArray(2);
		GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 32, new IntPtr(24));
	}
}
}
namespace GBufferPNTT
{


[StructLayout(LayoutKind.Explicit,Size=48)]
public struct VertexAttribute
{
	
	[Index(0)]
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexPosition;
		
	[Index(1)]
	[FieldOffset(12), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexNormal;
		
	[Index(2)]
	[FieldOffset(24), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector2 TexCoord;
		
	[Index(3)]
	[FieldOffset(32), ComponentCount(4), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector4 Tangent;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 48, new IntPtr(0));
		GL.EnableVertexAttribArray(1);
		GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 48, new IntPtr(12));
		GL.EnableVertexAttribArray(2);
		GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 48, new IntPtr(24));
		GL.EnableVertexAttribArray(3);
		GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, 48, new IntPtr(32));
	}
}
}
namespace Blur
{


[StructLayout(LayoutKind.Explicit,Size=20)]
public struct VertexAttribute
{
	
	[Index(0)]
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexPosition;
		
	[Index(1)]
	[FieldOffset(12), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector2 VertexTexCoord;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 20, new IntPtr(0));
		GL.EnableVertexAttribArray(1);
		GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 20, new IntPtr(12));
	}
}
}
namespace BloomMaterial
{


[StructLayout(LayoutKind.Explicit,Size=20)]
public struct VertexAttribute
{
	
	[Index(0)]
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexPosition;
		
	[Index(1)]
	[FieldOffset(12), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector2 VertexTexCoord;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 20, new IntPtr(0));
		GL.EnableVertexAttribArray(1);
		GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 20, new IntPtr(12));
	}
}
}
namespace CubemapMaterial
{


[StructLayout(LayoutKind.Explicit,Size=20)]
public struct VertexAttribute
{
	
	[Index(0)]
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexPosition;
		
	[Index(1)]
	[FieldOffset(12), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector2 TexCoord;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 20, new IntPtr(0));
		GL.EnableVertexAttribArray(1);
		GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 20, new IntPtr(12));
	}
}
}
namespace MSGBufferMaterial
{


[StructLayout(LayoutKind.Explicit,Size=48)]
public struct VertexAttribute
{
	
	[Index(0)]
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexPosition;
		
	[Index(1)]
	[FieldOffset(12), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexNormal;
		
	[Index(2)]
	[FieldOffset(24), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector2 TexCoord;
		
	[Index(3)]
	[FieldOffset(32), ComponentCount(4), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector4 Tangent;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 48, new IntPtr(0));
		GL.EnableVertexAttribArray(1);
		GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 48, new IntPtr(12));
		GL.EnableVertexAttribArray(2);
		GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 48, new IntPtr(24));
		GL.EnableVertexAttribArray(3);
		GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, 48, new IntPtr(32));
	}
}
}
namespace DepthVisualizeMaterial
{


[StructLayout(LayoutKind.Explicit,Size=20)]
public struct VertexAttribute
{
	
	[Index(0)]
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexPosition;
		
	[Index(1)]
	[FieldOffset(12), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector2 TexCoord;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 20, new IntPtr(0));
		GL.EnableVertexAttribArray(1);
		GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 20, new IntPtr(12));
	}
}
}
namespace FontRenderMaterial
{


[StructLayout(LayoutKind.Explicit,Size=32)]
public struct VertexAttribute
{
	
	[Index(0)]
	[FieldOffset(0), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector2 VertexPosition;
		
	[Index(1)]
	[FieldOffset(8), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector2 VertexTexCoord;
		
	[Index(2)]
	[FieldOffset(16), ComponentCount(4), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector4 VertexColor;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 32, new IntPtr(0));
		GL.EnableVertexAttribArray(1);
		GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 32, new IntPtr(8));
		GL.EnableVertexAttribArray(2);
		GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 32, new IntPtr(16));
	}
}
}
namespace FontBoxRenderMaterial
{


[StructLayout(LayoutKind.Explicit,Size=12)]
public struct VertexAttribute
{
	
	[Index(0)]
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexPosition;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 12, new IntPtr(0));
	}
}
}
namespace GridRenderMaterial
{


[StructLayout(LayoutKind.Explicit,Size=12)]
public struct VertexAttribute
{
	
	[Index(0)]
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexPosition;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 12, new IntPtr(0));
	}
}
}
namespace ThreeDTextRenderMaterial
{


[StructLayout(LayoutKind.Explicit,Size=20)]
public struct VertexAttribute
{
	
	[Index(0)]
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexPosition;
		
	[Index(1)]
	[FieldOffset(12), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector2 TexCoord;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 20, new IntPtr(0));
		GL.EnableVertexAttribArray(1);
		GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 20, new IntPtr(12));
	}
}
}
namespace ResolveMaterial
{


[StructLayout(LayoutKind.Explicit,Size=20)]
public struct VertexAttribute
{
	
	[Index(0)]
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexPosition;
		
	[Index(1)]
	[FieldOffset(12), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector2 VertexTexCoord;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 20, new IntPtr(0));
		GL.EnableVertexAttribArray(1);
		GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 20, new IntPtr(12));
	}
}
}
namespace SSAOMaterial
{


[StructLayout(LayoutKind.Explicit,Size=20)]
public struct VertexAttribute
{
	
	[Index(0)]
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexPosition;
		
	[Index(1)]
	[FieldOffset(12), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector2 VertexTexCoord;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 20, new IntPtr(0));
		GL.EnableVertexAttribArray(1);
		GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 20, new IntPtr(12));
	}
}
}
namespace LUTGenerateMaterial
{


[StructLayout(LayoutKind.Explicit,Size=20)]
public struct VertexAttribute
{
	
	[Index(0)]
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 InPosition;
		
	[Index(1)]
	[FieldOffset(12), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector2 InTexCoords;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 20, new IntPtr(0));
		GL.EnableVertexAttribArray(1);
		GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 20, new IntPtr(12));
	}
}
}
namespace PrefilterMaterial
{


[StructLayout(LayoutKind.Explicit,Size=12)]
public struct VertexAttribute
{
	
	[Index(0)]
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 Position;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 12, new IntPtr(0));
	}
}
}
namespace FXAAMaterial
{


[StructLayout(LayoutKind.Explicit,Size=20)]
public struct VertexAttribute
{
	
	[Index(0)]
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexPosition;
		
	[Index(1)]
	[FieldOffset(12), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector2 VertexTexCoord;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 20, new IntPtr(0));
		GL.EnableVertexAttribArray(1);
		GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 20, new IntPtr(12));
	}
}
}
namespace TBNMaterial
{


[StructLayout(LayoutKind.Explicit,Size=24)]
public struct VertexAttribute
{
	
	[Index(0)]
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexPosition;
		
	[Index(1)]
	[FieldOffset(12), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexColor;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 24, new IntPtr(0));
		GL.EnableVertexAttribArray(1);
		GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 24, new IntPtr(12));
	}
}
}
namespace SignedDistanceField
{


[StructLayout(LayoutKind.Explicit,Size=20)]
public struct VertexAttribute
{
	
	[Index(0)]
	[FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector3 VertexPosition;
		
	[Index(1)]
	[FieldOffset(12), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
	public OpenTK.Mathematics.Vector2 VertexTexCoord;
	
	public static void VertexAttributeBinding()
	{
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 20, new IntPtr(0));
		GL.EnableVertexAttribArray(1);
		GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 20, new IntPtr(12));
	}
}
}

}
