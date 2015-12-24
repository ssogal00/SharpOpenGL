using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using System.Runtime.InteropServices;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SharpOpenGL.StaticMesh
{
    [StructLayout(LayoutKind.Explicit, Size = 12)]
    public struct ObjMeshVertexAttribute
    {
        [FieldOffset(0), ComponentCount(3)]
        public OpenTK.Vector3 VertexPosition;
    }
}
