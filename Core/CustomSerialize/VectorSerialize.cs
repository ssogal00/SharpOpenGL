using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZeroFormatter;
using ZeroFormatter.Internal;
using OpenTK.Graphics;
using ZeroFormatter.Formatters;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Core.CustomSerialize
{
    public class Vector3Formatter<TTypeResolver> : Formatter<TTypeResolver, OpenTK.Vector3>
    where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {   
            return OpenTK.Vector3.SizeInBytes;
        }

        public override int Serialize(ref byte[] bytes, int offset, OpenTK.Vector3 value)
        {
            BinaryUtil.WriteSingle(ref bytes, offset, value.X);
            BinaryUtil.WriteSingle(ref bytes, offset + Marshal.SizeOf(typeof(float)), value.Y);
            BinaryUtil.WriteSingle(ref bytes, offset + Marshal.SizeOf(typeof(float)) * 2, value.Z);
            return OpenTK.Vector3.SizeInBytes;
        }

        public override OpenTK.Vector3 Deserialize(ref byte[] bytes, int offset, DirtyTracker tracker, out int byteSize)
        {
            var result = new OpenTK.Vector3();
            result.X = (float)BinaryUtil.ReadSingle(ref bytes, offset);
            result.Y = (float)BinaryUtil.ReadSingle(ref bytes, offset + Marshal.SizeOf(typeof(float)));
            result.Z = (float)BinaryUtil.ReadSingle(ref bytes, offset + Marshal.SizeOf(typeof(float)) * 2);
            byteSize = OpenTK.Vector3.SizeInBytes;
            return result;
        }
    }

    public class Vector4Formatter<TTypeResolver> : Formatter<TTypeResolver, OpenTK.Vector4>
   where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return OpenTK.Vector4.SizeInBytes;
        }

        public override int Serialize(ref byte[] bytes, int offset, OpenTK.Vector4 value)
        {
            BinaryUtil.WriteSingle(ref bytes, offset, value.X);
            BinaryUtil.WriteSingle(ref bytes, offset + Marshal.SizeOf(typeof(float)), value.Y);
            BinaryUtil.WriteSingle(ref bytes, offset + Marshal.SizeOf(typeof(float)) * 2, value.Z);
            BinaryUtil.WriteSingle(ref bytes, offset + Marshal.SizeOf(typeof(float)) * 3, value.W);
            return OpenTK.Vector4.SizeInBytes;
        }

        public override OpenTK.Vector4 Deserialize(ref byte[] bytes, int offset, DirtyTracker tracker, out int byteSize)
        {
            var result = new OpenTK.Vector4();
            result.X = (float)BinaryUtil.ReadSingle(ref bytes, offset);
            result.Y = (float)BinaryUtil.ReadSingle(ref bytes, offset + Marshal.SizeOf(typeof(float)));
            result.Z = (float)BinaryUtil.ReadSingle(ref bytes, offset + Marshal.SizeOf(typeof(float)) * 2);
            result.W = (float)BinaryUtil.ReadSingle(ref bytes, offset + Marshal.SizeOf(typeof(float)) * 3);
            byteSize = OpenTK.Vector4.SizeInBytes;
            return result;
        }
    }

    public class Vector2Formatter<TTypeResolver> : Formatter<TTypeResolver, OpenTK.Vector2>
    where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return OpenTK.Vector2.SizeInBytes;
        }

        public override int Serialize(ref byte[] bytes, int offset, OpenTK.Vector2 value)
        {
            BinaryUtil.WriteSingle(ref bytes, offset, value.X);
            BinaryUtil.WriteSingle(ref bytes, offset + Marshal.SizeOf(typeof(float)), value.Y);
            return OpenTK.Vector2.SizeInBytes;
        }

        public override OpenTK.Vector2 Deserialize(ref byte[] bytes, int offset, DirtyTracker tracker, out int byteSize)
        {
            var result = new OpenTK.Vector2();
            result.X = (float)BinaryUtil.ReadSingle(ref bytes, offset);
            result.Y = (float)BinaryUtil.ReadSingle(ref bytes, offset + Marshal.SizeOf(typeof(float)));
            byteSize = OpenTK.Vector2.SizeInBytes;
            return result;
        }
    }
}
