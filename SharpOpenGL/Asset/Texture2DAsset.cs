using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Core.Texture;
using ZeroFormatter;

namespace SharpOpenGL.Asset
{
    [ZeroFormattable]
    public class Texture2DAsset : Core.AssetBase
    {
        [Index(0)]
        public virtual int Width { get; protected set; } = 0;

        [Index(1)]
        public virtual int Height { get; protected set; } = 0;

        [Index(2)]
        public virtual byte[] Bytes { get; protected set; } = null;

        [Index(3)]
        public virtual int ByteLength { get; protected set; } = 0;

        [Index(4)]
        public virtual string OriginalFilePath { get; protected set; } = "";

        public override void ImportAssetSync()
        {
            using (var scopedImage = new ScopedFreeImage(OriginalFilePath))
            {
                Width = scopedImage.Width;
                Height = scopedImage.Height;
                this.Bytes = new byte[scopedImage.ByteSize];
                this.ByteLength = (int)scopedImage.ByteSize;
                Marshal.Copy(scopedImage.Bytes, this.Bytes, 0, (int) scopedImage.ByteSize);
            }
        }
    }
}
