using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Core.Texture;
using ZeroFormatter;
using OpenTK.Graphics.OpenGL;

namespace SharpOpenGL.Asset
{
    [ZeroFormattable]
    public class Texture2DAsset : Core.AssetBase
    {
        [Index(0)] public virtual int Width { get; protected set; } = 0;

        [Index(1)] public virtual int Height { get; protected set; } = 0;

        [Index(2)] public virtual byte[] Bytes { get; protected set; } = null;

        [Index(3)] public virtual int ByteLength { get; protected set; } = 0;

        [Index(4)] public virtual string OriginalFilePath { get; protected set; } = "";

        [Index(5)]
        public virtual PixelInternalFormat ImagePixelInternalFormat { get; protected set; } = PixelInternalFormat.Rgb;

        [Index(6)]
        public virtual OpenTK.Graphics.OpenGL.PixelFormat OpenglPixelFormat { get; protected set; } = OpenTK.Graphics.OpenGL.PixelFormat.Rgb;

        public override void ImportAssetSync()
        {
            using (var scopedImage = new ScopedFreeImage(OriginalFilePath))
            {
                this.Width = scopedImage.Width;
                this.Height = scopedImage.Height;
                this.Bytes = new byte[scopedImage.ByteSize];
                this.ByteLength = (int) scopedImage.ByteSize;
                Marshal.Copy(scopedImage.Bytes, this.Bytes, 0, (int) scopedImage.ByteSize);
            }
        }

        public override void SaveImportedAsset(string path)
        {
            var bytesarray = ZeroFormatter.ZeroFormatterSerializer.Serialize<Texture2DAsset>(this);
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                fs.Write(bytesarray, 0, bytesarray.Count());
            }
        }

        public Texture2DAsset(string path)
        {
            this.OriginalFilePath = path;
        }

        public override void InitializeInRenderThread()
        {
            base.InitializeInRenderThread();
        }
    }
}
