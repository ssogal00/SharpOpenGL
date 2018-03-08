using Core.VertexCustomAttribute;
using OpenTK.Graphics.OpenGL;
using System;
using System.Runtime.InteropServices;
using ZeroFormatter;
using ZeroFormatter.Internal;
using OpenTK;

namespace Core.Primitive
{
    // Position Only 
    [ZeroFormattable]
    [StructLayout(LayoutKind.Explicit, Size = 12)]
    public struct P_VertexAttribute
    {
        [Index(0)]
        [FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public OpenTK.Vector3 VertexPosition;

        public P_VertexAttribute(OpenTK.Vector3 position)
        {
            VertexPosition = position;
        }

        public static void VertexAttributeBinding()
        {
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 12, new IntPtr(0));
        }
    }

    // Position 
    // Color
    [ZeroFormattable]
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public struct PC_VertexAttribute
    {
        [Index(0)]
        [FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public OpenTK.Vector3 VertexPosition;

        [Index(1)]
        [FieldOffset(12), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public OpenTK.Vector3 VertexColor;

        public PC_VertexAttribute(OpenTK.Vector3 position, OpenTK.Vector3 color)
        {
            VertexPosition = position;
            VertexColor = color;
        }

        public static void VertexAttributeBinding()
        {
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 24, new IntPtr(0));
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 24, new IntPtr(12));
        }
    }

    // Position 
    // Texture Coordinate
    [ZeroFormattable]
    [StructLayout(LayoutKind.Explicit, Size = 20)]
    public struct PT_VertexAttribute
    {
        [Index(0)]
        [FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public OpenTK.Vector3 VertexPosition;

        [Index(1)]
        [FieldOffset(12), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
        public OpenTK.Vector2 TexCoord;

        public PT_VertexAttribute(OpenTK.Vector3 position, OpenTK.Vector2 texcoord)
        {
            VertexPosition = position;
            TexCoord = texcoord;
        }

        public static void VertexAttributeBinding()
        {
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 20, new IntPtr(0));
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 20, new IntPtr(12));
        }
    }

    // Position
    // Normal
    [ZeroFormattable]
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public struct PN_VertexAttribute
    {
        [Index(0)]
        [FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public OpenTK.Vector3 VertexPosition;

        [Index(1)]
        [FieldOffset(12), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public OpenTK.Vector3 VertexNormal;

        public PN_VertexAttribute(OpenTK.Vector3 position, OpenTK.Vector3 normal)
        {
            VertexPosition = position;
            VertexNormal = normal;
        }

        public static void VertexAttributeBinding()
        {
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 24, new IntPtr(0));
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 24, new IntPtr(12));
        }
    }


    // Position
    // Normal
    // Texture Coordinate
    [ZeroFormattable]
    [StructLayout(LayoutKind.Explicit, Size = 32)]
    public struct PNT_VertexAttribute
    {
        [Index(0)]
        [FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public OpenTK.Vector3 VertexPosition;

        [Index(1)]
        [FieldOffset(12), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public OpenTK.Vector3 VertexNormal;

        [Index(2)]
        [FieldOffset(24), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
        public OpenTK.Vector2 TexCoord;

        public PNT_VertexAttribute(OpenTK.Vector3 position, OpenTK.Vector3 normal, OpenTK.Vector2 texcoord)
        {
            VertexPosition = position;
            VertexNormal = normal;
            TexCoord = texcoord;
        }

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

    // position
    // normal
    // texcoord
    // tangent
    [ZeroFormattable]
    [StructLayout(LayoutKind.Explicit, Size = 48)]
    public struct PNTT_VertexAttribute
    {
        [Index(0)]
        [FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public OpenTK.Vector3 VertexPosition;

        [Index(1)]
        [FieldOffset(12), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public OpenTK.Vector3 VertexNormal;

        [Index(2)]
        [FieldOffset(24), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
        public OpenTK.Vector2 TexCoord;

        [Index(3)]
        [FieldOffset(32), ComponentCount(4), ComponentType(VertexAttribPointerType.Float)]
        public OpenTK.Vector4 Tangent;

        public PNTT_VertexAttribute(Vector3 position, Vector3 normal, Vector2 texcoord, Vector4 tangent)
        {
            VertexPosition = position;
            VertexNormal = normal;
            TexCoord = texcoord;
            Tangent = tangent;
        }

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
