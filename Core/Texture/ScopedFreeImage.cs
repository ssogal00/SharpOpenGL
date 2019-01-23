using System;
using FreeImageAPI;
using System.IO;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Core.Texture
{
    public class ScopedFreeImage : IDisposable
    {
        public ScopedFreeImage(string imagePath)
        {
            if (File.Exists(imagePath) == false)
            {
                bitmap = FreeImageHelper.Load("./Resources/Texture/Checker.png", out Width, out Height, out ImagePixelFormat);
            }
            else
            {
                bitmap = FreeImageHelper.Load(imagePath, out Width, out Height, out ImagePixelFormat);
            }

            if (ImagePixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
            {
                bitmap = FreeImage.ConvertTo24Bits(bitmap);
                ImagePixelInternalFormat = PixelInternalFormat.Rgb;
                OpenglPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Bgr;
            }
            else if (ImagePixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb)
            {
                bitmap = FreeImage.ConvertTo32Bits(bitmap);
                ImagePixelInternalFormat = PixelInternalFormat.Rgba;
                OpenglPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Bgra;
            }
            else if (ImagePixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppRgb)
            {
                bitmap = FreeImage.ConvertTo32Bits(bitmap);
                ImagePixelInternalFormat = PixelInternalFormat.Rgb;
                OpenglPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Bgr;
            }
            else if (ImagePixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
            {
                bitmap = FreeImage.ConvertTo8Bits(bitmap);
                ImagePixelInternalFormat = PixelInternalFormat.Luminance;
                OpenglPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Luminance;
            }
            else
            {
                Debug.Assert(false, "Unkown Pixel Format");
            }
            
            header = FreeImage.GetInfoHeaderEx(bitmap);
            Bytes = FreeImage.GetBits(bitmap);
            ByteSize = FreeImage.GetDIBSize(bitmap) - header.biSize;
        }
        public void Dispose()
        {
            FreeImage.Unload(bitmap);
        }
        
        public IntPtr Bytes;
        public uint ByteSize = 0;
        public FIBITMAP bitmap;
        private BITMAPINFOHEADER header;
        public int Width = 0;
        public int Height = 0;
        private System.Drawing.Imaging.PixelFormat ImagePixelFormat = System.Drawing.Imaging.PixelFormat.Max;
        public PixelInternalFormat ImagePixelInternalFormat = PixelInternalFormat.Rgb;
        public OpenTK.Graphics.OpenGL.PixelFormat OpenglPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Rgb;
    }
}
