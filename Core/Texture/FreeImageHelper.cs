using System;
using System.IO;
using FreeImageAPI;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;


namespace Core.Texture
{
    public static class FreeImageHelper
    {
        public static void SaveAsBmp(ref byte[] Data, int width, int height, string FileName)
        {
            Debug.Assert(width > 0 && height > 0);

            using (var bitmap = new Bitmap(width, height))
            {
                var bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                Marshal.Copy(Data, 0, bitmapData.Scan0, Data.Length);
                bitmap.UnlockBits(bitmapData);
                bool bSaved = FreeImage.SaveBitmap(bitmap, FileName);
            }
        }

        public static FIBITMAP Load(string FilePath, out int Width, out int Height)
        {
            FIBITMAP DIB = new FIBITMAP();

            Width = Height = 0;

            if (!File.Exists(FilePath))
            {
                Debug.Assert(false, string.Format("{0} not exist", FilePath));
                return DIB;
            }

            FREE_IMAGE_FORMAT FileType = FreeImageAPI.FreeImage.GetFileType(FilePath, 0);

            if (FileType == FREE_IMAGE_FORMAT.FIF_UNKNOWN)
            {
                FileType = FreeImage.GetFIFFromFilename(FilePath);
            }

            if (FileType == FREE_IMAGE_FORMAT.FIF_UNKNOWN)
            {
                Debug.Assert(false, string.Format("{0} format is unknown", FilePath));
                return DIB;
            }

            if (FreeImage.FIFSupportsReading(FileType))
            {
                DIB = FreeImage.Load(FileType, FilePath, FREE_IMAGE_LOAD_FLAGS.DEFAULT);
            }

            if(DIB == null)
            {
                Debug.Assert(false, "DIB is null");
            }
                        
            // get bit
            DIB = FreeImage.ConvertTo32Bits(DIB);

            // get width height
            Width = (int) FreeImage.GetWidth(DIB);
            Height = (int) FreeImage.GetHeight(DIB);

            return DIB;
        }
    }
}
