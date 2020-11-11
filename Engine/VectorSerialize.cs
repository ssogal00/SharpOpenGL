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
using OpenTK.Mathematics;

namespace Core.CustomSerialize
{
    
    public class Vector3Formatter<TTypeResolver> : Formatter<TTypeResolver, Vector3>
    where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {   
            return Vector3.SizeInBytes;
        }

        public override int Serialize(ref byte[] bytes, int offset, Vector3 value)
        {
            BinaryUtil.WriteSingle(ref bytes, offset, value.X);
            BinaryUtil.WriteSingle(ref bytes, offset + Marshal.SizeOf(typeof(float)), value.Y);
            BinaryUtil.WriteSingle(ref bytes, offset + Marshal.SizeOf(typeof(float)) * 2, value.Z);
            return Vector3.SizeInBytes;
        }

        public override Vector3 Deserialize(ref byte[] bytes, int offset, DirtyTracker tracker, out int byteSize)
        {
            var result = new Vector3();
            result.X = (float)BinaryUtil.ReadSingle(ref bytes, offset);
            result.Y = (float)BinaryUtil.ReadSingle(ref bytes, offset + Marshal.SizeOf(typeof(float)));
            result.Z = (float)BinaryUtil.ReadSingle(ref bytes, offset + Marshal.SizeOf(typeof(float)) * 2);
            byteSize = Vector3.SizeInBytes;
            return result;
        }
    }

    public class Vector4Formatter<TTypeResolver> : Formatter<TTypeResolver, Vector4>
   where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return Vector4.SizeInBytes;
        }

        public override int Serialize(ref byte[] bytes, int offset, Vector4 value)
        {
            BinaryUtil.WriteSingle(ref bytes, offset, value.X);
            BinaryUtil.WriteSingle(ref bytes, offset + Marshal.SizeOf(typeof(float)), value.Y);
            BinaryUtil.WriteSingle(ref bytes, offset + Marshal.SizeOf(typeof(float)) * 2, value.Z);
            BinaryUtil.WriteSingle(ref bytes, offset + Marshal.SizeOf(typeof(float)) * 3, value.W);
            return Vector4.SizeInBytes;
        }

        public override Vector4 Deserialize(ref byte[] bytes, int offset, DirtyTracker tracker, out int byteSize)
        {
            var result = new Vector4();
            result.X = (float)BinaryUtil.ReadSingle(ref bytes, offset);
            result.Y = (float)BinaryUtil.ReadSingle(ref bytes, offset + Marshal.SizeOf(typeof(float)));
            result.Z = (float)BinaryUtil.ReadSingle(ref bytes, offset + Marshal.SizeOf(typeof(float)) * 2);
            result.W = (float)BinaryUtil.ReadSingle(ref bytes, offset + Marshal.SizeOf(typeof(float)) * 3);
            byteSize = Vector4.SizeInBytes;
            return result;
        }
    }

    public class Vector2Formatter<TTypeResolver> : Formatter<TTypeResolver, Vector2>
    where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return Vector2.SizeInBytes;
        }

        public override int Serialize(ref byte[] bytes, int offset, Vector2 value)
        {
            BinaryUtil.WriteSingle(ref bytes, offset, value.X);
            BinaryUtil.WriteSingle(ref bytes, offset + Marshal.SizeOf(typeof(float)), value.Y);
            return Vector2.SizeInBytes;
        }

        public override Vector2 Deserialize(ref byte[] bytes, int offset, DirtyTracker tracker, out int byteSize)
        {
            var result = new Vector2();
            result.X = (float)BinaryUtil.ReadSingle(ref bytes, offset);
            result.Y = (float)BinaryUtil.ReadSingle(ref bytes, offset + Marshal.SizeOf(typeof(float)));
            byteSize = Vector2.SizeInBytes;
            return result;
        }
    }
}
