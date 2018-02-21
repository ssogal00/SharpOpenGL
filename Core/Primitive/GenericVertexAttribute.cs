using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;

using OpenTK.Graphics.OpenGL;
using Core;
using Core.Buffer;
using System.Runtime.InteropServices;
using Core.VertexCustomAttribute;

namespace Core.Primitive
{
    // Position Only 
    [StructLayout(LayoutKind.Explicit, Size = 12)]
    public struct P_VertexAttribute
    {
        [FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public OpenTK.Vector3 VertexPosition;

        public static void VertexAttributeBinding()
        {
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 12, new IntPtr(0));
        }
    }

    // Position 
    // Color
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public struct PC_VertexAttribute
    {
        [FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public OpenTK.Vector3 VertexPosition;

        [FieldOffset(12), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public OpenTK.Vector3 VertexColor;

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
    [StructLayout(LayoutKind.Explicit, Size = 20)]
    public struct PT_VertexAttribute
    {
        [FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public OpenTK.Vector3 VertexPosition;

        [FieldOffset(12), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
        public OpenTK.Vector2 TexCoord;

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
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public struct PN_VertexAttribute
    {
        [FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public OpenTK.Vector3 VertexPosition;

        [FieldOffset(12), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public OpenTK.Vector3 VertexNormal;

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
    [StructLayout(LayoutKind.Explicit, Size = 32)]
    public struct PNT_VertexAttribute
    {
        [FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public OpenTK.Vector3 VertexPosition;

        [FieldOffset(12), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public OpenTK.Vector3 VertexNormal;

        [FieldOffset(24), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
        public OpenTK.Vector2 TexCoord;

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
