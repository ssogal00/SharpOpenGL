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

    [StructLayout(LayoutKind.Explicit, Size = 12)]
    public struct PrimitiveVertexAttribute
    {
        [FieldOffset(0), ComponentCount(3), ComponentType(VertexAttribPointerType.Float)]
        public OpenTK.Vector3 VertexPosition;

        public static void VertexAttributeBinding()
        {
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 12, new IntPtr(0));
        }
    }

    public class Line
    {
        public Line(OpenTK.Vector3 vStart, OpenTK.Vector3 vEnd)
        {
            StartPoint = vStart;
            EndPoint = vEnd;
        }

        

        public OpenTK.Vector3 StartPoint;
        public OpenTK.Vector3 EndPoint;
    }
}
