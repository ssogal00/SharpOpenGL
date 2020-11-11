using Core.VertexCustomAttribute;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Core.OpenGLShader;
using ZeroFormatter;
using OpenTK.Mathematics;


namespace Core.Primitive
{
    public interface IGenericVertexAttribute
    {
        List<VertexAttribute> GetVertexAttributes();
        void VertexAttributeBinding();
    }

    // Position Only 
    [ZeroFormattable]
    [StructLayout(LayoutKind.Explicit, Size = 12)]
    public struct P_VertexAttribute : IGenericVertexAttribute
    {
        [Index(0)]
        [FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public Vector3 VertexPosition;

        public P_VertexAttribute(Vector3 position)
        {
            VertexPosition = position;
        }

        public void VertexAttributeBinding()
        {
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 12, new IntPtr(0));
        }

        public List<VertexAttribute> GetVertexAttributes()
        {
            return new List<VertexAttribute>
            {
                new VertexAttribute(0, ActiveAttribType.FloatVec3, "P")
            };
        }


    }

    // Position 
    // Color
    [ZeroFormattable]
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public struct PC_VertexAttribute : IGenericVertexAttribute
    {
        [Index(0)]
        [FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public Vector3 VertexPosition;

        [Index(1)]
        [FieldOffset(12), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public Vector3 VertexColor;

        public PC_VertexAttribute(Vector3 position, Vector3 color)
        {
            VertexPosition = position;
            VertexColor = color;
        }

        public void VertexAttributeBinding()
        {
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 24, new IntPtr(0));
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 24, new IntPtr(12));
        }

        public List<VertexAttribute> GetVertexAttributes()
        {
            return new List<VertexAttribute>
            {
                new VertexAttribute(0, ActiveAttribType.FloatVec3, "P"),
                new VertexAttribute(1, ActiveAttribType.FloatVec3, "C"),
            };
        }
    }

    // Position 
    // Texture Coordinate
    [ZeroFormattable]
    [StructLayout(LayoutKind.Explicit, Size = 20)]
    public struct PT_VertexAttribute : IGenericVertexAttribute
    {
        [Index(0)]
        [FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public Vector3 VertexPosition;

        [Index(1)]
        [FieldOffset(12), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
        public Vector2 TexCoord;

        public PT_VertexAttribute(Vector3 position, Vector2 texcoord)
        {
            VertexPosition = position;
            TexCoord = texcoord;
        }

        public void VertexAttributeBinding()
        {
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 20, new IntPtr(0));
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 20, new IntPtr(12));
        }

        public List<VertexAttribute> GetVertexAttributes()
        {
            return new List<VertexAttribute>
            {
                new VertexAttribute(0, ActiveAttribType.FloatVec3, "P"),
                new VertexAttribute(1, ActiveAttribType.FloatVec2, "T"),
            };
        }
    }

    // Position
    // Normal
    [ZeroFormattable]
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public struct PN_VertexAttribute : IGenericVertexAttribute
    {
        [Index(0)]
        [FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public Vector3 VertexPosition;

        [Index(1)]
        [FieldOffset(12), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public Vector3 VertexNormal;

        public PN_VertexAttribute(Vector3 position, Vector3 normal)
        {
            VertexPosition = position;
            VertexNormal = normal;
        }

        public void VertexAttributeBinding()
        {
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 24, new IntPtr(0));
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 24, new IntPtr(12));
        }

        public List<VertexAttribute> GetVertexAttributes()
        {
            return new List<VertexAttribute>
            {
                new VertexAttribute(0, ActiveAttribType.FloatVec3, "P"),
                new VertexAttribute(1, ActiveAttribType.FloatVec3, "N"),
            };
        }
    }


    // Position
    // Normal
    // Texture Coordinate
    [ZeroFormattable]
    [StructLayout(LayoutKind.Explicit, Size = 32)]
    public struct PNT_VertexAttribute : IGenericVertexAttribute
    {
        [Index(0)]
        [FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public Vector3 VertexPosition;

        [Index(1)]
        [FieldOffset(12), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public Vector3 VertexNormal;

        [Index(2)]
        [FieldOffset(24), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
        public Vector2 TexCoord;

        public PNT_VertexAttribute(Vector3 position, Vector3 normal, Vector2 texcoord)
        {
            VertexPosition = position;
            VertexNormal = normal;
            TexCoord = texcoord;
        }

        public void VertexAttributeBinding()
        {
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 32, new IntPtr(0));
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 32, new IntPtr(12));
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 32, new IntPtr(24));
        }

        public List<VertexAttribute> GetVertexAttributes()
        {
            return new List<VertexAttribute>
            {
                new VertexAttribute(0, ActiveAttribType.FloatVec3, "P"),
                new VertexAttribute(1, ActiveAttribType.FloatVec3, "N"),
                new VertexAttribute(2, ActiveAttribType.FloatVec2, "T"),
            };
        }
    }

    // Position
    // Normal
    // Color
    [ZeroFormattable]
    [StructLayout(LayoutKind.Explicit, Size = 36)]
    public struct PNC_VertexAttribute : IGenericVertexAttribute
    {
        [Index(0)]
        [FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public Vector3 VertexPosition;

        [Index(1)]
        [FieldOffset(12), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public Vector3 VertexNormal;

        [Index(2)]
        [FieldOffset(24), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public Vector3 VertexColor;

        public PNC_VertexAttribute(Vector3 position, Vector3 normal, Vector3 color)
        {
            VertexPosition = position;
            VertexNormal = normal;
            VertexColor = color;
        }

        public void VertexAttributeBinding()
        {
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 36, new IntPtr(0));
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 36, new IntPtr(12));
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 36, new IntPtr(24));
        }

        public List<VertexAttribute> GetVertexAttributes()
        {
            return new List<VertexAttribute>
            {
                new VertexAttribute(0, ActiveAttribType.FloatVec3, "P"),
                new VertexAttribute(1, ActiveAttribType.FloatVec3, "N"),
                new VertexAttribute(2, ActiveAttribType.FloatVec3, "C"),
            };
        }
    }

    // Position
    // Normal
    // Color
    [ZeroFormattable]
    [StructLayout(LayoutKind.Explicit, Size = 36)]
    public struct PNCT_VertexAttribute : IGenericVertexAttribute
    {
        [Index(0)]
        [FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public Vector3 VertexPosition;

        [Index(1)]
        [FieldOffset(12), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public Vector3 VertexNormal;

        [Index(2)]
        [FieldOffset(24), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public Vector3 VertexColor;

        [Index(3)]
        [FieldOffset(36), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
        public Vector2 TexCoord;

        public PNCT_VertexAttribute(Vector3 position, Vector3 normal, Vector3 color, Vector2 texcoord)
        {
            VertexPosition = position;
            VertexNormal = normal;
            VertexColor = color;
            TexCoord = texcoord;
        }

        public void VertexAttributeBinding()
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

        public List<VertexAttribute> GetVertexAttributes()
        {
            return new List<VertexAttribute>
            {
                new VertexAttribute(0, ActiveAttribType.FloatVec3, "P"),
                new VertexAttribute(1, ActiveAttribType.FloatVec3, "N"),
                new VertexAttribute(2, ActiveAttribType.FloatVec3, "C"),
                new VertexAttribute(3, ActiveAttribType.FloatVec2, "T"),
            };
        }
    }

    // position
    // normal
    // texcoord
    // tangent
    [ZeroFormattable]
    [StructLayout(LayoutKind.Explicit, Size = 48)]
    public struct PNTT_VertexAttribute : IGenericVertexAttribute
    {
        [Index(0)]
        [FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public Vector3 VertexPosition;

        [Index(1)]
        [FieldOffset(12), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public Vector3 VertexNormal;

        [Index(2)]
        [FieldOffset(24), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
        public Vector2 TexCoord;

        [Index(3)]
        [FieldOffset(32), ComponentCount(4), ComponentType(VertexAttribPointerType.Float)]
        public Vector4 Tangent;

        public PNTT_VertexAttribute(Vector3 position, Vector3 normal, Vector2 texcoord, Vector4 tangent)
        {
            VertexPosition = position;
            VertexNormal = normal;
            TexCoord = texcoord;
            Tangent = tangent;
        }

        public void VertexAttributeBinding()
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

        public List<VertexAttribute> GetVertexAttributes()
        {
            return new List<VertexAttribute>
            {
                new VertexAttribute(0, ActiveAttribType.FloatVec3, "P"),
                new VertexAttribute(1, ActiveAttribType.FloatVec3, "N"),
                new VertexAttribute(2, ActiveAttribType.FloatVec2, "T"),
                new VertexAttribute(3, ActiveAttribType.FloatVec4, "T"),
            };
        }

        public override int GetHashCode()
        {
            int x = (int) (this.VertexPosition.X * 100);
            int y = (int)(this.VertexPosition.Y * 100);
            int z = (int)(this.VertexPosition.Z * 100);

            return (3 * x + 5 * y + 7 * z) % (1 << 16);
        }
    }
}
