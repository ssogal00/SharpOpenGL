using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeImageAPI;
using System.IO;

namespace Core.Texture
{
    public class ScopedFreeImage : IDisposable
    {
        public ScopedFreeImage(string imagePath)
        {
            if (File.Exists(imagePath) == false)
            {
                bitmap = FreeImageHelper.Load("./Resources/Texture/Checker.png", out Width, out Height);
            }
            else
            {
                bitmap = FreeImageHelper.Load(imagePath, out Width, out Height);
            }
            
            Bytes = FreeImage.GetBits(bitmap);
        }
        public void Dispose()
        {
            FreeImage.Unload(bitmap);
        }

        public IntPtr Bytes;
        public FIBITMAP bitmap;
        public int Width = 0;
        public int Height = 0;
    }
}
