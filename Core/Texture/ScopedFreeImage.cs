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
        public ScopedFreeImage(string imagePath, bool bFloatingPoint = false)
        {
            if (File.Exists(imagePath) == false)
            {
                bitmap = FreeImageHelper.Load("./Resources/Texture/Checker.png", out Width, out Height, out ImagePixelFormat, out IsTransparent, out BPP);
            }
            else
            {
                bitmap = FreeImageHelper.Load(imagePath, out Width, out Height, out ImagePixelFormat, out IsTransparent, out BPP);
            }
            
            if (ImagePixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
            { 
                bitmap = FreeImage.ConvertTo24Bits(bitmap);
                ImagePixelInternalFormat = PixelInternalFormat.Rgb;
                OpenglPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Bgr;
                OpenglPixelType = OpenTK.Graphics.OpenGL.PixelType.UnsignedByte;
            }
            else if (ImagePixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb)
            {
                bitmap = FreeImage.ConvertTo32Bits(bitmap);
                ImagePixelInternalFormat = PixelInternalFormat.Rgba;
                OpenglPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Bgra;
                OpenglPixelType = OpenTK.Graphics.OpenGL.PixelType.UnsignedByte;
            }
            else if (ImagePixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppRgb)
            {
                bitmap = FreeImage.ConvertTo32Bits(bitmap);
                ImagePixelInternalFormat = PixelInternalFormat.Rgb;
                OpenglPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Bgr;
                OpenglPixelType = OpenTK.Graphics.OpenGL.PixelType.UnsignedByte;
            }
            else if (ImagePixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
            {
                bitmap = FreeImage.ConvertTo8Bits(bitmap);
                ImagePixelInternalFormat = PixelInternalFormat.Luminance;
                OpenglPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Luminance;
                OpenglPixelType = OpenTK.Graphics.OpenGL.PixelType.UnsignedByte;
            }
            else if (ImagePixelFormat == System.Drawing.Imaging.PixelFormat.DontCare)
            {
                ImagePixelInternalFormat = PixelInternalFormat.Rgb16;
                OpenglPixelType = PixelType.UnsignedByte;
                OpenglPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Rgb;
            }
            else
            {
                Debug.Assert(false, "Unkown Pixel Format");
            }
            
            header = FreeImage.GetInfoHeaderEx(bitmap);

            if(bFloatingPoint)
            {
                FloatData = FreeImage.GetBits(bitmap);
            }
            else
            {
                Bytes = FreeImage.GetBits(bitmap);
            }
            
            ByteSize = FreeImage.GetDIBSize(bitmap) - header.biSize;
        }
        public void Dispose()
        {
            FreeImage.Unload(bitmap);
        }
        
        public IntPtr Bytes;
        public IntPtr FloatData;
        public uint ByteSize = 0;
        public FIBITMAP bitmap;
        private BITMAPINFOHEADER header;
        public int Width = 0;
        public int Height = 0;
        public bool IsTransparent;
        public uint BPP = 0;
        private System.Drawing.Imaging.PixelFormat ImagePixelFormat = System.Drawing.Imaging.PixelFormat.Max;
        public PixelInternalFormat ImagePixelInternalFormat = PixelInternalFormat.Rgb;
        public OpenTK.Graphics.OpenGL.PixelFormat OpenglPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Rgb;
        public OpenTK.Graphics.OpenGL.PixelType OpenglPixelType = OpenTK.Graphics.OpenGL.PixelType.UnsignedByte;
    }
}
