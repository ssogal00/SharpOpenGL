using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FreeImageAPI;
using System.Diagnostics;

namespace Core.Texture
{
    public static class FreeImageHelper
    {
        public static IntPtr Load(string FilePath, out int Width, out int Height)
        {
            FIBITMAP DIB = new FIBITMAP();

            Width = Height = 0;

            if (!File.Exists(FilePath))
            {
                Debug.Assert(false, string.Format("{0} not exist", FilePath));
                return new IntPtr();
            }

            FREE_IMAGE_FORMAT FileType = FreeImageAPI.FreeImage.GetFileType(FilePath, 0);

            if (FileType == FREE_IMAGE_FORMAT.FIF_UNKNOWN)
            {
                FileType = FreeImage.GetFIFFromFilename(FilePath);
            }

            if (FileType == FREE_IMAGE_FORMAT.FIF_UNKNOWN)
            {
                Debug.Assert(false, string.Format("{0} format is unknown", FilePath));
                return new IntPtr();
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
            IntPtr Bytes = FreeImage.GetBits(DIB);

            // get width height
            Width = (int) FreeImage.GetWidth(DIB);
            Height = (int) FreeImage.GetHeight(DIB);
            //

            
            

            return Bytes;
        }
    }
}
