using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroFormatter;
using ZeroFormatter.Formatters;
using ZeroFormatter.Internal;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Core.CustomSerialize
{
    public class MatrixSerialize
    {
        public class Matrix2Formatter<TTypeResolver> : Formatter<TTypeResolver, OpenTK.Matrix2>
        where TTypeResolver : ITypeResolver, new()
        {
            public override int? GetLength()
            {
                return OpenTK.Vector2.SizeInBytes * 2;
            }

            public override int Serialize(ref byte[] bytes, int offset, OpenTK.Matrix2 value)
            {
                BinaryUtil.WriteSingle(ref bytes, offset, value.M11);
                offset += Marshal.SizeOf(typeof(float));

                BinaryUtil.WriteSingle(ref bytes, offset, value.M12);
                offset += Marshal.SizeOf(typeof(float));

                BinaryUtil.WriteSingle(ref bytes, offset, value.M21);
                offset += Marshal.SizeOf(typeof(float));

                BinaryUtil.WriteSingle(ref bytes, offset, value.M22);
                offset += Marshal.SizeOf(typeof(float));
                
                return OpenTK.Vector2.SizeInBytes * 2;
            }

            public override OpenTK.Matrix2 Deserialize(ref byte[] bytes, int offset, DirtyTracker tracker, out int byteSize)
            {
                var result = new OpenTK.Matrix2();
                result.M11 = BinaryUtil.ReadSingle(ref bytes, offset);
                offset += Marshal.SizeOf(typeof(float));

                result.M12 = BinaryUtil.ReadSingle(ref bytes, offset);
                offset += Marshal.SizeOf(typeof(float));

                result.M21 = BinaryUtil.ReadSingle(ref bytes, offset);
                offset += Marshal.SizeOf(typeof(float));

                result.M22 = BinaryUtil.ReadSingle(ref bytes, offset);
                offset += Marshal.SizeOf(typeof(float));
                
                byteSize = OpenTK.Vector2.SizeInBytes * 2;
                return result;
            }
        }
    }
}
